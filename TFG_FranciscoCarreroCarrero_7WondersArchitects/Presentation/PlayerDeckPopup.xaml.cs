using CommunityToolkit.Maui.Views;
using TFG_FranciscoCarreroCarrero_7WondersArchitects.Domain.Entities;

namespace TFG_FranciscoCarreroCarrero_7WondersArchitects.Presentation;

public partial class PlayerDeckPopup : Popup {
	public PlayerDeckPopup(IEnumerable<Card> mazoMano)
	{
		InitializeComponent();
        //esto espera a que se dibuje todo porque si no se dibuja mal el picker del popup
        Dispatcher.Dispatch(() =>
        {
            OpcionesPicker.SelectedIndex = 0;
        });
        ActualizarMazoMano(mazoMano);
    }

    private void OpcionesPicker_SelectedIndexChanged(object sender, EventArgs e) {
        // 1. Ocultamos todos los grids por seguridad
        GridResourcesCards.IsVisible = false;
        GridScienceCards.IsVisible = false;
        GridVPCards.IsVisible = false;
        GridWarCards.IsVisible = false;
        
        // 2. Mostramos solo el que toca según lo que haya elegido el usuario
        switch (OpcionesPicker.SelectedIndex) {
            case 0:
                GridResourcesCards.IsVisible = true;
                break;
            case 1:
                GridScienceCards.IsVisible = true;
                break;
            case 2:
                GridVPCards.IsVisible = true;
                break;
            case 3:
                GridWarCards.IsVisible = true;
                break;
        }
    }

    private void ActualizarMazoMano(IEnumerable<Card> mazoMano) {
        if (mazoMano == null || !mazoMano.Any())
            return;

        //cartas recurso
        LblClayCard.Text = $"Tienes {mazoMano.Count(c => c.Resource == Card.ResourceType.Clay)} cartas de arcilla";
        LblGlassCard.Text = $"Tienes {mazoMano.Count(c => c.Resource == Card.ResourceType.Glass)} cartas de cristal";
        LblWoodCard.Text = $"Tienes {mazoMano.Count(c => c.Resource == Card.ResourceType.Wood)} cartas de madera";
        LblPapyrusCard.Text = $"Tienes {mazoMano.Count(c => c.Resource == Card.ResourceType.Papyrus)} cartas de papiro";
        LblStoneCard.Text = $"Tienes {mazoMano.Count(c => c.Resource == Card.ResourceType.Stone)} cartas de piedra";
        LblGoldCard.Text = $"Tienes {mazoMano.Count(c => c.Resource == Card.ResourceType.Gold)} cartas de oro";


        //cartas ciencia
        LblCompassCard.Text = $"Tienes {mazoMano.Count(c => c.Science == Card.ScienceType.Compass)} cartas de compas";
        LblGearCard.Text = $"Tienes {mazoMano.Count(c => c.Science == Card.ScienceType.Gear)} cartas de engranaje";
        LblTabletCard.Text = $"Tienes {mazoMano.Count(c => c.Science == Card.ScienceType.Tablet)} cartas de tablilla";

        //cartas PV
        LblVP3Card.Text = $"Tienes {mazoMano.Count(c => c.VictoryPoints == 3)} cartas de 3 Puntos de Victoria";
        LblVP2Card.Text = $"Tienes {mazoMano.Count(c => c.VictoryPoints == 2)} cartas de 2 Puntos de Victoria";

        //cartas guerra
        Lbl0WarCard.Text = $"Tienes {mazoMano.Count(c => c.Horns == 0 && c.Type == Card.CardType.Military)} cartas de Guerra sin cuernos";
        Lbl1WarCard.Text = $"Tienes {mazoMano.Count(c => c.Horns == 1)} cartas de Guerra con 1 cuerno";
        Lbl2WarCard.Text = $"Tienes {mazoMano.Count(c => c.Horns == 2)} cartas de Guerra con 2 cuernos";
    }
}