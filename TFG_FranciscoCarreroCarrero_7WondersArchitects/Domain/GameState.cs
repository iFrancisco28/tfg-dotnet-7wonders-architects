using TFG_FranciscoCarreroCarrero_7WondersArchitects.Domain.Entities;

namespace TFG_FranciscoCarreroCarrero_7WondersArchitects.Domain
{
    public class GameState {
        //comun
        public List<Card> MainDeck { get; set; } = new List<Card>();
        public int CuernosGuerra { get; set; } = 0; // Para el futuro

        //jugadores
        public Player LocalPlayer { get; set; }
        public Player RemotePlayer { get; set; } // El rival de SignalR

        //estado
        public bool IsMyTurn { get; set; } = false;
    }
}
