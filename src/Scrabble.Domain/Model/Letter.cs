using System;

namespace Scrabble.Domain.Model
{
    class Letter(char name, short value)
    {
        public char Name { get; set; } = name;
        public short Value { get; set; } = value;
    }
}
