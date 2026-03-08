using CommunityToolkit.Maui.Views;

namespace TFG_FranciscoCarreroCarrero_7WondersArchitects.Presentation;

public partial class PlayerDeckPopup : Popup {
	public PlayerDeckPopup()
	{
		InitializeComponent();
        //esto espera a que se dibuje todo porque si no se dibuja mal el picker del popup
        Dispatcher.Dispatch(() =>
        {
            OpcionesPicker.SelectedIndex = 0;
        });
    }

    private void OpcionesPicker_SelectedIndexChanged(object sender, EventArgs e) {
        // 1. Ocultamos todos los grids por seguridad
        GridCartasRecursos.IsVisible = false;
        GridCartasCiencia.IsVisible = false;
        GridCartasPV.IsVisible = false;
        GridCartasGuerra.IsVisible = false;
        
        // 2. Mostramos solo el que toca según lo que haya elegido el usuario
        switch (OpcionesPicker.SelectedIndex) {
            case 0:
                GridCartasRecursos.IsVisible = true;
                break;
            case 1:
                GridCartasCiencia.IsVisible = true;
                break;
            case 2:
                GridCartasPV.IsVisible = true;
                break;
            case 3:
                GridCartasGuerra.IsVisible = true;
                break;
        }
    }
}