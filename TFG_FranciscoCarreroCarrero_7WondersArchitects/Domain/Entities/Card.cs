using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFG_FranciscoCarreroCarrero_7WondersArchitects.Domain.Entities
{
    public class Card {
        public enum CardType {
            Resource,
            Military,
            Science,
            Civilian,
            Guild
        }

        public enum ResourceType {
            Wood,
            Stone,
            Clay,
            Papyrus,
            Glass
        }

        public enum ScienceSymbol {
            Compass,
            Tablet,
            Gear
        }

        public string Id;
        public string Name;
        public CardType Type;

        // Recursos
        public ResourceType? ResourceProduced;

        // Militar
        public int Shields;

        // Ciencia
        public ScienceSymbol? Science;

        // Civil
        public int VictoryPoints;

        // Gremios
        public string GuildEffectId;

        public Card(string id, string name, CardType type) {
            Id = id;
            Name = name;
            Type = type;
        }
    }
}
