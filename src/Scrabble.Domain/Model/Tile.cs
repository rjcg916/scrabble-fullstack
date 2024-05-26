using System;

namespace Scrabble.Domain.Model
{
    public class Tile(Char letter, int Value = 1)
    {
        public Char Letter { get; } = Char.ToUpper( letter);
        public int Value { get; } = Value;
    }
}