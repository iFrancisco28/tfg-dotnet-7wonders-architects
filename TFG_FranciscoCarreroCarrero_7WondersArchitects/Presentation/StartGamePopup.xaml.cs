using CommunityToolkit.Maui.Views;
using TFG_FranciscoCarreroCarrero_7WondersArchitects.Domain.Entities;

namespace TFG_FranciscoCarreroCarrero_7WondersArchitects.Presentation;

public partial class StartGamePopup : Popup {
	public StartGamePopup(IEnumerable<Card> mazoMano)
	{
		InitializeComponent();
    }
}