using CommunityToolkit.Maui.Views;
using TFG_FranciscoCarreroCarrero_7WondersArchitects.Services;

namespace TFG_FranciscoCarreroCarrero_7WondersArchitects.Presentation;

public partial class StartGamePopup : Popup {
    private readonly SignalRService _signalRService;
    public StartGamePopup(SignalRService signalRService)
	{
		InitializeComponent();
        _signalRService = signalRService;
    }


    private async void HostGame(object sender, EventArgs e) {
        try {
            await _signalRService.ConnectAsync();

            // Creamos la sala
            string roomCode = await _signalRService.CreateRoomAsync();


            await Shell.Current.DisplayAlert("¡Sala Creada!", $"Tu código de sala es: {roomCode}\n\n¡Pásaselo a tus amigos para que se unan!", "Ok");
            await Shell.Current.GoToAsync("GameBoardPage");

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
                bool seHaUnido = await _signalRService.JoinRoomAsync(roomCode);

                //la respuesta del servidor dice si la sala existe o no
                if (seHaUnido) {

                    await Shell.Current.GoToAsync("GameBoardPage");
                } else {
                    //no hacemos el GoToAsync.
                    await Shell.Current.DisplayAlert("Error", "Esa sala no existe o el código es incorrecto. Vuelve a intentarlo.", "Ok");
                }

            } catch (Exception ex) {
                await Shell.Current.DisplayAlert("Error", "No se pudo entrar a la sala: " + ex.Message, "Ok");
            }
        }
    }

}