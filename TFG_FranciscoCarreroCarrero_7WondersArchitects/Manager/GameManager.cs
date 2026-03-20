using TFG_FranciscoCarreroCarrero_7WondersArchitects.Domain;
using TFG_FranciscoCarreroCarrero_7WondersArchitects.Domain.Entities;
using System.Text.Json; 
using System.Text.Json.Serialization;

namespace TFG_FranciscoCarreroCarrero_7WondersArchitects.Manager {
    public class GameManager {
        private readonly JsonSerializerOptions _jsonOptions;
        public event Action? OnStateUpdated;

        private GameState _state;

        public bool IsLocalPlayerTurn => _state.IsLocalPlayerTurn;
        public int EtapaActual => _state.LocalPlayer.EtapaConstruccion;
        public string NombreJugador => _state.LocalPlayer.Name;
        public Wonder.WonderType MaravillaJugador => _state.LocalPlayer.PlayerWonder.Type;
        public int CartasMazoCentral => _state.MainDeck.Count;
        public int CartasLocalMazoMaravilla => _state.LocalPlayer.WonderDeck.Count;
        public int CartasRivalMazoMaravilla => _state.RemotePlayer.WonderDeck.Count;
        public List<Card> ManoJugador => _state.LocalPlayer.HandDeck;
        public Player LocalPlayer => _state.LocalPlayer;
        public Player RemotePlayer => _state.RemotePlayer;
        //para ver si se acabo la partida
        public bool IsGameOver => _state.LocalPlayer.EtapaConstruccion >= 5 ||
                         (_state.RemotePlayer != null && _state.RemotePlayer.EtapaConstruccion >= 5);


        public GameManager(string localPlayerName, string localPlayerWonderString) {
            _jsonOptions = new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = ReferenceHandler.Preserve,
                IncludeFields = true
            };

            _state = new GameState();

            //preparamos mazo central
            _state.MainDeck = PreparacionCartas();

            //parseamos la maravilla a enum y creamos jugador local
            _state.LocalPlayer = new Player(localPlayerName, Enum.Parse<Wonder.WonderType>(localPlayerWonderString));

            //preparamos mazoMaravilla del jugador local
            _state.LocalPlayer.WonderDeck = GenerarMazoMaravilla(_state.LocalPlayer.PlayerWonder.Type);

            //arrancamos sin turno por defecto
            _state.IsLocalPlayerTurn = false;
        }


        public Card RobarCartaMazoPrincipal() {
            if (_state.MainDeck.Count == 0) return null;

            _state.LocalPlayer.HandDeck.Add(_state.MainDeck[0]);
            _state.MainDeck.RemoveAt(0);
            return _state.LocalPlayer.HandDeck.Last();
        }

        public Card RobarCartaMazoMaravillaLocal() {
            if (_state.LocalPlayer.WonderDeck.Count == 0) return null;

            _state.LocalPlayer.HandDeck.Add(_state.LocalPlayer.WonderDeck[0]);
            _state.LocalPlayer.WonderDeck.RemoveAt(0);
            return _state.LocalPlayer.HandDeck.Last();
        }

        public Card RobarCartaMazoMaravillaRival() {
            if (_state.RemotePlayer.WonderDeck.Count == 0) return null;

            _state.LocalPlayer.HandDeck.Add(_state.RemotePlayer.WonderDeck[0]);
            _state.RemotePlayer.WonderDeck.RemoveAt(0);
            return _state.LocalPlayer.HandDeck.Last();
        }

        public bool ComprobarConstruccion() {
            if (_state.LocalPlayer.EtapaConstruccion > 4) return false;

            //numero cartas por recurso
            int maderas = _state.LocalPlayer.HandDeck.Count(c => c.Resource == Card.ResourceType.Wood);
            int piedras = _state.LocalPlayer.HandDeck.Count(c => c.Resource == Card.ResourceType.Stone);
            int arcillas = _state.LocalPlayer.HandDeck.Count(c => c.Resource == Card.ResourceType.Clay);
            int papiros = _state.LocalPlayer.HandDeck.Count(c => c.Resource == Card.ResourceType.Papyrus);
            int cristales = _state.LocalPlayer.HandDeck.Count(c => c.Resource == Card.ResourceType.Glass);
            int oros = _state.LocalPlayer.HandDeck.Count(c => c.Resource == Card.ResourceType.Gold);

            int[] recursos = { maderas, piedras, arcillas, papiros, cristales };

            int diferentes = recursos.Count(c => c > 0);

            //para pasarle a borrar
            int maxIguales = maderas;
            Card.ResourceType recursoMayor = Card.ResourceType.Wood;

            if (piedras > maxIguales) { maxIguales = piedras; recursoMayor = Card.ResourceType.Stone; }
            if (arcillas > maxIguales) { maxIguales = arcillas; recursoMayor = Card.ResourceType.Clay; }
            if (papiros > maxIguales) { maxIguales = papiros; recursoMayor = Card.ResourceType.Papyrus; }
            if (cristales > maxIguales) { maxIguales = cristales; recursoMayor = Card.ResourceType.Glass; }

            bool puedeConstruir = false;

            //construyo segun etapaConstruccion
            switch (_state.LocalPlayer.EtapaConstruccion) {
                case 0: // 2 diferentes
                    puedeConstruir = (diferentes + oros) >= 2;
                    break;
                case 1: // 2 iguales
                    puedeConstruir = (maxIguales + oros) >= 2;
                    break;
                case 2: // 3 diferentes
                    puedeConstruir = (diferentes + oros) >= 3;
                    break;
                case 3: // 3 iguales
                    puedeConstruir = (maxIguales + oros) >= 3;
                    break;
                case 4: // 4 diferentes
                    puedeConstruir = (diferentes + oros) >= 4;
                    break;
            }

            //cambiar imagen (construir para el usuario)
            if (puedeConstruir) {
                switch (_state.LocalPlayer.EtapaConstruccion) {
                    case 0:
                        BorrarCartas(2, true, recursoMayor);
                        break;
                    case 1:
                        BorrarCartas(2, false, recursoMayor);
                        break;
                    case 2:
                        BorrarCartas(3, true, recursoMayor);
                        break;
                    case 3:
                        BorrarCartas(3, false, recursoMayor);
                        break;
                    case 4:
                        BorrarCartas(4, true, recursoMayor);
                        break;  
                }

                _state.LocalPlayer.EtapaConstruccion++;

                EvaluarHabilidadesMaravilla();

                return true;
            }
            return false;
        }

        private void EvaluarHabilidadesMaravilla() {
            Wonder.WonderType tipo = _state.LocalPlayer.PlayerWonder.Type;
            int etapaRecienConstruida = _state.LocalPlayer.EtapaConstruccion;

            if (tipo == Wonder.WonderType.Efeso && (etapaRecienConstruida == 2 || etapaRecienConstruida == 3 || etapaRecienConstruida == 4)) {
                RobarCartaMazoPrincipal();
            }

            if (tipo == Wonder.WonderType.Olimpia && (etapaRecienConstruida == 2 || etapaRecienConstruida == 4)) {
                RobarCartaMazoMaravillaLocal();
                RobarCartaMazoMaravillaRival();
            }

            //sin hacer

            if (tipo == Wonder.WonderType.Alejandria && (etapaRecienConstruida == 2 || etapaRecienConstruida == 4)) {
                //continua su turno, puede robar otra vez de cualquier mazo
            }

            if (tipo == Wonder.WonderType.Babilonia && (etapaRecienConstruida == 2 || etapaRecienConstruida == 4)) {
                //continua su turno, puede robar cualquier ficha de progreso
            }

            if (tipo == Wonder.WonderType.Rodas && (etapaRecienConstruida == 1 || etapaRecienConstruida == 4)) {
                //suma 1 escudo a player
            }

            if (tipo == Wonder.WonderType.Halicarnaso && (etapaRecienConstruida == 2 || etapaRecienConstruida == 4)) {
                //escoge mazo izq o derecha, se muestran las 5 cartas, elige y baraja despues
                //muy opcional, si no que sea robar central y local
            }




        }

        public void BorrarCartas(int cantidad, bool sonDiferentes, Card.ResourceType recursoMayor) {
            int borradas = 0;

            if (sonDiferentes) {
                //si diferentes se va borrando de cada recurso en cascada, excepto si no tiene, que devuelve false y sigue para abajo
                if (borradas < cantidad && BorrarUna(Card.ResourceType.Wood)) borradas++;
                if (borradas < cantidad && BorrarUna(Card.ResourceType.Stone)) borradas++;
                if (borradas < cantidad && BorrarUna(Card.ResourceType.Clay)) borradas++;
                if (borradas < cantidad && BorrarUna(Card.ResourceType.Papyrus)) borradas++;
                if (borradas < cantidad && BorrarUna(Card.ResourceType.Glass)) borradas++;
            } else {
                //si iguales se pasa "recursoMayor" "cantidad" veces
                while (borradas < cantidad && BorrarUna(recursoMayor)) {
                    borradas++;
                }
            }

            // si al llegar abajo faltan es porque tiene suficiente oro, lo borramos (si no no llegaria a borrarCartas)¡
            while (borradas < cantidad) {
                BorrarUna(Card.ResourceType.Gold);
                borradas++;
            }
        }

        //metodo que devuelve true o false segun borrado
        private bool BorrarUna(Card.ResourceType tipo) {
            //cojo la carta de tipo recurso y de recursoTipo args
            var carta = _state.LocalPlayer.HandDeck.FirstOrDefault(c => c.Type == Card.CardType.Resource && c.Resource == tipo);
            return carta != null && _state.LocalPlayer.HandDeck.Remove(carta);
        }


        private List<Card> PreparacionCartas() {
            List<Card> mazoPrincipal = new List<Card>();

            //cartas recursos
            var resourceTypes = Card.GetAllResourceTypes();
            foreach (var tipoRecurso in resourceTypes) {
                int cantidad = (tipoRecurso == Card.ResourceType.Gold) ? 6 : 4;
                mazoPrincipal = AgregarCartasRecurso(mazoPrincipal, tipoRecurso, cantidad);
            }

            //cartas militares
            mazoPrincipal = AgregarCartasMilitares(mazoPrincipal, 0, 4);
            mazoPrincipal = AgregarCartasMilitares(mazoPrincipal, 1, 4);
            mazoPrincipal = AgregarCartasMilitares(mazoPrincipal, 2, 2);


            //cartas ciencia
            var scienceTypes = Card.GetAllScienceTypes();
            foreach (var tipoCiencia in scienceTypes) {
                mazoPrincipal = AgregarCartasCiencia(mazoPrincipal, tipoCiencia, 4);
            }


            //cartas victory point
            mazoPrincipal = AgregarCartasVictoria(mazoPrincipal, true, 8, 0);   // Índices del 0 al 7
            mazoPrincipal = AgregarCartasVictoria(mazoPrincipal, false, 4, 8);  // Índices del 8 al 11


            var arrayMazoPrincipal = mazoPrincipal.ToArray();
            Random.Shared.Shuffle(arrayMazoPrincipal);//shuffle no esta para listas
            mazoPrincipal = arrayMazoPrincipal.ToList();

            return mazoPrincipal;
        }

        private List<Card> GenerarMazoMaravilla(Wonder.WonderType maravilla) {
            List<Card> mazoMaravilla = new List<Card>();

            switch (maravilla) {
                case Wonder.WonderType.Guiza:
                    //cartas recursos
                    foreach (var tipoRecurso in Card.GetAllResourceTypes()) {
                        int cantidad = (tipoRecurso == Card.ResourceType.Gold) ? 3 : (tipoRecurso == Card.ResourceType.Clay) ? 1 : 2;
                        mazoMaravilla = AgregarCartasRecurso(mazoMaravilla, tipoRecurso, cantidad);
                    }

                    //cartas militares
                    mazoMaravilla = AgregarCartasMilitares(mazoMaravilla, 0, 2);
                    mazoMaravilla = AgregarCartasMilitares(mazoMaravilla, 1, 2);
                    mazoMaravilla = AgregarCartasMilitares(mazoMaravilla, 2, 0);

                    //cartas ciencia
                    mazoMaravilla = AgregarCartasCiencia(mazoMaravilla, Card.ScienceType.Compass, 2);
                    mazoMaravilla = AgregarCartasCiencia(mazoMaravilla, Card.ScienceType.Gear, 1);
                    mazoMaravilla = AgregarCartasCiencia(mazoMaravilla, Card.ScienceType.Tablet, 1);

                    //cartas victory point
                    mazoMaravilla = AgregarCartasVictoria(mazoMaravilla, true, 3, 12);   // Índices del 0 al 7
                    mazoMaravilla = AgregarCartasVictoria(mazoMaravilla, false, 2, 15);  // Índices del 8 al 11
                    break;

                case Wonder.WonderType.Alejandria:
                    //cartas recursos
                    foreach (var tipoRecurso in Card.GetAllResourceTypes()) {
                        int cantidad = (tipoRecurso == Card.ResourceType.Gold) ? 4 : (tipoRecurso == Card.ResourceType.Glass) ? 1 : 2;
                        mazoMaravilla = AgregarCartasRecurso(mazoMaravilla, tipoRecurso, cantidad); 
                    }

                    //cartas militares
                    mazoMaravilla = AgregarCartasMilitares(mazoMaravilla, 0, 2);
                    mazoMaravilla = AgregarCartasMilitares(mazoMaravilla, 1, 1);
                    mazoMaravilla = AgregarCartasMilitares(mazoMaravilla, 2, 1);

                    //cartas ciencia
                    mazoMaravilla = AgregarCartasCiencia(mazoMaravilla, Card.ScienceType.Compass, 1);
                    mazoMaravilla = AgregarCartasCiencia(mazoMaravilla, Card.ScienceType.Gear, 2);
                    mazoMaravilla = AgregarCartasCiencia(mazoMaravilla, Card.ScienceType.Tablet, 1);

                    //cartas victory point
                    mazoMaravilla = AgregarCartasVictoria(mazoMaravilla, true, 2, 12);   // Índices del 0 al 7
                    mazoMaravilla = AgregarCartasVictoria(mazoMaravilla, false, 2, 15);  // Índices del 8 al 11
                    break;

                case Wonder.WonderType.Babilonia:
                    //cartas recursos
                    foreach (var tipoRecurso in Card.GetAllResourceTypes()) {
                        int cantidad = (tipoRecurso == Card.ResourceType.Gold) ? 3 : (tipoRecurso == Card.ResourceType.Stone) ? 1 : 2;
                        mazoMaravilla = AgregarCartasRecurso(mazoMaravilla, tipoRecurso, cantidad);
                    }

                    //cartas militares
                    mazoMaravilla = AgregarCartasMilitares(mazoMaravilla, 0, 2);
                    mazoMaravilla = AgregarCartasMilitares(mazoMaravilla, 1, 1);
                    mazoMaravilla = AgregarCartasMilitares(mazoMaravilla, 2, 1);

                    //cartas ciencia
                    mazoMaravilla = AgregarCartasCiencia(mazoMaravilla, Card.ScienceType.Compass, 2);
                    mazoMaravilla = AgregarCartasCiencia(mazoMaravilla, Card.ScienceType.Gear, 2);
                    mazoMaravilla = AgregarCartasCiencia(mazoMaravilla, Card.ScienceType.Tablet, 1);

                    //cartas victory point
                    mazoMaravilla = AgregarCartasVictoria(mazoMaravilla, true, 2, 12);   // Índices del 0 al 7
                    mazoMaravilla = AgregarCartasVictoria(mazoMaravilla, false, 2, 15);  // Índices del 8 al 11
                    break;

                case Wonder.WonderType.Efeso:
                    //cartas recursos
                    foreach (var tipoRecurso in Card.GetAllResourceTypes()) {
                        int cantidad = (tipoRecurso == Card.ResourceType.Gold) ? 3 : 2;
                        mazoMaravilla = AgregarCartasRecurso(mazoMaravilla, tipoRecurso, cantidad);
                    }

                    //cartas militares
                    mazoMaravilla = AgregarCartasMilitares(mazoMaravilla, 0, 2);
                    mazoMaravilla = AgregarCartasMilitares(mazoMaravilla, 1, 1);
                    mazoMaravilla = AgregarCartasMilitares(mazoMaravilla, 2, 1);

                    //cartas ciencia
                    mazoMaravilla = AgregarCartasCiencia(mazoMaravilla, Card.ScienceType.Compass, 2);
                    mazoMaravilla = AgregarCartasCiencia(mazoMaravilla, Card.ScienceType.Gear, 1);
                    mazoMaravilla = AgregarCartasCiencia(mazoMaravilla, Card.ScienceType.Tablet, 2);

                    //cartas victory point
                    mazoMaravilla = AgregarCartasVictoria(mazoMaravilla, true, 2, 12);   // Índices del 0 al 7
                    mazoMaravilla = AgregarCartasVictoria(mazoMaravilla, false, 1, 15);  // Índices del 8 al 11
                    break;

                case Wonder.WonderType.Halicarnaso:
                    //cartas recursos
                    foreach (var tipoRecurso in Card.GetAllResourceTypes()) {
                        int cantidad = (tipoRecurso == Card.ResourceType.Gold) ? 3 : (tipoRecurso == Card.ResourceType.Papyrus) ? 1 : 2;
                        mazoMaravilla = AgregarCartasRecurso(mazoMaravilla, tipoRecurso, cantidad);
                    }

                    //cartas militares
                    mazoMaravilla = AgregarCartasMilitares(mazoMaravilla, 0, 2);
                    mazoMaravilla = AgregarCartasMilitares(mazoMaravilla, 1, 2);
                    mazoMaravilla = AgregarCartasMilitares(mazoMaravilla, 2, 1);

                    //cartas ciencia
                    mazoMaravilla = AgregarCartasCiencia(mazoMaravilla, Card.ScienceType.Compass, 1);
                    mazoMaravilla = AgregarCartasCiencia(mazoMaravilla, Card.ScienceType.Gear, 1);
                    mazoMaravilla = AgregarCartasCiencia(mazoMaravilla, Card.ScienceType.Tablet, 2);

                    //cartas victory point
                    mazoMaravilla = AgregarCartasVictoria(mazoMaravilla, true, 2, 12);   // Índices del 0 al 7
                    mazoMaravilla = AgregarCartasVictoria(mazoMaravilla, false, 2, 15);  // Índices del 8 al 11
                    break;

                case Wonder.WonderType.Olimpia:
                    //cartas recursos
                    foreach (var tipoRecurso in Card.GetAllResourceTypes()) {
                        int cantidad = (tipoRecurso == Card.ResourceType.Gold) ? 3 : (tipoRecurso == Card.ResourceType.Wood) ? 1 : 2;
                        mazoMaravilla = AgregarCartasRecurso(mazoMaravilla, tipoRecurso, cantidad);
                    }

                    //cartas militares
                    mazoMaravilla = AgregarCartasMilitares(mazoMaravilla, 0, 2);
                    mazoMaravilla = AgregarCartasMilitares(mazoMaravilla, 1, 2);
                    mazoMaravilla = AgregarCartasMilitares(mazoMaravilla, 2, 1);

                    //cartas ciencia
                    mazoMaravilla = AgregarCartasCiencia(mazoMaravilla, Card.ScienceType.Compass, 1);
                    mazoMaravilla = AgregarCartasCiencia(mazoMaravilla, Card.ScienceType.Gear, 1);
                    mazoMaravilla = AgregarCartasCiencia(mazoMaravilla, Card.ScienceType.Tablet, 2);

                    //cartas victory point
                    mazoMaravilla = AgregarCartasVictoria(mazoMaravilla, true, 3, 12);   // Índices del 0 al 7
                    mazoMaravilla = AgregarCartasVictoria(mazoMaravilla, false, 1, 15);  // Índices del 8 al 11
                    break;


                case Wonder.WonderType.Rodas:
                    //cartas recursos
                    foreach (var tipoRecurso in Card.GetAllResourceTypes()) {
                        int cantidad = 2;
                        mazoMaravilla = AgregarCartasRecurso(mazoMaravilla, tipoRecurso, cantidad);
                    }

                    //cartas militares
                    mazoMaravilla = AgregarCartasMilitares(mazoMaravilla, 0, 2);
                    mazoMaravilla = AgregarCartasMilitares(mazoMaravilla, 1, 2);
                    mazoMaravilla = AgregarCartasMilitares(mazoMaravilla, 2, 1);

                    //cartas ciencia
                    mazoMaravilla = AgregarCartasCiencia(mazoMaravilla, Card.ScienceType.Compass, 1);
                    mazoMaravilla = AgregarCartasCiencia(mazoMaravilla, Card.ScienceType.Gear, 2);
                    mazoMaravilla = AgregarCartasCiencia(mazoMaravilla, Card.ScienceType.Tablet, 1);

                    //cartas victory point
                    mazoMaravilla = AgregarCartasVictoria(mazoMaravilla, true, 2, 12);   // Índices del 0 al 7
                    mazoMaravilla = AgregarCartasVictoria(mazoMaravilla, false, 2, 15);  // Índices del 8 al 11
                    break;
            }

            // Barajamos el mazo de la maravilla
            var arrayMazo = mazoMaravilla.ToArray();
            Random.Shared.Shuffle(arrayMazo);
            return arrayMazo.ToList();
        }


        //cartas recursos
        private List<Card> AgregarCartasRecurso(List<Card> deck, Card.ResourceType subtipo, int cantidad) {
            for (int i = 0; i < cantidad; i++) {
                deck.Add(new Card("MazoPrincipal_" + subtipo.ToString() + i, subtipo));
            }
            return deck;
        }

        //cartas militares
        private List<Card> AgregarCartasMilitares(List<Card> deck, int horns, int quantity) {
            for (int i = 0; i < quantity; i++) {
                deck.Add(new Card("MazoPrincipal_Military" + horns + "_" + i, horns));
            }
            return deck;
        }

        //cartas ciencia
        private List<Card> AgregarCartasCiencia(List<Card> deck, Card.ScienceType subtipo, int cantidad) {
            for (int i = 0; i < cantidad; i++) {
                deck.Add(new Card("MazoPrincipal_" + subtipo.ToString() + i, subtipo));
            }
            return deck;
        }

        //cartas victory point
        private List<Card> AgregarCartasVictoria(List<Card> deck, bool hasCat, int cantidad, int indiceInicio = 0) {
            for (int i = 0; i < cantidad; i++) {
                int indiceActual = indiceInicio + i;
                deck.Add(new Card("MazoPrincipal_VictoryPoint" + indiceActual, hasCat));
            }
            return deck;

        }


        public string ExportStateToJson() {
            return JsonSerializer.Serialize(_state, _jsonOptions);
        }

        public void OverwriteStateFromJson(string jsonState) {
            if (string.IsNullOrWhiteSpace(jsonState)) return;

            var newState = JsonSerializer.Deserialize<GameState>(jsonState, _jsonOptions);

            if (newState != null) {
                //el que me pasa el state (el rival) deja de ser local y se convierte en visitante
                var temp = newState.LocalPlayer;
                newState.LocalPlayer = newState.RemotePlayer;
                newState.RemotePlayer = temp;

                //cambiamos el turno
                newState.IsLocalPlayerTurn = !newState.IsLocalPlayerTurn;

                //machacamos estado antiguo
                _state = newState;
                
                //si ya termino la partida ya no es tu turno
                if (IsGameOver) _state.IsLocalPlayerTurn = false;

                //repintamos ui
                OnStateUpdated?.Invoke();
            }
        }

        public void RegistrarRival(string nombreRival, string maravillaRival) {
            _state.RemotePlayer = new Player(nombreRival, Enum.Parse<Wonder.WonderType>(maravillaRival));

            _state.RemotePlayer.WonderDeck = GenerarMazoMaravilla(Enum.Parse<Wonder.WonderType>(maravillaRival));

            _state.IsLocalPlayerTurn = true;
        }

        public void FinalizarTurno() {
            _state.IsLocalPlayerTurn = false;
        }

        

    }

}
