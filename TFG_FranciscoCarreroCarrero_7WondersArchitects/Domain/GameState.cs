using TFG_FranciscoCarreroCarrero_7WondersArchitects.Domain.Entities;

namespace TFG_FranciscoCarreroCarrero_7WondersArchitects.Domain
{
    public class GameState {
        //comun
        public List<Card> MainDeck { get; set; } = new List<Card>();

        public int AvisosGuerraLevantadas { get; set; } = 0; //sin hacer

        //jugadores
        public Player LocalPlayer { get; set; }
        public Player RemotePlayer { get; set; } 

        //estado
        public bool IsLocalPlayerTurn { get; set; }
    }
}
