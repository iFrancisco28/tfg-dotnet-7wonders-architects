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
        var popup = new StartGamePopup(_signalRService);
        this.ShowPopup(popup);
    }

    private void Show_Help(object sender, EventArgs e) {
        var popup = new HelpPopup();
        this.ShowPopup(popup);
    }
    
}