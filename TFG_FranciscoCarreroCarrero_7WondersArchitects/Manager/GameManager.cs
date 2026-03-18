using TFG_FranciscoCarreroCarrero_7WondersArchitects.Domain;
using TFG_FranciscoCarreroCarrero_7WondersArchitects.Domain.Entities;

namespace TFG_FranciscoCarreroCarrero_7WondersArchitects.Manager {
    public class GameManager {
        public GameState State { get; private set; }

        public GameManager(string localPlayerName, string localPlayerWonderString) {
            State = new GameState();

            // 1. Parseamos la maravilla y creamos al jugador local
            if (Enum.TryParse(localPlayerWonderString, out Player.Wonder maravillaEnum)) {
                State.LocalPlayer = new Player(localPlayerName, maravillaEnum);
            }

            // 2. Preparamos el mazo central (tu código original)
            State.MainDeck = PreparacionCartas();

            // 3. Preparamos el mazo específico de la maravilla del jugador
            State.LocalPlayer.WonderDeck = GenerarMazoMaravilla(State.LocalPlayer.PlayerWonder);
        }

        public Card RobarCartaMazoPrincipal() {
            if (State.MainDeck.Count == 0) return null;

            State.LocalPlayer.HandDeck.Add(State.MainDeck[0]);
            State.MainDeck.RemoveAt(0);
            return State.LocalPlayer.HandDeck.Last();
        }
        public bool ComprobarConstruccion() {
            if (State.LocalPlayer.EtapaConstruccion > 4) return false;

            //numero cartas por recurso
            int maderas = State.LocalPlayer.HandDeck.Count(c => c.Resource == Card.ResourceType.Wood);
            int piedras = State.LocalPlayer.HandDeck.Count(c => c.Resource == Card.ResourceType.Stone);
            int arcillas = State.LocalPlayer.HandDeck.Count(c => c.Resource == Card.ResourceType.Clay);
            int papiros = State.LocalPlayer.HandDeck.Count(c => c.Resource == Card.ResourceType.Papyrus);
            int cristales = State.LocalPlayer.HandDeck.Count(c => c.Resource == Card.ResourceType.Glass);
            int oros = State.LocalPlayer.HandDeck.Count(c => c.Resource == Card.ResourceType.Gold);

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
            switch (State.LocalPlayer.EtapaConstruccion) {
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
                switch (State.LocalPlayer.EtapaConstruccion) {
                    case 0:
                        borrarCartas(2, true, recursoMayor);
                        break;
                    case 1:
                        borrarCartas(2, false, recursoMayor);
                        break;
                    case 2:
                        borrarCartas(3, true, recursoMayor);
                        break;
                    case 3:
                        borrarCartas(3, false, recursoMayor);
                        break;
                    case 4:
                        borrarCartas(4, true, recursoMayor);
                        break;
                }

                State.LocalPlayer.EtapaConstruccion++;
                return true;
            }
            return false;
        }

        public void borrarCartas(int cantidad, bool sonDiferentes, Card.ResourceType recursoMayor) {
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
            var carta = State.LocalPlayer.HandDeck.FirstOrDefault(c => c.Type == Card.CardType.Resource && c.Resource == tipo);
            return carta != null && State.LocalPlayer.HandDeck.Remove(carta);
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
            mazoPrincipal = AgregarCartasVictoria(mazoPrincipal, 2, true, 8, 0);   // Índices del 0 al 7
            mazoPrincipal = AgregarCartasVictoria(mazoPrincipal, 3, false, 4, 8);  // Índices del 8 al 11


            var arrayMazoPrincipal = mazoPrincipal.ToArray();
            Random.Shared.Shuffle(arrayMazoPrincipal);//shuffle no esta para listas
            mazoPrincipal = arrayMazoPrincipal.ToList();

            return mazoPrincipal;
        }

        private List<Card> GenerarMazoMaravilla(Player.Wonder maravilla) {
            List<Card> mazo = new List<Card>();

            switch (maravilla) {
                case Player.Wonder.Guiza:
                    // Rellenar...
                    break;
                case Player.Wonder.Alejandria:
                    // Rellenar...
                    break;
                case Player.Wonder.Babilonia:
                    // Rellenar...
                    break;
                case Player.Wonder.Efeso:
                    // Rellenar...
                    break;
                case Player.Wonder.Halicarnaso:
                    // Rellenar...
                    break;
                case Player.Wonder.Olimpia:
                    // Rellenar...
                    break;
                case Player.Wonder.Rodas:
                    // Rellenar...
                    break;
            }

            // Barajamos el mazo de la maravilla
            var arrayMazo = mazo.ToArray();
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
        private List<Card> AgregarCartasVictoria(List<Card> deck, int victoryPoints, bool hasCat, int cantidad, int indiceInicio = 0) {
            for (int i = 0; i < cantidad; i++) {
                int indiceActual = indiceInicio + i;
                deck.Add(new Card("MazoPrincipal_VictoryPoint" + indiceActual, victoryPoints, hasCat));
            }
            return deck;

        }

    }

}
