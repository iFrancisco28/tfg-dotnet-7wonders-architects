using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFG_FranciscoCarreroCarrero_7WondersArchitects.Domain.Entities
{
    public sealed class Relic {
        public int Id { get; }
        public string Name { get; }

        public int Part1BuiltPoints { get; private set; }
        public bool Part1IsBuilt { get; private set; }

        public int Part2BuiltPoints { get; private set; }
        public bool Part2IsBuilt { get; private set; }

        public int Part3BuiltPoints { get; private set; }
        public bool Part3IsBuilt { get; private set; }

        public int Part4BuiltPoints { get; private set; }
        public bool Part4IsBuilt { get; private set; }

        public int Part5BuiltPoints { get; private set; }
        public bool Part5IsBuilt { get; private set; }

        public Relic(
            int id,
            string name,
            int part1Points,
            int part2Points,
            int part3Points,
            int part4Points,
            int part5Points) {
            Id = id;
            Name = name;
            Part1BuiltPoints = part1Points;
            Part2BuiltPoints = part2Points;
            Part3BuiltPoints = part3Points;
            Part4BuiltPoints = part4Points;
            Part5BuiltPoints = part5Points;

            Part1IsBuilt = false;
            Part2IsBuilt = false;
            Part3IsBuilt = false;
            Part4IsBuilt = false;
            Part5IsBuilt = false;
        }

        public void BuildPart(int partNumber) {
            switch (partNumber) {
                case 1: Part1IsBuilt = true; break;
                case 2: Part2IsBuilt = true; break;
                case 3: Part3IsBuilt = true; break;
                case 4: Part4IsBuilt = true; break;
                case 5: Part5IsBuilt = true; break;
                default: throw new ArgumentException("Invalid relic part number.");
            }
        }

        public int TotalPoints() {
            int total = 0;
            if (Part1IsBuilt) total += Part1BuiltPoints;
            if (Part2IsBuilt) total += Part2BuiltPoints;
            if (Part3IsBuilt) total += Part3BuiltPoints;
            if (Part4IsBuilt) total += Part4BuiltPoints;
            if (Part5IsBuilt) total += Part5BuiltPoints;
            return total;
        }

        public bool IsComplete() {
            return Part1IsBuilt
                && Part2IsBuilt
                && Part3IsBuilt
                && Part4IsBuilt
                && Part5IsBuilt;
        }
    }

}
