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


        static public (Placement placement, int fixedLocation, List<(int, Tile)> tileLocations) ToLocationsTile(List<(Coord coord, Tile tile)> tileList)
        {
            if (tileList.Select(c => c.coord.RowValue).Distinct().Count() == 1)
            {
                var fixedLocation = tileList.Select(c => c.coord.RowValue).First();
                var tileLocations = tileList.Select(tl =>  (tl.coord.ColValue, tl.tile)).ToList();
                return (Placement.Horizontal, fixedLocation, tileLocations);
      
            } else if (tileList.Select(c => c.coord.ColValue).Distinct().Count() == 1) {
                var fixedLocation = tileList.Select(c => c.coord.ColValue).First();
                var tileLocations = tileList.Select(tl => (tl.coord.RowValue, tl.tile)).ToList();
                return (Placement.Vertical, fixedLocation, tileLocations);
        
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

        public int ScoreRun(int loc, int start, int end, bool isVertical)
        {
            int score = 0;
            var wordMultiplier = 1;

            for (int pos = start; pos <= end; pos++)
            {              
                var location = isVertical ? squares[pos, loc]: squares[loc, pos];
                var squareType = location.SquareType;
                var tileValue  = location.Tile.Value;

                switch (squareType)
                {
                    case SquareType.start:
                    case SquareType.reg:
                        score += tileValue;
                        break;

                    case SquareType.dl:
                        score += (tileValue * 2);
                        break;

                    case SquareType.tl:
                        score += (tileValue * 3);
                        break;

                    case SquareType.dw:
                        score += (tileValue);
                        wordMultiplier *= 2;
                        break;

                    case SquareType.tw:
                        score += (tileValue);
                        wordMultiplier *= 3;
                        break;

                    default:
                        throw new Exception("Unkown SquareType");                       
                }
            }
            return score;
        }

        public int ScoreMove(List<(Coord coord, Tile tile)> tileList)
        {
            (Placement placement, int fixedLocation, List<(int location, Tile tile)> tileLocations) =
                ToLocationsTile(tileList);

            var score = placement switch
            {
                Placement.Horizontal => ScoreMove(fixedLocation, tileLocations, false),
                Placement.Vertical => ScoreMove(fixedLocation, tileLocations, true),
                _ => throw new Exception("Invalid Placement"),
            };
            return score;
        }

        private int ScoreMove(int fixedLocation, List<(int location, Tile tile)> tileLocations, bool isVertical)
        {
            int score = 0;

            var singleRun = GetRun(fixedLocation, tileLocations.Select(tl => tl.location).ToList(), isVertical);
            score += ScoreRun(fixedLocation, singleRun.start, singleRun.end, isVertical);

            for (int c = singleRun.start; c <= singleRun.end; c++)
            {
                var multiRun = GetRun(c, [fixedLocation], !isVertical);
                score += ScoreRun(fixedLocation, multiRun.start, multiRun.end, !isVertical);
            }

            return score;
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