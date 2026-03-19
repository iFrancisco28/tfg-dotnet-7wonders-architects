using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace TFG_FranciscoCarreroCarrero_7WondersArchitects.Domain.Entities {
    
    public sealed class Player {
        public enum Wonder {
            Alejandria,
            Babilonia,
            Efeso,
            Guiza,
            Halicarnaso,
            Olimpia,
            Rodas
        }

        [JsonInclude] public string Name { get; }
        [JsonInclude] public Wonder PlayerWonder { get; }
        [JsonInclude] public List<Card> WonderDeck { get; set; } = new List<Card>();//mazoMaravilla
        [JsonInclude] public List<Card> HandDeck { get; set; } = new List<Card>();//mazoMano
        [JsonInclude] public int EtapaConstruccion { get; set; } = 0;

        //constructor vacio para json
        [JsonConstructor]
        private Player() { }

        public Player(string name, Wonder wonder) {
            Name = name;
            PlayerWonder = wonder;
        }
    }
}
