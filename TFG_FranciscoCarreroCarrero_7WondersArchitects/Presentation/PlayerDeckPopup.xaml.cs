using CommunityToolkit.Maui.Views;
using TFG_FranciscoCarreroCarrero_7WondersArchitects.Domain.Entities;

namespace TFG_FranciscoCarreroCarrero_7WondersArchitects.Presentation;

public partial class PlayerDeckPopup : Popup {
    private readonly Player _localPlayer;
    private readonly Player _rivalPlayer;
    public PlayerDeckPopup(Player local, Player rival,bool isGameOver) {
		InitializeComponent();

        _localPlayer = local;
        _rivalPlayer = rival;

        //esto espera a que se dibuje todo porque si no se dibuja mal el picker del popup
        Dispatcher.Dispatch(() =>
        {
            OpcionesPicker.SelectedIndex = isGameOver ? 4 : 0;
        });
        ActualizarMazoMano(_localPlayer);
    }

    private void OpcionesPicker_SelectedIndexChanged(object sender, EventArgs e) {
        // 1. Ocultamos todos los grids por seguridad
        GridResourcesCards.IsVisible = false;
        GridScienceCards.IsVisible = false;
        GridVPCards.IsVisible = false;
        GridWarCards.IsVisible = false;
        GridGeneralInventory.IsVisible = false;
        
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
            case 4:
                GridGeneralInventory.IsVisible = true;
                break;
        }
    }

    private void SwitchRival_Toggled(object sender, ToggledEventArgs e) {
        var jugador = e.Value ? _rivalPlayer : _localPlayer;

        if (jugador == null) {
            return;
        }

        ActualizarMazoMano(jugador);
    }

    private void ActualizarMazoMano(Player jugador) {
        
        List<Card> mazoMano = jugador.HandDeck;
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

        //inventario general
        LblPlayerName.Text = $"Jugador: {jugador.Name}";
        LblWonderName.Text = $"Nombre: {jugador.PlayerWonder.Type}";
        LblConstructionStage.Text = $"Etapa: {jugador.EtapaConstruccion}/5";
        LblVictoryPoints.Text = $"Puntos de Victoria: {jugador.GetPuntosVictoriaMaravilla()+jugador.GetPuntosVictoriaCartas()}";
        LblMilitaryPoints.Text = $"Puntos de Victoria Mitar: {jugador.FichasVictoriaMilitar}";
    }
}