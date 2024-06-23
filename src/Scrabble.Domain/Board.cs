using System.Collections.Generic;
using System.Linq;
using System;

namespace Scrabble.Domain
{
    public record LocationSquare(Coord Coord, Square Square);

    public partial class Board
    {
        public  Square[,] squares = new Square[Coord.RowCount, Coord.ColCount];

        /// <summary>
        /// "SquareBy" functions allow Higher Order Functions to operater in either Horizontal or Vertical direction
        /// </summary>       
        public Square SquareByRow(int row, int col) =>
            squares[row, col];
        public Square SquareByColumn(int col, int row) =>
            squares[row, col];

        public static readonly Coord STAR = new(R._8, C.H);

        public int MoveNumber = 0;

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

        public Board(Board other)
        {            
            IsWordValid = other.IsWordValid;
         
            MoveNumber = other.MoveNumber;

            foreach (var r in Coord.Rows)
                foreach (var c in Coord.Cols)
                    squares[(int)r, (int)c] = new Square(other.squares[(int)r, (int)c]);          
        }

        /// <summary>
        /// create a new Board with initial move
        /// </summary>>
        public Board(Func<string, bool> IsWordValid,
                     List<TilePlacement> tileList) :
            this(IsWordValid)
        {
            MakeMove(tileList);
        }

        /// <summary>
        /// create a new Board with initial move
        /// </summary>
        public Board(Func<string, bool> IsWordValid,
                        Coord startFrom,
                        List<Tile> tiles,
                        bool isHorizontal) :
            this(IsWordValid)
        {
            MakeMove(startFrom, tiles, isHorizontal);
        }

        // return a new board with tiles placed in the move
        public Board MakeMove(Move move) =>
            MakeMove(move.TilePlacements);

        public Board MakeMove(Coord startFrom, List<Tile> tiles, bool isHorizontal) =>
            MakeMove(GetTilePlacements(startFrom, tiles, isHorizontal));
        
        public Board MakeMove(List<TilePlacement> tileList)
        {
            this.MoveNumber++;

            foreach (var placement in tileList)
            {
                int row = placement.Coord.RVal;
                int col = placement.Coord.CVal;
                squares[row, col].Tile = new Tile(placement.Tile.Letter);
                squares[row, col].MoveNumber = MoveNumber;
            }
            return this;
        }



        public int ScoreMove(List<TilePlacement> tilePlacementList)
        {
            return new Score(this, tilePlacementList).Calculate();
        }

        public int ScoreMove(Move move)
        {
            return new Score(this, move).Calculate();
        }

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

        /// <summary>
        /// get list of coord and squares (by default, empty squares)
        /// </summary>
        public List<LocationSquare> GetLocationSquares(bool IsOccupied = false)
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

        /// <summary>
        /// place tiles on board
        /// </summary>
        private static List<TilePlacement> GetTilePlacements(Coord startFrom, List<Tile> tiles, bool isHorizontal) =>
                isHorizontal ? 
                 tiles.Select((tile, index) => new TilePlacement(new Coord(startFrom.Row, (C)(startFrom.CVal + index)), tile)).ToList()
               : tiles.Select((tile, index) => new TilePlacement(new Coord((R)(startFrom.RVal + index), startFrom.Col), tile)).ToList();
            
        public static bool DoesMoveTouchSTAR(List<TilePlacement> tileList) =>
             tileList.Exists(t => (t.Coord.Col == Board.STAR.Col) && (t.Coord.Row == Board.STAR.Row));

        public bool IsOccupied(Coord coord) => squares[coord.RVal, coord.CVal].IsOccupied;
        public bool AreOccupied(List<Coord> locations) => locations.Any(l => IsOccupied(l));

        public (bool valid, List<PlacementError> errorList) IsMoveValid(List<TilePlacement> moveToTest)
        {
            Board board = new(this);

            board.MakeMove(moveToTest);

            if (!IsOccupied(STAR))
            {
                return (false, [new(Board.STAR, "STAR not occupied")]);
            }

            var (areTilesContiguous, placementError) = board.TilesContiguousOnBoard(moveToTest);

            if (!areTilesContiguous)
            {
                return (false, [placementError]);
            }

            return board.BoardContainsOnlyValidWords();
        }

        public (bool valid, List<PlacementError> errorList) BoardContainsOnlyValidWords()
        {
            var invalidMessages = new List<PlacementError>();

            invalidMessages.AddRange(ValidateWordSlices(r => GetSquares(SquareByColumn, r, (0, Coord.ColCount - 1)), Coord.ColCount, true));
            invalidMessages.AddRange(ValidateWordSlices(c => GetSquares(SquareByRow, c, (0, Coord.RowCount - 1)), Coord.RowCount, false));

            return invalidMessages.Count > 0 ? (false, invalidMessages) : (true, null);
        }

        public (bool valid, PlacementError error) TilesContiguousOnBoard(List<TilePlacement> tilePlacementList)
        {
            // make sure each of the tiles in list is contiguous with another tile
            foreach (var (coord, _) in tilePlacementList)
            {
                bool isContiguous = false;

                var adjacentCoords = coord.GetAdjacent();

                foreach (var adjCoord in adjacentCoords)
                {
                    if (squares[adjCoord.RVal, adjCoord.CVal].IsOccupied)
                    {
                        isContiguous = true;
                        break; // no need to search for another continguous tile
                    }
                }

                if (!isContiguous) // no need to examine other tiles
                {
                    return (false, new PlacementError(coord, "Tile Not Contiguous"));
                }
            }

            return (true, new PlacementError(Board.STAR, "No Error"));
        }

        /// <summary>
        /// apply word validity check across a slice (row or col) of the board
        /// </summary>
        internal List<PlacementError> ValidateWordSlices(
                                            Func<int, List<Square>> getSquares,
                                            int sliceCount,
                                            bool isHorizontal)
        {
            List<PlacementError> invalidMessages = [];

            for (int index = 0; index < sliceCount; index++)
            {
                var sqList = getSquares(index);
                var charList = sqList.ToCharList();

                if (charList != null)
                {
                    var (valid, invalidWord) = charList.IsValidWordList(IsWordValid);

                    if (!valid)
                    {
                        var coord = isHorizontal
                                ? new Coord((R)index, 0) : new Coord(0, (C)index);

                        invalidMessages.Add(new(coord, invalidWord));
                    }
                }
            }

            return invalidMessages;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var other = (Board)obj;


            if (!IsWordValid.Method.Equals(other.IsWordValid.Method))
            {
                return false;
            }

            if (MoveNumber != other.MoveNumber)
            {
                return false;
            }

            for (int r = 0; r < Coord.RowCount; r++)
            {
                for (int c = 0; c < Coord.ColCount; c++)
                {
                    if (!squares[r, c].Equals(other.squares[r, c]))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public override int GetHashCode()
        {
            var hashCode = new HashCode();

            // Combine hash codes of relevant properties
            hashCode.Add(IsWordValid.Method);
            hashCode.Add(MoveNumber);

            for (int r = 0; r < Coord.RowCount; r++)
            {
                for (int c = 0; c < Coord.ColCount; c++)
                {
                    hashCode.Add(squares[r, c]);
                }
            }

            return hashCode.ToHashCode();
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