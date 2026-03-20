using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace TFG_FranciscoCarreroCarrero_7WondersArchitects.Domain.Entities {
    
    public sealed class Player {

        [JsonInclude] public string Name { get; private set; }
        [JsonInclude] public Wonder PlayerWonder { get; private set; }
        [JsonInclude] public List<Card> WonderDeck { get; set; } = new List<Card>();//mazoMaravilla
        [JsonInclude] public List<Card> HandDeck { get; set; } = new List<Card>();//mazoMano
        [JsonInclude] public int EtapaConstruccion { get; set; } = 0;
        [JsonInclude] public int FichasVictoriaMilitar { get; set; } = 0;
        [JsonInclude] public List<ProgressToken> FichasProgreso { get; set; } = new List<ProgressToken>();

        //constructor vacio para json
        [JsonConstructor]
        private Player() { }

        public Player(string name, Wonder.WonderType wonderType) {
            Name = name;
            PlayerWonder = new Wonder(wonderType);
        }

        public int GetPuntosMaravilla() {
            return PlayerWonder.TotalPoints(EtapaConstruccion);
        }
    }
}
