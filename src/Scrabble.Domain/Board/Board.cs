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

            foreach (var r in Coord.Rows) //Enumerable.Range(0, Coord.RowCount))
                foreach (var c in Coord.Cols) // Enumerable.Range(0, Coord.ColCount))
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

        // clone a Board        
        //public Board(Board other)
        //{
        //    for (int r = 0; r < Coord.RowCount; r++)
        //    {
        //        for (int c = 0; c < Coord.ColCount; c++)
        //        {
        //            squares[r, c] = other.squares[r, c].Copy();
        //        }
        //    }
        //    IsWordValid = other.IsWordValid;
        //    MovesMade = other.MovesMade;
        //}

        public Board(Board other)
        {
            foreach (var r in Coord.Rows)
                foreach (var c in Coord.Cols)
                    squares[(int)r, (int)c] = other.squares[(int)r, (int)c].Copy();
                            
            IsWordValid = other.IsWordValid;
            MovesMade = other.MovesMade;
        }

        //public Board Copy() => new(this);

        // vert
        public Square SquareByRow(int row, int col) =>
            squares[row, col];

        // hori
        public Square SquareByColumn(int col, int row) =>
            squares[row, col];

        internal List<Square> GetSquares(Func<int, int, Square> GetSquare, int sliceLocation, int rangeStart, int rangeEnd )
        {
            List<Square> slice = [];

            for (int location = rangeStart; location <= rangeEnd; location++)
            {
                var sq = GetSquare(location, sliceLocation);
                if (sq.IsOccupied)
                    slice.Add(sq.Copy());
            }
            return slice;
        }

        //// retrieve contents of squares from a specified range
        //internal List<Square> GetSquaresVerticalX(int sliceLocation, int rangeStart, int rangeEnd)
        //{
        //    List<Square> slice = [];

        //    for (int row = rangeStart; row <= rangeEnd; row++)
        //    {
        //        var sq = squares[row, sliceLocation];
        //        if (sq.IsOccupied)
        //            slice.Add(sq.Copy());
        //    }
        //    return slice;
        //}


        //internal List<Square> GetSquaresHorizontalX(int sliceLocation, int rangeStart, int rangeEnd)
        //{
        //    List<Square> slice = [];

        //    for (int col = rangeStart; col <= rangeEnd; col++)
        //    {
        //        var sq = squares[sliceLocation, col];
        //        if (sq.IsOccupied)
        //            slice.Add(sq.Copy());
        //    }

        //    return slice;
        //}

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

        //// determine start and end location of occupied squares contiguous with specified squares
        //internal (int start, int end) GetEndpointsHorizontalx(int sliceLocation, List<int> locationList)
        //{
        //    var minMove = locationList.Min();
        //    var maxMove = locationList.Max();
        //    var minOccupied = minMove;
        //    var maxOccupied = maxMove;

        //    for (int pos = minMove - 1; pos >= 0; pos--)
        //    {
        //        if (!(squares[sliceLocation, pos].IsOccupied ))
        //        {
        //            break;
        //        }
        //        minOccupied--;
        //    }

        //    for (int pos = maxMove + 1; pos < (Coord.ColCount ); pos++)
        //    {
        //        if (!(squares[sliceLocation, pos].IsOccupied))
        //        {
        //            break;
        //        }
        //        maxOccupied++;
        //    }

        //    return (minOccupied, maxOccupied);
        //}

        //// determine start and end location of occupied squares contiguous with specified squares
        //internal (int start, int end) GetEndpointsVerticalx(int sliceLocation, List<int> locationList)
        //{
        //    var minMove = locationList.Min();
        //    var maxMove = locationList.Max();
        //    var minOccupied = minMove;
        //    var maxOccupied = maxMove;

        //    for (int pos = minMove - 1; pos >= 0; pos--)
        //    {
        //        if (!(squares[pos, sliceLocation].IsOccupied))
        //        {
        //            break;
        //        }
        //        minOccupied--;
        //    }

        //    for (int pos = maxMove + 1; pos < (Coord.RowCount); pos++)
        //    {
        //        if (!(squares[pos, sliceLocation].IsOccupied))
        //        {
        //            break;
        //        }
        //        maxOccupied++;
        //    }

        //    return (minOccupied, maxOccupied);
        //}


  

        // determine start and end location of occupied squares contiguous with specified squares
        internal (int start, int end) GetEndpoints(Func<int, int, Square> GetSquare, int sliceLocation, List<int> locationList)
        {
   
            var minMove = locationList.Min();
            var maxMove = locationList.Max();
            var minOccupied = minMove;
            var maxOccupied = maxMove;

            for (int pos = minMove - 1; pos >= 0; pos--)
            {
                if (!( GetSquare(pos, sliceLocation).IsOccupied )) 
                {
                    break;
                }
                minOccupied--;
            }

            var maxCount = (GetSquare == this.SquareByRow) ? Coord.RowCount : Coord.ColCount;

            for (int pos = maxMove + 1; pos < maxCount; pos++) 
            {
                if (!(GetSquare(pos, sliceLocation).IsOccupied))
                {
                    break;
                }
                maxOccupied++;
            }

            return (minOccupied, maxOccupied);
        }

        //public List<LocationSquare> GetLocationSquares(bool IsOccupied = false)
        //{
        //    List<LocationSquare> locationSquareList = [];

        //    foreach (var r in Enumerable.Range(0, Coord.RowCount))
        //        foreach (var c in Enumerable.Range(0, Coord.ColCount))
        //            if (IsOccupied ? squares[r, c].IsOccupied : !squares[r, c].IsOccupied)
        //                locationSquareList.Add(new LocationSquare(new Coord((R)r, (C)c), squares[r, c]));

        //    return locationSquareList;
        //}

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