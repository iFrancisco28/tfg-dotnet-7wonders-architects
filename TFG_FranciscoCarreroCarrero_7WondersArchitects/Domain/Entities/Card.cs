using System.Text.Json.Serialization;

namespace TFG_FranciscoCarreroCarrero_7WondersArchitects.Domain.Entities
{
    public class Card {
        public enum CardType {
            Resource,
            Military,
            Science,
            VictoryPoint
        }

        public enum ResourceType {
            Wood,
            Stone,
            Clay,
            Papyrus,
            Glass,
            Gold
        }

        public enum ScienceType {
            Compass,
            Tablet,
            Gear
        }

        [JsonInclude] public string Id { get; private set; }
        [JsonInclude] public CardType Type { get; private set; }
        [JsonInclude] public int Horns { get; private set; }
        [JsonInclude] public ScienceType? Science { get; private set; }
        [JsonInclude] public ResourceType? Resource { get; private set; }
        [JsonInclude] public int VictoryPoints { get; private set; }
        [JsonInclude] public bool HasCat { get; private set; }

        //constructor vacio para json
        [JsonConstructor]
        private Card() { }

        //constructor carta Recurso
        public Card(string id, ResourceType resourceType) {
            Id = id;
            Type = CardType.Resource; // Se asigna automáticamente
            Resource = resourceType;
        }

        //constructor carta Ciencia
        public Card(string id, ScienceType scienceType) {
            Id = id;
            Type = CardType.Science; // Se asigna automáticamente
            Science = scienceType;
        }

        //constructor carta Puntos Victoria
        public Card(string id, bool hasCat) {
            Id = id;
            Type = CardType.VictoryPoint; // Se asigna automáticamente
            if (hasCat) {
                VictoryPoints = 2;
            } else {
                VictoryPoints = 3;
            }
            HasCat = hasCat;
        }

        //constructor carta Militar
        public Card(string id, int horns) {
            Id = id;
            Type = CardType.Military; // Se asigna automáticamente
            Horns = horns;
        }


        public static ResourceType[] GetAllResourceTypes() {
            return Enum.GetValues<ResourceType>();
        }
        public static ScienceType[] GetAllScienceTypes() {
            return Enum.GetValues<ScienceType>();
        }

        public override string ToString() {
            string tipoEsp = Type switch {
                CardType.Resource => "Recurso",
                CardType.Science => "Ciencia",
                CardType.Military => "Militar",
                CardType.VictoryPoint => "Puntos de Victoria"
            };

            switch (Type) {
                case CardType.Resource:
                    string recursoEs = Resource switch {
                        ResourceType.Wood => "Madera",
                        ResourceType.Stone => "Piedra",
                        ResourceType.Clay => "Arcilla",
                        ResourceType.Papyrus => "Papiro",
                        ResourceType.Glass => "Botella", 
                        ResourceType.Gold => "Oro"
                    };

                    return $"Tipo: {tipoEsp}, {recursoEs}";

                case CardType.Science:
                    string cienciaEs = Science switch {
                        ScienceType.Compass => "Compás",
                        ScienceType.Tablet => "Tablilla",
                        ScienceType.Gear => "Engranaje"
                    };

                    return $"Tipo: {tipoEsp}, {cienciaEs}";

                case CardType.Military:
                    return $"Tipo: {tipoEsp}, con {Horns} cuernos";

                case CardType.VictoryPoint:
                    return $"Tipo: {tipoEsp}, otorga {VictoryPoints} Puntos de Victoria{(HasCat ? " e incluye al gato" : "")}";

                default:
                    return $"Tipo: {tipoEsp}";
            }
        }

    }
}
