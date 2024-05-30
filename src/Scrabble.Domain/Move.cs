using System;
using System.Collections.Generic;
using System.Linq;

namespace Scrabble.Domain
{
    public abstract class Move(Board board, string letters, Func<string, bool> IsWordValid)
    {
        protected List<Tile> Tiles = letters.LettersToTiles();
        protected int Length = letters.Length;
        protected Board board = board;

        static protected int LowerBound(int i) => Math.Max(0, i);
        protected int UpperBound(int i) => Math.Min(Length - 1, i);

        protected bool AreSlicesValid(Func<int, List<Square>> sliceFunc, int startIdx, int endIdx)
        {

            var invalidSequence = Enumerable
                .Range(LowerBound(startIdx), UpperBound(endIdx) - LowerBound(startIdx) + 1)
                .Select(sliceFunc)
                .Select(slice => slice.Select(s => s.Tile?.Letter ?? ' ').ToList())
                .Select(sequence => (sequence, result: sequence.IsValidSequence(IsWordValid)))
                .FirstOrDefault(t => !t.result.valid);

            if (invalidSequence != default)
            {
                throw new InvalidOperationException($"Invalid sequence found in: {invalidSequence.sequence} at: {invalidSequence.result.invalidWord}");
            }

            return true;
        }
    }

    public class HorizontalMove : Move
    {
        readonly int row;
        readonly int colStart;
        readonly int colEnd;

        public HorizontalMove(Board board, string letters, Func<string, bool> IsWordValid, Coord startFrom)
            : base(board, letters, IsWordValid)
        {
            row = startFrom.RowToValue();
            colStart = startFrom.ColToValue();
            colEnd = colStart + Length;
        }

        public bool IsValid() =>
            AreSlicesValid(board.GetRowSlice, LowerBound(row - 1), UpperBound(row + 1))
                &&
            AreSlicesValid(board.GetColumnSlice, LowerBound(colStart - 1), UpperBound(colEnd + 1));
    }

    public class VerticalMove : Move
    {
        readonly int col;
        readonly int rowStart;
        readonly int rowEnd;

        public VerticalMove(Board board, string letters, Func<string, bool> IsWordValid, Coord startFrom)
            : base(board, letters, IsWordValid)
        {
            col = startFrom.ColToValue();
            rowStart = startFrom.RowToValue();
            rowEnd = rowStart + Length;
        }

        public bool IsValid() =>
            AreSlicesValid(board.GetColumnSlice, LowerBound(col - 1), UpperBound(col + 1))
                &&
            AreSlicesValid(board.GetRowSlice, LowerBound(rowStart - 1), UpperBound(rowEnd + 1));
    }
}
