using System;
using System.Collections.Generic;

namespace Scrabble.Domain
{
    public partial class Board
    {
        /// <summary>
        /// "SquareBy" functions allow Higher Order Functions to operate in Horizontal or Vertical direction
        /// </summary>       
        public Square SquareByRow(int row, int col) =>
            squares[row, col];
        public Square SquareByColumn(int col, int row) =>
            squares[row, col];

        public Tile GetTile(Coord loc) => squares[loc.RVal, loc.CVal]?.Tile;


  
        /// <summary>
        ///  Fetch all squares in the range of a slice
        /// </summary>
        internal static List<Square> GetSquares(
                                        Func<int, int, Square> GetSquare,
                                        int sliceLocation,
                                        (int Start, int End) range)
        {
            List<Square> slice = [];

            for (int location = range.Start; location <= range.End; location++)
            {
                var square = GetSquare(location, sliceLocation);
                if (square.IsOccupied)
                    slice.Add(new(square));
            }

            return slice;
        }

        public List<LocationSquare> GetOccupiedSquares() =>
            GetLocationSquares(IsOccupied: true);

        public List<LocationSquare> GetVacantSquares() =>
           GetLocationSquares(IsOccupied: false);

        /// <summary>
        /// get list of coord and squares (by default, empty squares)
        /// </summary>
        private List<LocationSquare> GetLocationSquares(bool IsOccupied = false)
        {
            List<LocationSquare> locationSquareList = [];

            foreach (var r in Coord.Rows)
                foreach (var c in Coord.Cols)
                    if (IsOccupied ? squares[(int)r, (int)c].IsOccupied : !squares[(int)r, (int)c].IsOccupied)
                        locationSquareList.Add(new LocationSquare(new Coord(r, c), squares[(int)r, (int)c]));

            return locationSquareList;
        }
    }
}
