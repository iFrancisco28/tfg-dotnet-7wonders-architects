using System.Text.Json.Serialization;

namespace TFG_FranciscoCarreroCarrero_7WondersArchitects.Domain.Entities {
    public sealed class Wonder {
        public enum WonderType {
            Alejandria,
            Babilonia,
            Efeso,
            Guiza,
            Halicarnaso,
            Olimpia,
            Rodas
        }

        [JsonInclude] public WonderType Type { get; private set; }

        [JsonInclude] public int[] PointsPerStage { get; private set; }

        [JsonConstructor]
        private Wonder() { }

        public Wonder(WonderType type) {
            Type = type;

            PointsPerStage = type switch {
                WonderType.Guiza => new[] { 4, 5, 6, 7, 8 },
                WonderType.Alejandria => new[] { 4, 3, 6, 5, 7 },
                WonderType.Babilonia => new[] { 3, 0, 5, 5, 7 },
                WonderType.Efeso => new[] { 3, 5, 4, 3, 7 },
                WonderType.Halicarnaso => new[] { 3, 3, 6, 5, 7 },
                WonderType.Olimpia => new[] { 3, 2, 5, 5, 7 },
                WonderType.Rodas => new[] { 4, 4, 5, 6, 7 }
            };
        }

        //los PV que tiene el jugador por {maravilla} en su {etapa}
        public int TotalPoints(int currentStage) {
            int total = 0;
            for (int i = 0; i < currentStage; i++) {
                total += PointsPerStage[i];
            }
            return total;
        }
    }
}