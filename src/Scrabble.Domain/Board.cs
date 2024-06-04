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


        public Board Copy() => new(this);

        public Square GetSquare(Coord loc) => squares[loc.RowValue, loc.ColValue];

        public Tile GetTile(Coord loc) => GetSquare(loc).Tile;

        public static Coord Start => new(R._8, C.H);

        public static int GameStartRow => Start.RowValue;
        public static int GameStartCol => Start.ColValue;

        public bool IsFirstMove() => MovesMadeCount == 0;

        public bool IsOccupied(Coord coord) => squares[coord.RowValue, coord.ColValue].IsOccupied;
        public bool IsOccupied(List<Coord> locations) => locations.Select(l => IsOccupied(l)).Any();


        Func<string, bool> IsWordValid;

        public Board(Func<string, bool> IsWordValid)
        {
            this.IsWordValid = IsWordValid;

            foreach (var r in Enumerable.Range(0, rowCount))
                foreach (var c in Enumerable.Range(0, colCount))
                    squares[r, c] = new Square();

            Initialize();
        }
        
        public Board(Func<string, bool> IsWordValid, Coord startFrom, List<Tile> tiles, Placement placement) : this(IsWordValid) {

            if (placement == Placement.Horizontal)
            {
                PlaceTiles(tiles.Select((tile, index) =>
                    (new Coord(startFrom.Row, (C)(startFrom.ColValue + index)), tile)).ToList());

            }
            else if (placement == Placement.Vertical)
            {
                PlaceTiles(tiles.Select((tile, index) =>
                    (new Coord((R)(startFrom.RowValue + index), startFrom.Col), tile)).ToList());
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(placement), placement, null);
            }
        }
        
        public Board(Board other)
        {
            for (int r = 0; r < rowCount; r++)
            {
                for (int c = 0; c < colCount; c++)
                {
                    squares[r, c] = other.squares[r, c].Copy();
                }
            }
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

        public List<Square> GetColumnSlice(int col)
        {
            List<Square> slice = [];
            for (int row = 0; row < rowCount; row++)
            {
                slice.Add(squares[row, col]);
            }
            return slice;
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


        public bool DoesMoveTouchStart(List<(Coord coord, Tile tile)> tileList) =>
            tileList.Exists(t => (t.coord.Col == Board.Start.Col) && (t.coord.Row == Board.Start.Row));

 

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
            foreach (var (coord, tile) in tileList)
            {
                bool isContiguous = false;

                // Check the four adjacent squares (up, down, left, right)
                var adjacentCoords = new List<Coord>
                    {
                        new Coord((R)( Math.Max( coord.RowValue - 1, 0)), coord.Col), // Up
                        new Coord((R)( Math.Min( coord.RowValue + 1, Board.rowCount - 1) ), coord.Col), // Down
                        new Coord(coord.Row, (C)( Math.Max(coord.ColValue - 1, 0))), // Left
                        new Coord(coord.Row, (C)( Math.Min(coord.ColValue + 1, Board.colCount - 1)))  // Right
                    };

                foreach (var adjCoord in adjacentCoords)
                {
                    if (adjCoord.RowValue >= 0 && adjCoord.RowValue < rowCount &&
                        adjCoord.ColValue >= 0 && adjCoord.ColValue < colCount)
                    {
                        if (board.squares[adjCoord.RowValue, adjCoord.ColValue].IsOccupied)
                        {
                            isContiguous = true;
                            break;
                        }
                    }
                }

                if (!isContiguous)
                {
                    return false; // The tile is not contiguous with another tile
                }
            }

 
            return true;

        }
    
        public Board PlaceTiles(List<(Coord coord, Tile tile)> tileList)
        {                  

            Board board = this.Copy();

            foreach (var (coord, tile) in tileList)
            {
                board.squares[coord.RowValue, coord.ColValue].Tile = tile;
            };
       
            return board;
        }

        public (bool valid, string invalidWord) IsBoardValid()
        {
            // Check that rows are valid
            for (int row = 0; row < rowCount; row++)
            {
                var rowSlice = GetRowSlice(row);
                var charList = rowSlice.Select(square => square.Tile?.Letter ?? ' ').ToList();

                var (valid, invalidWord) = charList.IsValidSequence(IsWordValid);
                if (!valid)
                {
                    return (false, invalidWord);
                }
            }

            // Check that columns are valid
            for (int col = 0; col < colCount; col++)
            {
                var colSlice = GetColumnSlice(col);
                var charList = colSlice.Select(square => square.Tile?.Letter ?? ' ').ToList();

                var (valid, invalidWord) = charList.IsValidSequence(IsWordValid);
                if (!valid)
                {
                    return (false, invalidWord);
                }
            }

            return (true, null);
        }
    }
}