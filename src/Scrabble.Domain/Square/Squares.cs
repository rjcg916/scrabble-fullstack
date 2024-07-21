using System;
using System.Collections.Generic;
using System.Linq;
using Scrabble.Util;

namespace Scrabble.Domain
{
    public static class Squares
    {
        static public List<char> ToCharList(this List<Square> slice) =>
            slice.Select(square => square.Tile?.Letter ?? ' ').ToList();


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


    }
}
