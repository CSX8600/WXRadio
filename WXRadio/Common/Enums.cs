using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WXRadio.Attributes;

namespace Common
{
    public static class Enums
    {
        public enum Directions
        {
            [EnumCode("NW", "Northwest")]
            Northwest,
            [EnumCode("N", "North")]
            North,
            [EnumCode("NE", "Northeast")]
            Northeast,
            [EnumCode("W", "West")]
            West,
            [EnumCode("Here", "Central")]
            Here,
            [EnumCode("E", "East")]
            East,
            [EnumCode("SW", "Southwest")]
            Southwest,
            [EnumCode("S", "South")]
            South,
            [EnumCode("SE", "Southeast")]
            Southeast
        }
    }
}
