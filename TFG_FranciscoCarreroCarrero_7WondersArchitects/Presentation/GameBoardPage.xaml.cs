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
            switch (maravillaJugador) {
                case "Guiza":
                    GuizaWonder.IsVisible = true;
                    
                    break;
                case "Alejandria":
                    AlejandriaWonder.IsVisible = true;
                    
                    break;
                case "Babilonia":
                    BabiloniaWonder.IsVisible = true;
                    
                    break;
                case "Efeso":
                    EfesoWonder.IsVisible = true;
                    
                    break;
                case "Halicarnaso":
                    HalicarnasoWonder.IsVisible = true;
                    
                    break;
                case "Olimpia":
                    OlimpiaWonder.IsVisible = true;
                    
                    break;
                case "Rodas":
                    RodasWonder.IsVisible = true;
                    
                    break;
            }
        }

        private void ActualizarImagenesMaravilla(int etapa) {
            switch (etapa) {
                case 1: GuizaPart0.Source = "guiza0b.png"; break;
                case 2: GuizaPart1.Source = "guiza1b.png"; break;
                case 3: GuizaPart2.Source = "guiza2b.png"; break;
                case 4: GuizaPart3.Source = "guiza3b.png"; break;
                case 5: GuizaPart4.Source = "guiza4b.png"; break;
            }
        }





        private void Button_Clicked(object sender, EventArgs e) {
            //robamos
            Card cartaRobada = _gameManager.RobarCartaMazoPrincipal();

            if (cartaRobada != null) {
                DisplayAlert("Has robado", cartaRobada.ToString(), "OK");
            }
            
            //comprobamos
            bool seHaConstruidoAlgo = _gameManager.ComprobarConstruccion();

            //si es correcto, construimos
            if (seHaConstruidoAlgo) {
                int etapaRecienCompletada = _gameManager.State.LocalPlayer.EtapaConstruccion;
                ActualizarImagenesMaravilla(etapaRecienCompletada);
                DisplayAlert("Nueva etapa!", $"Has completado la parte {etapaRecienCompletada} de Guiza", "Ok");
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
