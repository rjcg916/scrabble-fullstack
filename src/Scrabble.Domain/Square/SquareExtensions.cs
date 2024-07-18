using System.Collections.Generic;
using System.Linq;

namespace Scrabble.Domain
{
    public static class SquareExtensions
    {
        static public List<char> ToCharList(this List<Square> slice) =>
            slice.Select(square => square.Tile?.Letter ?? ' ').ToList();

        static public int ScoreRun(this List<Square> squares)
        {
            int wordScore = 0;

            int wordMultiplier = 1;

            foreach (var location in squares)
            {
                wordScore += (location.Tile.Value * location.LetterMultiplier);
                wordMultiplier *= location.WordMultiplier;
            }

            return wordScore * wordMultiplier;
        }
    }

}
