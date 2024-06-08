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

        private int MovesMade = 0;
 
        private readonly Func<string, bool> IsWordValid;
        
        public static Coord Star => new(R._8, C.H);
        public static int StarRow => Star.RowValue;
        public static int StarCol => Star.ColValue;

        public Board Copy() => new(this);

        public Square GetSquare(Coord loc) => squares[loc.RowValue, loc.ColValue];

        public Tile GetTile(Coord loc) => GetSquare(loc).Tile;

        public bool IsFirstMove() => MovesMade == 0;

        public bool IsOccupied(Coord coord) => squares[coord.RowValue, coord.ColValue].IsOccupied;
        public bool AreOccupied(List<Coord> locations) => locations.Select(l => IsOccupied(l)).Any();


        static public (int fixedLocation, List<(int, Tile)> tileLocations) ToLocationsTile(List<(Coord coord, Tile tile)> tileList)
        {
            if (tileList.Select(c => c.coord.RowValue).Distinct().Count() == 1)
            {
                var fixedLocation = tileList.Select(c => c.coord.RowValue).First();
                var tileLocations = tileList.Select(tl =>  (tl.coord.ColValue, tl.tile)).ToList();
                return (fixedLocation, tileLocations);
            } else if (tileList.Select(c => c.coord.ColValue).Distinct().Count() == 1) {
                var fixedLocation = tileList.Select(c => c.coord.ColValue).First();
                var tileLocations = tileList.Select(tl => (tl.coord.RowValue, tl.tile)).ToList();
                return (fixedLocation, tileLocations);
            } else
            {
                throw new Exception("Invalid Move");
            }

        }
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
        public Board(Func<string, bool> IsWordValid, Coord startFrom, List<Tile> tiles, Placement placement) :
            this(IsWordValid)
        {
            switch (placement)
            {
                case Placement.Horizontal:
                    PlaceTiles(tiles.Select((tile, index) =>
                        (new Coord(startFrom.Row, (C)(startFrom.ColValue + index)), tile)).ToList());
                    break;

                case Placement.Vertical:
                    PlaceTiles(tiles.Select((tile, index) =>
                        (new Coord((R)(startFrom.RowValue + index), startFrom.Col), tile)).ToList());
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
        private List<Square> GetSlice(int index, bool isRow)
        {
            List<Square> slice = [];
            if (isRow)
            {
                for (int col = 0; col < colCount; col++)
                {
                    slice.Add(squares[index, col]);
                }
            }
            else
            {
                for (int row = 0; row < rowCount; row++)
                {
                    slice.Add(squares[row, index]);
                }
            }
            return slice;
        }

        public List<Square> GetRowSlice(int row)
        {
            return GetSlice(row, true);
        }

        public List<Square> GetColSlice(int col)
        {
            return GetSlice(col, false);
        }

        public (bool valid, string invalidWord) IsSliceValid(List<Square> slice)
        {
            var charList = slice.Select(square => square.Tile?.Letter ?? ' ').ToList();
            return charList.IsValidSequence(IsWordValid);
        }

        public List<EvaluateAt<Square>> GetLocationSquares(bool filterForOccupied = false)
        {
            List<EvaluateAt<Square>> squareList = [];

            foreach (var r in Enumerable.Range(0, rowCount))
                foreach (var c in Enumerable.Range(0, colCount))
                    if (squares[r, c].IsOccupied && filterForOccupied || !filterForOccupied)
                        squareList.Add(new EvaluateAt<Square>(r, c, squares[r, c]));

            return squareList;
        }

        static public bool DoesMoveTouchStar(List<(Coord coord, Tile tile)> tileList) =>
            tileList.Exists(t => (t.coord.Col == Board.Star.Col) && (t.coord.Row == Board.Star.Row));

        public bool AreAllTilesContiguous(List<(Coord coord, Tile tile)> tileList)
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

        private (int start, int end) GetRun(int fixedLocation, List<int> locationList, bool isVertical)
        {
            var minMove = locationList.Min();
            var maxMove = locationList.Max();
            var minOccupied = minMove;
            var maxOccupied = maxMove;

            for (int pos = minMove - 1; pos >= 0; pos--)
            {
                if (!(isVertical ? squares[pos, fixedLocation].IsOccupied : squares[fixedLocation, pos].IsOccupied))
                {
                    break;
                }
                minOccupied--;
            }

            for (int pos = maxMove + 1; pos < (isVertical ? Board.rowCount : Board.colCount); pos++)
            {
                if (!(isVertical ? squares[pos, fixedLocation].IsOccupied : squares[fixedLocation, pos].IsOccupied))
                {
                    break;
                }
                maxOccupied++;
            }

            return (minOccupied, maxOccupied);
        }

        public (int start, int end) GetVerticalRun(List<Coord> coordList)
        {
            var fixedCol = coordList.Select(c => c.ColValue).First();
            var locList = coordList.Select(c => c.RowValue).ToList();
            return GetRun(fixedCol, locList, true);
        }

        public (int start, int end) GetHorizontalRun(List<Coord> coordList)
        {
            var fixedRow = coordList.Select(c => c.RowValue).First();
            var locList = coordList.Select(c => c.ColValue).ToList();
            return GetRun(fixedRow, locList, false);
        }

        public int ScoreMove(List<(Coord coord, Tile tile)> tileList)
        {
            // horizontal
            // score horizonal run
            //  GetRun(fixedRow, locList, false);
            // find and score any vertical runs
              // look at all columns from start to end of row
                  //  for c col from start to end
                      // GetRun(c, locList= single entry (fixedRow,c), true)
            // score
            // score letter 
            // adjust letter score
            // score words (adjusted letter score)
            // adjust word score
            // report total
            // total(wordscore(letter scores)

            return 0;
        }

        public void PlaceTiles(List<(Coord coord, Tile tile)> tileList)
        {
            MovesMade++;

            foreach (var (coord, tile) in tileList)
            {
                var loc = squares[coord.RowValue, coord.ColValue];
                loc.Tile = new Tile(tile.Letter);
                loc.MoveOfOccupation = MovesMade;
            };
        }

        public Board NextBoard(List<(Coord coord, Tile tile)> tileList)
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

        public (bool valid, List<(Placement errorType, int loc, string letters)> errorList) IsBoardValid()
        {
            if (!IsOccupied(Star))
            {
                return (false, new List<(Placement, int, string)> { (Placement.Star, 0, "STAR not occupied") });
            }

            var invalidMessages = new List<(Placement errorType, int location, string letters)>();

            ValidateSlices(rowCount, GetRowSlice, Placement.Horizontal, invalidMessages);
            ValidateSlices(colCount, GetColSlice, Placement.Vertical, invalidMessages);

            return invalidMessages.Count > 0 ? (false, invalidMessages) : (true, null);
        }

        private void ValidateSlices(int count, 
                                    Func<int, List<Square>> getSlice, 
                                    Placement placement, 
                                    List<(Placement errorType, int loc, string letters)> invalidMessages)
        {
            for (int index = 0; index < count; index++)
            {
                var (valid, invalidWord) = IsSliceValid(getSlice(index));
                if (!valid)
                {
                    invalidMessages.Add((placement, index, invalidWord));
                }
            }
        }

    }
}