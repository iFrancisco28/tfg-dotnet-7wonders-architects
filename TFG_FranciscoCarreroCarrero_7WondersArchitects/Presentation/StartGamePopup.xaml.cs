using CommunityToolkit.Maui.Views;
using TFG_FranciscoCarreroCarrero_7WondersArchitects.Services;

namespace TFG_FranciscoCarreroCarrero_7WondersArchitects.Presentation;

public partial class StartGamePopup : Popup {
    private readonly SignalRService _signalRService;
    private readonly string _playerName;
    private readonly string _playerWonder;
    public StartGamePopup(SignalRService signalRService, string playerName, string playerWonder)
	{
		InitializeComponent();
        _signalRService = signalRService;
        _playerName = playerName;
        _playerWonder = playerWonder;
    }


    private async void HostGame(object sender, EventArgs e) {
        try {
            await _signalRService.ConnectAsync();

            // Creamos la sala
            string roomCode = await _signalRService.CreateRoomAsync(_playerName, _playerWonder);


            await Shell.Current.DisplayAlert("¡Sala Creada!", $"Tu código de sala es: {roomCode}\n\n¡Pásaselo a tus amigos para que se unan!", "Ok");

            //hacemos esto para pasar argumentos sin romper la Inyeccion de MauiProgram (tiene el signalRService como singleton)
            var servicios = this.Handler!.MauiContext!.Services;
            var gameBoard = ActivatorUtilities.CreateInstance<GameBoardPage>(servicios, _playerName, _playerWonder);
            await Shell.Current.Navigation.PushAsync(gameBoard);

            await this.CloseAsync();

        } catch (Exception ex) {
            await Shell.Current.DisplayAlert("Error", "No se pudo conectar: " + ex.Message, "Ok");
        }
    }

    private async void JoinGame(object sender, EventArgs e) {
        string roomCode = await Shell.Current.DisplayPromptAsync(
            "Unirse a una partida",
            "Introduce el código de la sala:",
            "Aceptar", "Cancelar", "Ej: A7X2");

        if (!string.IsNullOrWhiteSpace(roomCode)) {
            try {
                await _signalRService.ConnectAsync();

                //nos intentamos unir a la sala
                string resultado = await _signalRService.JoinRoomAsync(roomCode, _playerName, _playerWonder);

                //la respuesta del servidor dice si la sala existe o no
                if (resultado == "OK") {
                    //hacemos esto para pasar argumentos sin romper la Inyeccion de MauiProgram (tiene el signalRService como singleton)
                    var servicios = this.Handler!.MauiContext!.Services;
                    var gameBoard = ActivatorUtilities.CreateInstance<GameBoardPage>(servicios, _playerName, _playerWonder);
                    await Shell.Current.Navigation.PushAsync(gameBoard);
                } else if (resultado == "WONDER_TAKEN") {
                    await Shell.Current.DisplayAlert("Maravilla ocupada", "El anfitrion ya ha escogido esa maravilla. Por favor escoge otra.", "Ok");
                } else { 
                    //no hacemos el GoToAsync
                    await Shell.Current.DisplayAlert("Error", "Esa sala no existe o el código es incorrecto. Vuelve a intentarlo.", "Ok");
                }

            } catch (Exception ex) {
                await Shell.Current.DisplayAlert("Error", "No se pudo entrar a la sala: " + ex.Message, "Ok");
            }
        }
    }

}