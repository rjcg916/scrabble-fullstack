using System;
using System.Collections.Generic;
using System.Linq;

namespace Scrabble.Domain
{
    public static class TileExtensions
    {
        public static List<Tile> LettersToTiles(this string letters) =>
            letters == null ?
                throw new ArgumentNullException(nameof(letters)) :
                letters.Select(letter => new Tile(letter)).ToList();

        public static string TilesToLetters(this List<Tile> tiles) =>
            string.Join(", ", tiles.Select(t => t.Letter));
    }

    public class Tile
    {
        private char _letter;

        public Tile(char letter)
        {
            _letter = Char.ToUpper(letter);
            Value = LetterValues.TryGetValue(_letter, out int value) 
                    ? value : throw new ArgumentException("Invalid character");
        }

        public int Value { get; }

        public char Letter
        {
            get => _letter;
            private set
            {
                if (Value == 0)
                {
                    _letter = Char.ToUpper(value);
                }
                else
                {
                    throw new InvalidOperationException("Only tiles with value 0 can be changed.");
                }
            }
        }

        private static readonly Dictionary<char, int> LetterValues = new()
        {
            { 'A', 1 }, { 'B', 3 }, { 'C', 3 }, { 'D', 2 }, { 'E', 1 },
            { 'F', 4 }, { 'G', 2 }, { 'H', 4 }, { 'I', 1 }, { 'J', 8 },
            { 'K', 5 }, { 'L', 1 }, { 'M', 3 }, { 'N', 1 }, { 'O', 1 },
            { 'P', 3 }, { 'Q', 10 }, { 'R', 1 }, { 'S', 1 }, { 'T', 1 },
            { 'U', 1 }, { 'V', 4 }, { 'W', 4 }, { 'X', 8 }, { 'Y', 4 },
            { 'Z', 10 }, { '?', 0 }
        };

        public void ChangeLetter(char newLetter)
        {
            Letter = newLetter;
        }

        public bool Equals(Tile other)
        {
            if (other == null)
                return false;

            return _letter == other._letter && Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Tile);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_letter, Value);
        }

        public static bool operator ==(Tile left, Tile right)
        {
            if (left is null)
                return right is null;

            return left.Equals(right);
        }

        public static bool operator !=(Tile left, Tile right)
        {
            return !(left == right);
        }
    }
}
