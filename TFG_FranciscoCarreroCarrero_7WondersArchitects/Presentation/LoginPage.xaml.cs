using CommunityToolkit.Maui.Extensions;

namespace TFG_FranciscoCarreroCarrero_7WondersArchitects.Presentation;

public partial class LoginPage : ContentPage {
	public LoginPage() {
		InitializeComponent();
	}

	private async void Redirect(object sender, EventArgs e) {
        await Shell.Current.GoToAsync("GameBoardPage");
    }

    private void Show_Help(object sender, EventArgs e) {
        var popup = new HelpPopup();
        this.ShowPopup(popup);
    }
    
}