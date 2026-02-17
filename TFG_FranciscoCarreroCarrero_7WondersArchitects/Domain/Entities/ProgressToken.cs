using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public Type TokenType { get; }

        public ProgressToken(int id, Type tokenType) {
            Id = id;
            TokenType = tokenType;
        }
    }


}
