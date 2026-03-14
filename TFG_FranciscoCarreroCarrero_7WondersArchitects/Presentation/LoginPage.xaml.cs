using CommunityToolkit.Maui.Extensions;

namespace TFG_FranciscoCarreroCarrero_7WondersArchitects.Presentation;

public partial class LoginPage : ContentPage {
	public LoginPage() {
		InitializeComponent();
	}

	private void Redirect(object sender, EventArgs e) {
        var popup = new StartGamePopup();
        this.ShowPopup(popup);
    }

    private void Show_Help(object sender, EventArgs e) {
        var popup = new HelpPopup();
        this.ShowPopup(popup);
    }
    
}