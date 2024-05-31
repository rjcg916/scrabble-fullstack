using System;
using System.Collections.Generic;
using System.Linq;

namespace Scrabble.Domain
{
    public class EvaluatorFor<T>(int row, int col, T evaluator = default)
    {
        public T Evaluator { get; set; } = evaluator;
        public int Row { get; set; } = row;
        public string RowName { get; set; } = ((R)row).ToString()[1..];
        public int Col { get; set; } = col;
        public string ColName { get; set; } = ((C)col).ToString()[0..];
    }

    public class Board
    {
        public static readonly int rowCount = R._15 - R._1 + 1;
        public static readonly int colCount = C.O - C.A + 1;

        readonly Square[,] squares = new Square[rowCount, colCount];

        public Board()
        {
            foreach (var r in Enumerable.Range(0, rowCount))
                foreach (var c in Enumerable.Range(0, colCount))
                    squares[r, c] = new Square();

            Initialize();
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

        public Board Copy() =>
            new (this);
        
        public Square GetSquare(Coord loc) =>
            squares[loc.RowToValue(), loc.ColToValue()];

        public Tile GetTile(Coord loc) =>
            GetSquare(loc).Tile;

        public bool IsOccupied(Coord coord) =>
            squares[coord.RowToValue(), coord.ColToValue()].IsOccupied;
       
        public bool IsOccupied(Coord startCoord, Coord endCoord)
        {
            int startRow = startCoord.RowToValue();
            int endRow = endCoord.RowToValue();
            int startCol = startCoord.ColToValue();
            int endCol = endCoord.ColToValue();

            if (startRow == endRow)     // If checking a row
            {
                return IsOccupiedRange(startRow, startCol, endCol, true);
            }
            else if (startCol == endCol) // If checking a column
            {
                return IsOccupiedRange(startCol, startRow, endRow, false);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private bool IsOccupiedRange(int fixedValue, int start, int end, bool isRow)
        {
            for (int i = start; i <= end; i++)
            {
                var squareValue = isRow ? squares[fixedValue, i] : squares[i, fixedValue];
                if (squareValue.IsOccupied)
                    return true;

            }
            return false;
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

        public List<EvaluatorFor<Square>> GetCoordSquares(bool filterForOccupied = false)
        {
            List<EvaluatorFor<Square>> squareList = [];

            foreach (var r in Enumerable.Range(0, rowCount))
                foreach (var c in Enumerable.Range(0, colCount))
                    if (squares[r, c].IsOccupied && filterForOccupied || !filterForOccupied)
                        squareList.Add(new EvaluatorFor<Square>(r, c, squares[r, c]));

            return squareList;
        }

        public bool PlaceTile(Coord coord, Tile tile)
        {
            bool isSuccessful;

            var square = squares[coord.RowToValue(), coord.ColToValue()];

            if (IsOccupied(coord))
                isSuccessful = false;
            else
            {
                square.Tile = tile;
                isSuccessful = true;
            }
            return isSuccessful;
        }

        protected Board PlaceTiles(int fixedValue, int start, string letters, bool isRow)
        {
            var charLength = letters.Length;

            for (int i = start; i <= start + charLength; i++)
            {
                if (isRow)
                    this.squares[fixedValue, i].Tile = new Tile(letters[i]); 
                
            }            

            return this;
        }

        public Board PlaceTilesInRow(Coord startFrom, string letters)
        {
            Board boardCopy = this.Copy();
            var start = startFrom.ColToValue();
            return boardCopy.PlaceTiles(startFrom.RowToValue(), start, letters, true);            
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
                squares[(int)loc.Row, (int)loc.Col].SquareType = t;
            }
        }

    }
}