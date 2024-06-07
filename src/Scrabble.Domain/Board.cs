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

        private int MovesMadeCount = 0;
 
        private readonly Func<string, bool> IsWordValid;
        
        public static Coord Star => new(R._8, C.H);
        public static int StarRow => Star.RowValue;
        public static int StarCol => Star.ColValue;

        public Board Copy() => new(this);

        public Square GetSquare(Coord loc) => squares[loc.RowValue, loc.ColValue];

        public Tile GetTile(Coord loc) => GetSquare(loc).Tile;

        public bool IsFirstMove() => MovesMadeCount == 0;

        public bool IsOccupied(Coord coord) => squares[coord.RowValue, coord.ColValue].IsOccupied;
 //       public bool IsOccupied(List<Coord> locations) => locations.Select(l => IsOccupied(l)).Any();


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


        public List<Square> GetRowSlice(int row)
        {
            List<Square> slice = [];
            for (int col = 0; col < colCount; col++)
            {
                slice.Add(squares[row, col]);
            }
            return slice;
        }

        public List<Square> GetColSlice(int col)
        {
            List<Square> slice = [];
            for (int row = 0; row < rowCount; row++)
            {
                slice.Add(squares[row, col]);
            }
            return slice;
        }

        public (bool valid, string invalidWord) ToValidatedString(List<Square> slice)
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

        public void PlaceTiles(List<(Coord coord, Tile tile)> tileList)
        {
            foreach (var (coord, tile) in tileList)
            {
                squares[coord.RowValue, coord.ColValue].Tile = new Tile(tile.Letter);
            };

            MovesMadeCount++;            
        }

        public Board NextBoard(List<(Coord coord, Tile tile)> tileList)
        {
            Board board = this.Copy();

            foreach (var (coord, tile) in tileList)
            {
                board.squares[coord.RowValue, coord.ColValue].Tile = tile;
            };

            board.MovesMadeCount++;

            return board;
        }


        public (bool valid, List<(Placement errorType, int loc, string letters)> errorList) IsBoardValid()
        {
            if (!IsOccupied(Star))
               return (false, [(Placement.Star, 0, "STAR not occupied")]);

            var invalidMessages = new List<(Placement errorType, int location, string letters)>();

            // Check that rows are valid

            for (int row = 0; row < rowCount; row++)
            {
                var (valid, invalidWord) = ToValidatedString(GetRowSlice(row));

                if (!valid)
                {
                    invalidMessages.Add((Placement.Horizontal, row, invalidWord));
                }
            }

            // Check that columns are valid

            for (int col = 0; col < colCount; col++)
            {
                var (valid, invalidWord) = ToValidatedString(GetColSlice(col));
                if (!valid)
                {
                    invalidMessages.Add((Placement.Vertical, col, invalidWord));
                }
            }

            return invalidMessages.Count > 0 ? (false, invalidMessages) : (true, null);
        }
    }
}