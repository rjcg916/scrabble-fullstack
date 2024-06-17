using System.Collections.Generic;
using System.Linq;
using System;

namespace Scrabble.Domain
{
    public partial class Board
    {

        public readonly Square[,] squares = new Square[Coord.RowCount, Coord.ColCount];
        public static readonly Coord STAR = new(R._8, C.H);

        public int MovesMade = 0;

        internal readonly Func<string, bool> IsWordValid;

        public Tile GetTile(Coord loc) => squares[loc.RVal, loc.CVal]?.Tile;

        public Board(Func<string, bool> IsWordValid)
        {
            this.IsWordValid = IsWordValid;

            foreach (var r in Coord.Rows) 
                foreach (var c in Coord.Cols) 
                    squares[(int)r, (int)c] = new Square();

            Initialize();
        }

        // create a new Board with initial move
        public Board(Func<string, bool> IsWordValid,
                     List<TilePlacement> tileList) :
            this(IsWordValid)
        {
            MovesMade++;
            PlaceTiles(tileList);
        }

        // create a new Board with initial move
        public Board(Func<string, bool> IsWordValid,
                        Coord startFrom,
                        List<Tile> tiles,
                        Placement placement) :
            this(IsWordValid)
        {
            MovesMade++;
            switch (placement)
            {
                case Placement.Star:
                case Placement.Horizontal:
                    PlaceTiles(tiles.Select((tile, index) =>
                        new TilePlacement(new Coord(startFrom.Row, (C)(startFrom.CVal + index)), tile)
                        ).ToList());
                    break;

                case Placement.Vertical:
                    PlaceTiles(tiles.Select((tile, index) =>
                        new TilePlacement(new Coord((R)(startFrom.RVal + index), startFrom.Col), tile)
                        ).ToList());
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(placement), placement, null);
            }
        }

 
        public Board(Board other)
        {
            foreach (var r in Coord.Rows)
                foreach (var c in Coord.Cols)
                    squares[(int)r, (int)c] = other.squares[(int)r, (int)c].Copy();
                            
            IsWordValid = other.IsWordValid;
            MovesMade = other.MovesMade;
        }

        public Square SquareByRow(int row, int col) =>
            squares[row, col];

        public Square SquareByColumn(int col, int row) =>
            squares[row, col];

        internal static List<Square> GetSquares(Func<int, int, Square> GetSquare, int sliceLocation, List<int> locationList, int maxIndex = Coord.RowCount - 1)
        {
            var (start, end) = GetEndpoints(GetSquare, sliceLocation, locationList, maxIndex);
            return GetSquares(GetSquare, sliceLocation, (start, end));
        }

        internal static List<Square> GetSquares(Func<int, int, Square> GetSquare, int sliceLocation, (int Start, int End) range )
        {
            List<Square> slice = [];

            for (int location = range.Start; location <= range.End; location++)
            {
                var sq = GetSquare(location, sliceLocation);
                if (sq.IsOccupied)
                    slice.Add(sq.Copy());
            }

            return slice;
        }

        // determine start and end location of occupied squares contiguous with specified squares
        internal static (int start, int end) GetEndpoints(Func<int, int, Square> GetSquare, int sliceLocation, List<int> locationList, int maxIndex = Coord.RowCount - 1)
        {
   
            var minMove = locationList.Min();
            var minOccupied = minMove;
 
            for (int pos = minMove - 1; pos >= 0; pos--)
            {
                if (!( GetSquare(pos, sliceLocation).IsOccupied )) 
                {
                    break;
                }
                minOccupied--;
            }

            var maxMove = locationList.Max();
            var maxOccupied = maxMove;

            for (int pos = maxMove + 1; pos <= maxIndex; pos++) 
            {
                if (!(GetSquare(pos, sliceLocation).IsOccupied))
                {
                    break;
                }
                maxOccupied++;
            }

            return (minOccupied, maxOccupied);
        }

        
        public List<LocationSquare> GetLocationSquares(bool IsOccupied = false)
        {
            List<LocationSquare> locationSquareList = [];

            foreach (var r in Coord.Rows)
                foreach (var c in Coord.Cols)
                    if (IsOccupied ? squares[(int)r, (int)c].IsOccupied : !squares[(int)r, (int)c].IsOccupied)
                        locationSquareList.Add(new LocationSquare(new Coord(r, c), squares[(int)r, (int)c]));

            return locationSquareList;
        }

        private void Initialize()
        {
            // start
            squares[(int)R._8, (int)C.H].SquareType = SquareType.start;

            // triple letters
            SetSquareTypes(SquareType.tl,

              [
                new(R._2, C.F), new(R._2, C.J),

                new(R._6, C.B), new(R._6, C.F), new(R._6, C.J), new(R._6, C.N),

                new(R._10, C.B), new(R._10, C.F), new(R._10, C.J), new(R._10, C.N),

                new(R._14, C.F), new(R._14, C.J)
              ]);

            // double letters
            SetSquareTypes(SquareType.dl,
                [
              new(R._1, C.D), new(R._1, C.L),

              new(R._3, C.G), new(R._3, C.I),

              new(R._4, C.A), new(R._4, C.H), new(R._4, C.O),

              new(R._7, C.C), new(R._7, C.G), new(R._7, C.I), new(R._7, C.M),

              new(R._8, C.D), new(R._8, C.L),

              new(R._9, C.C), new(R._9, C.G), new(R._9, C.I), new(R._9, C.M),

              new(R._12, C.A), new(R._12, C.H), new(R._12, C.O),

              new(R._13, C.G), new(R._13, C.I),

              new(R._15, C.D), new(R._15, C.L)

            ]);

            // double word
            SetSquareTypes(SquareType.dw,

              [
                new(R._2, C.B), new(R._2, C.N),

                new(R._3, C.C), new(R._3, C.M),

                new(R._4, C.D), new(R._4, C.L),

                new(R._5, C.E), new(R._5, C.K),

                new(R._11, C.E), new(R._11, C.K),

                new(R._12, C.D), new(R._12, C.L),

                new(R._13, C.C), new(R._13, C.M),

                new(R._14, C.B), new(R._14, C.N)

              ]);


            // triple word
            SetSquareTypes(
              SquareType.tw,

              [
                new(R._1, C.A), new(R._1, C.H), new(R._1, C.O),

                new(R._8, C.A), new(R._8, C.O),

                new(R._15, C.A), new(R._15, C.H), new(R._15, C.O)
              ]
            );
        }
        private void SetSquareTypes(SquareType t, Coord[] locs)
        {
            foreach (Coord loc in locs)
            {
                squares[(int)loc.Row, (int)loc.Col] = new Square
                {
                    SquareType = t
                };
            }
        }
    }
}