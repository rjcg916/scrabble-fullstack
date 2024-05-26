using System;

namespace Scrabble.Domain.Model
{
    public class Tile(Char letter, ushort Value = 1)
    {
        public Char Letter { get; } = Char.ToUpper(letter);
        public ushort Value { get; } = Value;
    }
}