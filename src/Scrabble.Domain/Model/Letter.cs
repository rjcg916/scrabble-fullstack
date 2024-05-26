using System;

namespace Scrabble.Domain.Model
{
    class Letter(string name, short value)
    {
        public String name { get; set; } = name;
        public short value { get; set; } = value;
    }
}
