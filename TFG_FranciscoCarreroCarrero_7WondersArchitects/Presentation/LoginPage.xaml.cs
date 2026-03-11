namespace TFG_FranciscoCarreroCarrero_7WondersArchitects.Presentation;

public partial class LoginPage : ContentPage {
	public LoginPage() {
		InitializeComponent();
	}

	private async void Redirect(object sender, EventArgs e) {
        await Shell.Current.GoToAsync("//MainPage");
    }
}