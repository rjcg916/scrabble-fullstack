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

        static readonly int rowCount = R._15 - R._1 + 1;
        static readonly int colCount = C.O - C.A + 1;

        readonly Square[,] board = new Square[rowCount, colCount];

        public Board()
        {
            foreach (var r in Enumerable.Range(0, rowCount))
                foreach (var c in Enumerable.Range(0, colCount))
                    board[r, c] = new Square();

            SetAllSquareTypes();
        }

        public Square GetSquare(Coord loc) =>
            board[loc.RowToValue(), loc.ColToValue()];

        public Tile GetTile(Coord loc) =>
            GetSquare(loc).Tile;

        public bool IsOccupied(Coord coord) =>
            board[coord.RowToValue(), coord.ColToValue()].IsOccupied;

        public List<Square> GetRowSlice(int row)
        {
            List<Square> slice = [];
            for (int col = 0; col < colCount; col++)
            {
                slice.Add(board[row, col]);
            }
            return slice;
        }

        public List<Square> GetColumnSlice(int col)
        {
            List<Square> slice = [];
            for (int row = 0; row < rowCount; row++)
            {
                slice.Add(board[row, col]);
            }
            return slice;
        }

        public List<EvaluatorFor<Square>> GetCoordSquares(bool filterForOccupied = false)
        {
            List<EvaluatorFor<Square>> squares = [];

            foreach (var r in Enumerable.Range(0, rowCount))
                foreach (var c in Enumerable.Range(0, colCount))
                    if (board[r, c].IsOccupied && filterForOccupied || !filterForOccupied)
                        squares.Add(new EvaluatorFor<Square>(r, c, board[r, c]));

            return squares;
        }

        public bool PlaceTile(Coord coord, Tile tile)
        {
            bool isSuccessful;

            var square = board[coord.RowToValue(), coord.ColToValue()];

            if (IsOccupied(coord))
                isSuccessful = false;
            else
            {
                square.Tile = tile;
                isSuccessful = true;
            }

            return isSuccessful;
        }

 
        static public (bool valid, char invalidChar) ValidSequence(List<char> charArray, Func<string, bool> IsWordValid)
        {
            char[] separator = [' ', '\t', '\n', '\r'];

            string input = charArray.ToString();

            // Split the input string by whitespace
            var words = input.Split(separator, StringSplitOptions.RemoveEmptyEntries);

            // Check each word using the IsWordValid function
            foreach (var word in words)
            {
                if (!IsWordValid(word))
                {
                    // Return false and the first invalid character
                    return (false, word.First());
                }
            }

            // If all words are valid, return true
            return (true, '\0'); // '\0' is the null character indicating no invalid character
        }


        private void SetAllSquareTypes()
        {
            // start
            board[(int)R._8, (int)C.H].SquareType = SquareType.start;

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
                board[(int)loc.Row, (int)loc.Col].SquareType = t;
            }
        }

    }
}