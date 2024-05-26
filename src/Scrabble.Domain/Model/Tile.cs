using System;

namespace Scrabble.Domain.Model
{
    public class Tile(String letter, int Value = 1)
    {
        public string Letter { get; } = letter.ToUpper();
        public int Value { get; } = Value;
    }
}




