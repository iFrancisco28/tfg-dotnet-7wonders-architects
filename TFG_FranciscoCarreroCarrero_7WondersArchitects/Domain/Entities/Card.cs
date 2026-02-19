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
            Bottle,
            Gold
        }

        public enum ScienceType {
            Compass,
            Tablet,
            Gear
        }

        public string Id;
        public CardType Type;

        // Militar
        public int Horns;

        // Ciencia
        public ScienceType? Science;

        // Recurso
        public ResourceType? Resource;

        // Puntos de victoria
        public int VictoryPoints;
        public bool HasCat;

        //constructor carta Recurso
        public Card(string id, CardType Cardtype, ResourceType resourceType) {
            Id = id;
            Type = Cardtype;
            Resource = resourceType;
        }

        //constructor carta Ciencia
        public Card(string id, CardType Cardtype, ScienceType scienceType) {
            Id = id;
            Type = Cardtype;
            Science = scienceType;
        }

        //constructor carta Puntos Victoria
        public Card(string id, CardType Cardtype, int victoryPoints, bool hasCat) {
            Id = id;
            Type = Cardtype;
            VictoryPoints = victoryPoints;
            HasCat = hasCat;
        }

        //constructor carta Militar
        public Card(string id, CardType Cardtype, int horns) {
            Id = id;
            Type = Cardtype;
            Horns = horns;
        }


        public static ResourceType[] GetAllResourceTypes() {
            return Enum.GetValues<ResourceType>();
        }
        public static ScienceType[] GetAllScienceTypes() {
            return Enum.GetValues<ScienceType>();
        }

        public override string ToString() {
            // Puedes personalizar esto como más te guste para leerlo rápido en la consola
            return $"Card(Id: {Id}, Type: {Type}, Resource: {Resource}, Science: {Science}, VictoryPoints: {VictoryPoints}, HasCat: {HasCat}, Horns: {Horns})";
        }

    }
}
