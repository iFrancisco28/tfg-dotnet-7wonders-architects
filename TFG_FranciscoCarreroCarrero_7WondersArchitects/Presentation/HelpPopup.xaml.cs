using CommunityToolkit.Maui.Views;
using Microsoft.Maui.Controls;
using TFG_FranciscoCarreroCarrero_7WondersArchitects.Domain.Entities;

namespace TFG_FranciscoCarreroCarrero_7WondersArchitects.Presentation;

public partial class HelpPopup : Popup {
	public HelpPopup()
	{
		InitializeComponent();
    }

    private async void GoToRules(object sender, EventArgs e) {
        try {
            Uri url = new Uri("https://cdn.svc.asmodee.net/production-asmodeees/uploads/2023/06/Reglas_7W_Architects.pdf");

            await Browser.Default.OpenAsync(url, BrowserLaunchMode.SystemPreferred);
        } catch (Exception) {
            await Application.Current.MainPage.DisplayAlert("Aviso", "No se pudo abrir la pagina web", "Ok");
        }
    }

    private async void GoToVideo(object sender, EventArgs e) {
        try {
            Uri url = new Uri("https://www.youtube.com/watch?v=GgpGIjzdVIw");

            await Browser.Default.OpenAsync(url, BrowserLaunchMode.SystemPreferred);
        } catch (Exception) {
            await Application.Current.MainPage.DisplayAlert("Aviso", "No se pudo abrir la pagina web", "Ok");
        }
    }

}