using CommunityToolkit.Maui.Extensions;
using TFG_FranciscoCarreroCarrero_7WondersArchitects.Services;

namespace TFG_FranciscoCarreroCarrero_7WondersArchitects.Presentation;

public partial class LoginPage : ContentPage {
    private readonly SignalRService _signalRService;
    public LoginPage(SignalRService signalRService) {
		InitializeComponent();
        _signalRService = signalRService;
    }

	private void Redirect(object sender, EventArgs e) {
        if (string.IsNullOrEmpty(NameEntry.Text)) {
            DisplayAlert("Error", "Por favor, introduzca un nombre.", "Ok");
            return;
        }
        if (WonderPicker.SelectedIndex == -1) {
            DisplayAlert("Error", "Por favor, seleccione una maravilla.", "Ok");
            return;
        }

        var popup = new StartGamePopup(_signalRService);
        this.ShowPopup(popup);
    }

    private void Show_Help(object sender, EventArgs e) {
        var popup = new HelpPopup();
        this.ShowPopup(popup);
    }
    
}