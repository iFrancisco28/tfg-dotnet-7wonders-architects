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

        public string Id { get; private set; }
        public CardType Type { get; private set; }
        public int Horns { get; private set; }
        public ScienceType? Science { get; private set; }
        public ResourceType? Resource { get; private set; }
        public int VictoryPoints { get; private set; }
        public bool HasCat { get; private set; }

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
            switch (Type) {
                case CardType.Resource:
                    return $"Type: {Type}, Resource: {Resource}";

                case CardType.Science:
                    return $"Type: {Type}, Science: {Science}";

                case CardType.Military:
                    return $"Type: {Type}, Horns: {Horns}";

                case CardType.VictoryPoint:
                    return $"Type: {Type}, VictoryPoints: {VictoryPoints}, HasCat: {HasCat}";

                default:
                    return $"Type: {Type}";
            }
        }

    }
}
