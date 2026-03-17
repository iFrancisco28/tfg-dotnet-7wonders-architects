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

        public string Name { get; }
        public Wonder PlayerWonder { get; } 

        public Player(string name, Wonder wonder) {
            Name = name;
            PlayerWonder = wonder;
        }

    }
}
