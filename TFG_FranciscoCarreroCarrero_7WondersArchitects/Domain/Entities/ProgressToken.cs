using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TFG_FranciscoCarreroCarrero_7WondersArchitects.Domain.Entities
{
    public sealed class ProgressToken {
        public enum Type {
            Urbanism,
            Crafts,
            Jewellery,
            Science,
            Propaganda,
            Architecture,
            Economy,
            Engineering,
            Tactics,
            Decor,
            Politics,
            Strategy,
            Education,
            Culture
        }

        public int Id { get; }
        [JsonInclude] public Type TokenType { get; private set; }
        [JsonInclude] public string Description { get; private set; }


        [JsonConstructor]
        private ProgressToken() { }

        public ProgressToken(int id, Type tokenType, string description) {
            Id = id;
            TokenType = tokenType;
            Description = description;
        }

        public override string ToString() {
            return $"{TokenType}: {Description}";
        }
    }
}
