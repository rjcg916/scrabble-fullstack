using System;

namespace Scrabble.Domain.Model
{
    class Letter(string name, short value)
    {
        public String Name { get; set; } = name;
        public short Value { get; set; } = value;
    }
}
