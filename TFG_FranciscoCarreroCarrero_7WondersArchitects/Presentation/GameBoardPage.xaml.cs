using CommunityToolkit.Maui.Extensions;
using TFG_FranciscoCarreroCarrero_7WondersArchitects.Domain.Entities;
using TFG_FranciscoCarreroCarrero_7WondersArchitects.Presentation;
using TFG_FranciscoCarreroCarrero_7WondersArchitects.Services;
using TFG_FranciscoCarreroCarrero_7WondersArchitects.Manager;
using static Microsoft.Maui.ApplicationModel.Permissions;

namespace TFG_FranciscoCarreroCarrero_7WondersArchitects
{
    public partial class GameBoardPage : ContentPage
    {
        private readonly GameManager _gameManager;
        private readonly SignalRService _signalRService;
        private string _roomCode;

        public GameBoardPage(SignalRService signalRService, string nombreJugador, string maravillaJugador, string roomCode) {
            InitializeComponent();
            _signalRService = signalRService;
            _gameManager = new Manager.GameManager(nombreJugador, maravillaJugador);
            _roomCode = roomCode;

            //me suscribo al evento de mostrar aviso
            _signalRService.OnMessageReceived += MostrarAviso;
            
            //me suscribo al evento de recibir estado del rival y actualizar tablero con jugadas hechas
            _signalRService.OnGameStateReceived += RecibirEstadoDelRival;
            _gameManager.OnStateUpdated += RepintarTablero;

            _signalRService.OnPlayerJoined += RivalSeHaUnido;

            //evento de resolucion de guerra
            _signalRService.OnGameNotificationReceived += MostrarAlertaGuerra;
            _gameManager.OnGuerraFinalizada += EnviarResultadoGuerra;

            PrepararTablero(maravillaJugador);
            RepintarTablero();
        }

        //avisador multijugador
        private void MostrarAviso(string user, string message) {
            //es el mainthread el que toca la ui
            MainThread.BeginInvokeOnMainThread(async () => {
                // Lanzamos el diálogo simple y ya está
                await this.DisplayAlert($"Aviso de {user}", message, "Genial");
            });
        }

        /*
        //para desuscribirnos del evento, al unirse una persona
        protected override void OnDisappearing() {
            base.OnDisappearing();
            // Nos desuscribimos al salir para evitar errores
            _signalRService.OnMessageReceived -= MostrarAviso;
        }
         */


        private void RecibirEstadoDelRival(string jsonState) {
            //si hemos recibido el estado, que el manager lo gestione
            _gameManager.OverwriteStateFromJson(jsonState);
        }
        
        private void RepintarTablero() {
            //esto saltara cada vez que el manager machaque su antiguo gameState con el nuevo recibido
            MainThread.BeginInvokeOnMainThread(() => {
                //pintar fichas paz/guerra
                int avisos = _gameManager.AvisosGuerra;
                FichaConflicto1.Source = avisos >= 1 ? "ficha_conflicto_guerra.png" : "ficha_conflicto_paz.png";
                FichaConflicto2.Source = avisos >= 2 ? "ficha_conflicto_guerra.png" : "ficha_conflicto_paz.png";
                FichaConflicto3.Source = avisos >= 3 ? "ficha_conflicto_guerra.png" : "ficha_conflicto_paz.png";

                //comprobar si se termino
                if (_gameManager.IsGameOver) {
                    lblTurn.Text = "¡PARTIDA FINALIZADA!";
                    lblTurn.TextColor = Colors.Gold; 

                    MainDeck.IsEnabled = false;
                    LocalWonderDeck.IsEnabled = false;
                    RemoteWonderDeck.IsEnabled = false;

                    var popup = new PlayerDeckPopup(_gameManager.LocalPlayer, _gameManager.RemotePlayer, true);
                    this.ShowPopup(popup);
                }

                //"semaforo"
                bool meToca = _gameManager.IsLocalPlayerTurn;
                if (meToca) {
                    lblTurn.Text = "ES TU TURNO";
                    lblTurn.TextColor = Colors.Green;
                } else {
                    lblTurn.Text = "TURNO DEL RIVAL";
                    lblTurn.TextColor = Colors.Red;
                }
                MainDeck.IsEnabled = meToca;
                LocalWonderDeck.IsEnabled = meToca;
                RemoteWonderDeck.IsEnabled = meToca;
            });
        }

        private async void EnviarEstadoAlRival() {
            //terminamos turno, enviamos gameState y que juegue el rival
            string json = _gameManager.ExportStateToJson();
            await _signalRService.SendGameStateAsync(_roomCode, json);
        }

        private void RivalSeHaUnido(string nombreRival, string maravillaRival) {
            _gameManager.RegistrarRival(nombreRival, maravillaRival);

            RepintarTablero();

            EnviarEstadoAlRival();
        }


        private void PrepararTablero(string maravillaJugador) {
            string nombreElementoVertical = $"{maravillaJugador}Wonder";

            var elementoVertical = (VerticalStackLayout)this.FindByName(nombreElementoVertical);

            elementoVertical.IsVisible = true;
        }

        private void ActualizarImagenesMaravilla(int etapa, string nombreMaravilla) {
            string idElementoImage = $"{nombreMaravilla}Part{etapa - 1}";

            string nombreImagenNueva = $"{nombreMaravilla.ToLower()}{etapa - 1}b.png";

            var elementoImage = (Image)this.FindByName(idElementoImage);

            elementoImage.Source = nombreImagenNueva;
        }





        private async void GetMainDeckCard(object sender, EventArgs e) {
            Card cartaRobada = _gameManager.RobarCartaMazoPrincipal();
            await ProcesarRoboYConstruccion(cartaRobada);
            
            _gameManager.FinalizarTurno();
            RepintarTablero();
            EnviarEstadoAlRival();
        }

        private async void GetLocalWonderDeckCard(object sender, EventArgs e) {
            Card cartaRobada = _gameManager.RobarCartaMazoMaravillaLocal();
            await ProcesarRoboYConstruccion(cartaRobada);
            
            _gameManager.FinalizarTurno();
            RepintarTablero();
            EnviarEstadoAlRival();
        }

        private async void GetRemoteWonderDeckCard(object sender, EventArgs e) {
            Card cartaRobada = _gameManager.RobarCartaMazoMaravillaRival();
            await ProcesarRoboYConstruccion(cartaRobada);

            _gameManager.FinalizarTurno();
            RepintarTablero();
            EnviarEstadoAlRival();
        }


        private async Task ProcesarRoboYConstruccion(Card cartaRobada) {
            if (cartaRobada != null) {
                await DisplayAlert("Has robado", cartaRobada.ToString(), "OK");
            } else {
                await DisplayAlert("Aviso", "Este mazo está vacío.", "OK");
                return; 
            }
            
            _gameManager.EvaluarGuerra(cartaRobada);

            bool seHaConstruidoAlgo = _gameManager.ComprobarConstruccion();

            if (seHaConstruidoAlgo) {
                int etapaRecienCompletada = _gameManager.EtapaActual;
                string nombreMaravilla = _gameManager.MaravillaJugador.ToString();
                ActualizarImagenesMaravilla(etapaRecienCompletada, nombreMaravilla);
                await DisplayAlert("Nueva etapa!", $"Has completado la parte {etapaRecienCompletada} de tu maravilla ({nombreMaravilla})", "Ok");
            }
        }

        private async void EnviarResultadoGuerra(string texto) {
            await _signalRService.SendGameNotificationAsync(_roomCode, texto);
        }

        private void MostrarAlertaGuerra(string mensaje) {
            MainThread.BeginInvokeOnMainThread(async () => {
                await this.DisplayAlert("Se acabo la paz!", mensaje, "Aceptar");
            });
        }


        //sacar popup de mazoPropio
        private void ButtonShowPlayerDeck(object sender, EventArgs e) {
            //para probar
            //var popup = new PlayerDeckPopup(_gameManager.State.MainDeck);
            //var popup = new PlayerDeckPopup(_gameManager._state.LocalPlayer.WonderDeck);

            var local = _gameManager.LocalPlayer;
            var rival = _gameManager.RemotePlayer;

            var popup = new PlayerDeckPopup(local, rival, _gameManager.IsGameOver);
            this.ShowPopup(popup);
        }

    }
}
