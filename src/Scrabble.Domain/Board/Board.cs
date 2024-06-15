using System.Collections.Generic;
using System.Linq;
using System;

namespace Scrabble.Domain
{
    public partial class Board
    {
        public static readonly int rowCount = R._15 - R._1 + 1;
        public static readonly int colCount = C.O - C.A + 1;

        public readonly Square[,] squares = new Square[rowCount, colCount];
        public static readonly Coord STAR = new(R._8, C.H);

        private const int LOWERBOUND = 0;
        private const int DIMENSION = 15;

        public int MovesMade = 0;

        internal readonly Func<string, bool> IsWordValid;

        public Tile GetTile(Coord loc) => squares[loc.RowValue, loc.ColValue]?.Tile;

        public Board(Func<string, bool> IsWordValid)
        {
            this.IsWordValid = IsWordValid;

            foreach (var r in Enumerable.Range(0, rowCount))
                foreach (var c in Enumerable.Range(0, colCount))
                    squares[r, c] = new Square();

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
                        new TilePlacement(new Coord(startFrom.Row, (C)(startFrom.ColValue + index)), tile)
                        ).ToList());
                    break;

                case Placement.Vertical:
                    PlaceTiles(tiles.Select((tile, index) =>
                        new TilePlacement(new Coord((R)(startFrom.RowValue + index), startFrom.Col), tile)
                        ).ToList());
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(placement), placement, null);
            }
        }

        // clone a Board        
        public Board(Board other)
        {
            for (int r = 0; r < rowCount; r++)
            {
                for (int c = 0; c < colCount; c++)
                {
                    squares[r, c] = other.squares[r, c].Copy();
                }
            }
            IsWordValid = other.IsWordValid;
            MovesMade = other.MovesMade;
        }

        //public Board Copy() => new(this);
        // retrieve contents of squares from a specified range

        internal List<Square> GetSquaresVertical(int sliceLocation, int rangeStart = LOWERBOUND, int rangeEnd = DIMENSION - 1)
        {
            List<Square> slice = [];


            for (int row = rangeStart; row <= rangeEnd; row++)
            {
                var sq = squares[row, sliceLocation];
                if (sq.IsOccupied)
                    slice.Add(sq.Copy());
            }


            return slice;
        }




        internal List<Square> GetSquaresHorizontal(int sliceLocation, int rangeStart = LOWERBOUND, int rangeEnd = DIMENSION - 1)
        {
            List<Square> slice = [];

            for (int col = rangeStart; col <= rangeEnd; col++)
            {
                var sq = squares[sliceLocation, col];
                if (sq.IsOccupied)
                    slice.Add(sq.Copy());
            }

            return slice;
        }

        //internal List<Square> GetSquares(bool isHorizontal, int sliceLocation, int rangeStart = LOWERBOUND, int rangeEnd = DIMENSION - 1)
        //{
        //    List<Square> slice = [];

        //    if (isHorizontal)
        //    {
        //        for (int col = rangeStart; col <= rangeEnd; col++)
        //        {
        //            var sq = squares[sliceLocation, col];
        //            if (sq.IsOccupied)
        //                slice.Add(sq.Copy());
        //        }
        //    }
        //    else
        //    {
        //        for (int row = rangeStart; row <= rangeEnd; row++)
        //        {
        //            var sq = squares[row, sliceLocation];
        //            if (sq.IsOccupied)
        //                slice.Add(sq.Copy());
        //        }
        //    }

        //    return slice;
        //}

        // determine start and end location of occupied squares contiguous with specified squares
        //internal (int start, int end) GetEndpoints(bool isHorizontal, int sliceLocation, List<int> locationList)
        //{
        //    var minMove = locationList.Min();
        //    var maxMove = locationList.Max();
        //    var minOccupied = minMove;
        //    var maxOccupied = maxMove;

        //    for (int pos = minMove - 1; pos >= 0; pos--)
        //    {
        //        if (!(isHorizontal ? squares[sliceLocation, pos].IsOccupied :
        //                           squares[pos, sliceLocation].IsOccupied))
        //        {
        //            break;
        //        }
        //        minOccupied--;
        //    }

        //    for (int pos = maxMove + 1; pos < (isHorizontal ? Board.colCount : Board.rowCount); pos++)
        //    {
        //        if (!(isHorizontal ? squares[sliceLocation, pos].IsOccupied :
        //                            squares[pos, sliceLocation].IsOccupied))
        //        {
        //            break;
        //        }
        //        maxOccupied++;
        //    }

        //    return (minOccupied, maxOccupied);
        //}

        // determine start and end location of occupied squares contiguous with specified squares
        internal (int start, int end) GetEndpointsHorizontal(int sliceLocation, List<int> locationList)
        {
            var minMove = locationList.Min();
            var maxMove = locationList.Max();
            var minOccupied = minMove;
            var maxOccupied = maxMove;

            for (int pos = minMove - 1; pos >= 0; pos--)
            {
                if (!(squares[sliceLocation, pos].IsOccupied ))
                {
                    break;
                }
                minOccupied--;
            }

            for (int pos = maxMove + 1; pos < (Board.colCount ); pos++)
            {
                if (!(squares[sliceLocation, pos].IsOccupied))
                {
                    break;
                }
                maxOccupied++;
            }

            return (minOccupied, maxOccupied);
        }

        // determine start and end location of occupied squares contiguous with specified squares
        internal (int start, int end) GetEndpointsVertical(int sliceLocation, List<int> locationList)
        {
            var minMove = locationList.Min();
            var maxMove = locationList.Max();
            var minOccupied = minMove;
            var maxOccupied = maxMove;

            for (int pos = minMove - 1; pos >= 0; pos--)
            {
                if (!(squares[pos, sliceLocation].IsOccupied))
                {
                    break;
                }
                minOccupied--;
            }

            for (int pos = maxMove + 1; pos < (Board.rowCount); pos++)
            {
                if (!(squares[pos, sliceLocation].IsOccupied))
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

            foreach (var r in Enumerable.Range(0, rowCount))
                foreach (var c in Enumerable.Range(0, colCount))
                    if (IsOccupied ? squares[r, c].IsOccupied : !squares[r, c].IsOccupied)
                        locationSquareList.Add(new LocationSquare(new Coord((R)r, (C)c), squares[r, c]));

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