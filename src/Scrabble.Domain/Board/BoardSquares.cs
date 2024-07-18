using System.Collections.Generic;
using System;
using System.Linq;

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

        public static bool DoesMoveTouchSTAR(List<TilePlacement> tileList) =>
             tileList.Exists(t => (t.Coord.Col == Board.STAR.Col) && (t.Coord.Row == Board.STAR.Row));

        public bool IsOccupied(Coord coord) => squares[coord.RVal, coord.CVal].IsOccupied;
        public bool AreOccupied(List<Coord> locations) => locations.Any(l => IsOccupied(l));

        // fetch all squares in a range of a slice
        internal static List<Square> GetSquares(
                                        Func<int, int, Square> GetSquare,
                                        int sliceLocation,
                                        (int Start, int End) range)
        {
            List<Square> slice = [];

            for (int location = range.Start; location <= range.End; location++)
            {
                var sq = GetSquare(location, sliceLocation);
                if (sq.IsOccupied)
                    slice.Add(new(sq));
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

        /// <summary>
        /// fetch all squares in a range of a slice
        /// </summary>
        internal static List<(int, Square)> GetLocationSquares(
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
