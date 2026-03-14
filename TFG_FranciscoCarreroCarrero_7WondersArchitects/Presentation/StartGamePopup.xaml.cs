using CommunityToolkit.Maui.Views;
using TFG_FranciscoCarreroCarrero_7WondersArchitects.Domain.Entities;
using System;

namespace TFG_FranciscoCarreroCarrero_7WondersArchitects.Presentation;

public partial class StartGamePopup : Popup {
	public StartGamePopup()
	{
		InitializeComponent();
    }

    //a hacer con signalR

    private async void HostGame(object sender, EventArgs e) {
        await Application.Current!.Windows[0].Page!.DisplayAlert("Aviso", "Tu codigo de sala es 1234", "Ok");
        await Shell.Current.GoToAsync("GameBoardPage");
    }

    private async void JoinGame(object sender, EventArgs e) {

         
        string resultado = await Application.Current!.Windows[0].Page!.DisplayPromptAsync("Unirse a una partida", null, "Aceptar", "Cancelar", "Codigo de sala");

        if (resultado != null) {
            await Application.Current!.Windows[0].Page!.DisplayAlert("Éxito", $"Ingresaste: {resultado}", "OK");
            await Shell.Current.GoToAsync("GameBoardPage");
        }


    }


}