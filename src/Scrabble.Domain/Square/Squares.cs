using System;
using System.Collections.Generic;
using System.Linq;

namespace Scrabble.Domain
{
    public static class Squares
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

        /// <summary>
        /// fetch all squares in a range of a slice
        /// </summary>
        public static List<(int, Square)> GetLocationSquares(
                                        Func<int, int, Square> GetSquare,
                                        int sliceLocation,
                                        (int Start, int End) range)
        {
            List<(int, Square)> slice = [];

            for (int location = range.Start; location <= range.End; location++)
            {
                var sq = GetSquare(location, sliceLocation);
                if (sq.IsOccupied)
                    slice.Add((location, new(sq)));
            }
            return slice;
        }

        public static List<PlacementError> ValidateWordSlices(
                                    Func<int, List<Square>> getSquares,
                                    int sliceCount,
                                    bool isHorizontal,
                                    Func<string, bool> IsWordValid)
        {
            List<PlacementError> invalidMessages = [];

            for (int index = 0; index < sliceCount; index++)
            {
                var squareList = getSquares(index);
                var charList = squareList.ToCharList();

                if (charList != null)
                {
                    var words = charList.ToWords();
                    var (valid, invalidWord) = words.ValidateWordList(IsWordValid);

                    if (!valid)
                    {
                        var coord = isHorizontal
                                    ? new Coord((R)index, 0) 
                                    : new Coord(0, (C)index);

                        invalidMessages.Add(new(coord, invalidWord));
                    }
                }
            }

            return invalidMessages;
        }
    }

}
