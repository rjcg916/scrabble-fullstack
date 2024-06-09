using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Scrabble.Domain
{

    public partial class Board
    {
        public static readonly int rowCount = R._15 - R._1 + 1;
        public static readonly int colCount = C.O - C.A + 1;

        public readonly Square[,] squares = new Square[rowCount, colCount];

        private const int LOWERBOUND = 0;
        private const int DIMENSION = 15;

        private int MovesMade = 0;

        internal readonly Func<string, bool> IsWordValid;
        public static Coord Star => new(R._8, C.H);
        public static int StarRow => Star.RowValue;
        public static int StarCol => Star.ColValue;

        // create a new Board
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
                        Coord startFrom,
                        List<Tile> tiles,
                        Placement placement) :
            this(IsWordValid)
        {
            switch (placement)
            {
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
        }
        public Board Copy() => new(this);

        public static bool DoesMoveTouchStar(List<TilePlacement> tileList) =>
          tileList.Exists(t => (t.Coord.Col == Board.Star.Col) && (t.Coord.Row == Board.Star.Row));

        public Square GetSquare(Coord loc) => squares[loc.RowValue, loc.ColValue];

        public Tile GetTile(Coord loc) => GetSquare(loc).Tile;

        public bool IsFirstMove() => MovesMade == 0;

        public bool IsOccupied(Coord coord) => squares[coord.RowValue, coord.ColValue].IsOccupied;
        public bool AreOccupied(List<Coord> locations) => locations.Select(l => IsOccupied(l)).Any();
 
        static public PlacementSpec 
            ToPlacementSpec(List<TilePlacement> tileList)
        {
            if (tileList.Select(c => c.Coord.RowValue).Distinct().Count() == 1)
            {
                var fixedLocation = tileList.Select(c => c.Coord.RowValue).First();
                var tileLocations = tileList.Select(tl => (tl.Coord.ColValue, tl.Tile)).ToList();
                return new (Placement.Horizontal, fixedLocation, tileLocations);

            } else if (tileList.Select(c => c.Coord.ColValue).Distinct().Count() == 1) {

                var fixedLocation = tileList.Select(c => c.Coord.ColValue).First();
                var tileLocations = tileList.Select(tl => (tl.Coord.RowValue, tl.Tile)).ToList();
                return new (Placement.Vertical, fixedLocation, tileLocations);

            } else
            {
                throw new Exception("Invalid Move");
            }
        }


        public (bool valid, List<PlacementError> errorList) IsBoardValid()
        {
            if (!IsOccupied(Star))
            {
                return (false, [ new(Placement.Star, 0, "STAR not occupied")]);
            }

            var invalidMessages = new List<PlacementError>();

            ValidateBoardSlices(GetRowSlice, Placement.Horizontal, invalidMessages);
            ValidateBoardSlices(GetColSlice, Placement.Vertical, invalidMessages);

            return invalidMessages.Count > 0 ? (false, invalidMessages) : (true, null);
        }

 
        public bool AreAllTilesContiguous(List<TilePlacement> tileList)
        {
            // make a copy of board for testing

            Board board = this.Copy();

            // place the new tiles
            foreach (var (coord, tile) in tileList)
            {
                board.squares[coord.RowValue, coord.ColValue].Tile = tile;
            }

            // make sure each of the new tiles is contiguous with another tile
            foreach (var (coord, _) in tileList)
            {
                bool isContiguous = false;

                // Check the four adjacent squares (up, down, left, right)
                var adjacentCoords = new List<Coord>
                    {
                        new((R)( Math.Max( coord.RowValue - 1, 0)), coord.Col), // Up
                        new((R)( Math.Min( coord.RowValue + 1, Board.rowCount - 1) ), coord.Col), // Down
                        new(coord.Row, (C)( Math.Max(coord.ColValue - 1, 0))), // Left
                        new(coord.Row, (C)( Math.Min(coord.ColValue + 1, Board.colCount - 1)))  // Right
                    };

                foreach (var adjCoord in adjacentCoords)
                {
                    if (adjCoord.RowValue >= 0 && adjCoord.RowValue < rowCount &&
                        adjCoord.ColValue >= 0 && adjCoord.ColValue < colCount)
                    {
                        if (board.squares[adjCoord.RowValue, adjCoord.ColValue].IsOccupied)
                        {
                            isContiguous = true;
                            break; // no need to search for another continguous tile
                        }
                    }
                }

                if (!isContiguous) // no need to examine other tiles
                {
                    return false;
                }
            }

            return true;
        }

        public int ScoreMove(List<TilePlacement> tileList)
        {
            PlacementSpec tileSpecs = ToPlacementSpec(tileList);

            var score = tileSpecs.Placement switch
            {
                Placement.Horizontal => ScoreMove(tileSpecs.FixedLocation, tileSpecs.TileLocations, false),
                Placement.Vertical => ScoreMove(tileSpecs.FixedLocation, tileSpecs.TileLocations, true),
                _ => throw new Exception("Invalid Placement"),
            };
            return score;
        }
        public void PlaceTiles(List<TilePlacement> tileList)
        {
            MovesMade++;

            foreach (var (coord, tile) in tileList)
            {
                var location = squares[coord.RowValue, coord.ColValue];

                location.Tile = new Tile(tile.Letter);
                location.MoveOfOccupation = MovesMade;
            };
        }

        public Board NextBoard(List<TilePlacement> tileList)
        {
            Board board = this.Copy();

            board.MovesMade++;

            foreach (var (coord, tile) in tileList)
            {
                var loc = board.squares[coord.RowValue, coord.ColValue];
                loc.Tile = tile;
                loc.MoveOfOccupation = MovesMade;
            };

            return board;
        }
        internal int ScoreMove(int fixedLocation, List<(int location, Tile tile)> tileLocations, bool isHorizontal)
        {
            int score = 0;

            var (singleRunStart, singleRunEnd) = GetRun(!isHorizontal, fixedLocation, tileLocations.Select(tl => tl.location).ToList());
            score += GetSlice(!isHorizontal, fixedLocation, singleRunStart, singleRunEnd).ScoreRun();

            for (int c = singleRunStart; c <= singleRunEnd; c++)
            {
                var (multiRunStart, multiRunEnd) = GetRun(isHorizontal, c, [fixedLocation]);
                score += GetSlice(isHorizontal,fixedLocation, multiRunStart, multiRunEnd).ScoreRun();
            }

            return score;
        }

        internal void ValidateBoardSlices(Func<int, List<Square>> getSlice,
                                     Placement placement,
                                     List<PlacementError> invalidMessages)
        {
            int sliceCount = Placement.Horizontal == placement ? Board.colCount : Board.colCount;
            for (int index = 0; index < sliceCount; index++)
            {
                var charList = getSlice(index).ToCharList();
                
                if (charList != null)
                {
                    var (valid, invalidWord) = charList.IsValidSequence(IsWordValid);

                    if (!valid)
                    {
                        invalidMessages.Add(new(placement, index, invalidWord));
                    }
                }
            }
        }

        internal List<Square> GetRowSlice(int row)
        {
            return GetSlice(true, row);
        }

        internal List<Square> GetColSlice(int col)
        {
            return GetSlice(false, col);
        }

        internal List<Square> GetSlice(bool isHorizontal, int index, int start = LOWERBOUND, int end = DIMENSION)
        {
            List<Square> slice = [];
            if (isHorizontal)
            {
                for (int col = start; col < end; col++)
                {
                    slice.Add(squares[index, col]);
                }
            }
            else
            {
                for (int row = start; row < end; row++)
                {
                    slice.Add(squares[row, index]);
                }
            }
            return slice;
        }

        internal (int start, int end) GetRun(bool isHorizontal, int fixedLocation, List<int> locationList)
        {
            var minMove = locationList.Min();
            var maxMove = locationList.Max();
            var minOccupied = minMove;
            var maxOccupied = maxMove;

            for (int pos = minMove - 1; pos >= 0; pos--)
            {
                if (!(isHorizontal ? squares[fixedLocation, pos].IsOccupied :
                                   squares[pos, fixedLocation].IsOccupied))
                {
                    break;
                }
                minOccupied--;
            }

            for (int pos = maxMove + 1; pos < (isHorizontal ? Board.colCount : Board.rowCount); pos++)
            {
                if (!(isHorizontal ? squares[fixedLocation, pos].IsOccupied :
                                    squares[pos, fixedLocation].IsOccupied))
                {
                    break;
                }
                maxOccupied++;
            }

            return (minOccupied, maxOccupied);
        }



        public static (bool valid, string invalidWord) IsSliceValid0(Func<string, bool> IsWordValid, List<Square> slice)
        {
            var charList = slice.ToCharList();
            return charList.IsValidSequence(IsWordValid);
        }

        //public List<EvaluateAt<Square>> GetLocationSquares(bool filterForOccupied = false)
        //{
        //    List<EvaluateAt<Square>> squareList = [];

        //    foreach (var r in Enumerable.Range(0, rowCount))
        //        foreach (var c in Enumerable.Range(0, colCount))
        //            if (squares[r, c].IsOccupied && filterForOccupied || !filterForOccupied)
        //                squareList.Add(new EvaluateAt<Square>(r, c, squares[r, c]));

        //    return squareList;
        //}
    }
}