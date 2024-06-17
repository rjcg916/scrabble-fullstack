using System.Collections.Generic;
using System;

namespace Scrabble.Domain
{
    public class Tile(char character)
    {
        public char Letter = Char.ToUpper(character);

        private static readonly Dictionary<char, int> LetteRValues = new()
        {
            { 'A', 1 }, { 'B', 3 }, { 'C', 3 }, { 'D', 2 }, { 'E', 1 },
            { 'F', 4 }, { 'G', 2 }, { 'H', 4 }, { 'I', 1 }, { 'J', 8 },
            { 'K', 5 }, { 'L', 1 }, { 'M', 3 }, { 'N', 1 }, { 'O', 1 },
            { 'P', 3 }, { 'Q', 10 }, { 'R', 1 }, { 'S', 1 }, { 'T', 1 },
            { 'U', 1 }, { 'V', 4 }, { 'W', 4 }, { 'X', 8 }, { 'Y', 4 },
            { 'Z', 10 }, { ' ', 0 }
        };

        public int Value
        {
            get
            {
                if (LetteRValues.TryGetValue(character, out int value))
                {
                    return value;
                }
                throw new ArgumentException("Invalid character");
            }
        }

        // Override Equals method
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var other = (Tile)obj;

            // Compare Letter and Value properties
            return Letter == other.Letter && Value == other.Value;
        }

        // Override GetHashCode method
        public override int GetHashCode()
        {
            return HashCode.Combine(Letter, Value);
        }
    }
}