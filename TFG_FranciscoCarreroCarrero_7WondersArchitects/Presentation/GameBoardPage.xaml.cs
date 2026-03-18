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

        public GameBoardPage(SignalRService signalRService, string nombreJugador, string maravillaJugador) {
            InitializeComponent();
            _signalRService = signalRService;
            _gameManager = new Manager.GameManager(nombreJugador, maravillaJugador);

            _signalRService.OnMessageReceived += MostrarAviso;
            PrepararTablero(maravillaJugador);
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

        private void PrepararTablero(string maravillaJugador) {
            string nombreElementoVertical = $"{maravillaJugador}Wonder";

            var elementoVertical = (VisualElement)this.FindByName(nombreElementoVertical);

            elementoVertical.IsVisible = true;
        }

        private void ActualizarImagenesMaravilla(int etapa) {
            string nombreMaravilla = _gameManager.State.LocalPlayer.PlayerWonder.ToString();

            string idElementoImage = $"{nombreMaravilla}Part{etapa - 1}";

            string nombreImagenNueva = $"{nombreMaravilla.ToLower()}{etapa - 1}b.png";

            var elementoImage = (Image)this.FindByName(idElementoImage);

            elementoImage.Source = nombreImagenNueva;
        }





        private void GetMainDeckCard(object sender, EventArgs e) {
            Card cartaRobada = _gameManager.RobarCartaMazoPrincipal();
            ProcesarRoboYConstruccion(cartaRobada);
        }

        private void GetLocalWonderDeckCard(object sender, EventArgs e) {
            Card cartaRobada = _gameManager.RobarCartaMazoMaravilla();
            ProcesarRoboYConstruccion(cartaRobada);
        }

        private void GetRemoteWonderDeckCard(object sender, EventArgs e) {
            Card cartaRobada = _gameManager.RobarCartaMazoMaravilla();
            ProcesarRoboYConstruccion(cartaRobada);
        }


        private void ProcesarRoboYConstruccion(Card cartaRobada) {
            if (cartaRobada != null) {
                DisplayAlert("Has robado", cartaRobada.ToString(), "OK");
            } else {
                DisplayAlert("Aviso", "Este mazo está vacío.", "OK");
                return; // Si no hay carta, no comprobamos nada más
            }

            bool seHaConstruidoAlgo = _gameManager.ComprobarConstruccion();

            if (seHaConstruidoAlgo) {
                int etapaRecienCompletada = _gameManager.State.LocalPlayer.EtapaConstruccion;
                ActualizarImagenesMaravilla(etapaRecienCompletada);
                DisplayAlert("Nueva etapa!", $"Has completado la parte {etapaRecienCompletada} de tu maravilla", "Ok");
            }
        }



        //sacar popup de mazoPropio
        private void ButtonShowPlayerDeck(object sender, EventArgs e) {
            //para probar
            //var popup = new PlayerDeckPopup(_gameManager.State.MainDeck);
            //var popup = new PlayerDeckPopup(_gameManager.State.LocalPlayer.WonderDeck);


            var popup = new PlayerDeckPopup(_gameManager.State.LocalPlayer.HandDeck);
            this.ShowPopup(popup);
        }

    }
}
