using System;
using System.Collections.Generic;
using System.Linq;

namespace Scrabble.Domain
{
    public abstract class Move(string letters, Func<string, bool> IsWordValid)
    {
        protected List<Tile> Tiles = letters.LettersToTiles();
        protected int Length = letters.Length;
        protected int start;
        protected int end;

        protected void ValidateSlices(Func<int, List<Square>> sliceFunc, int startIdx, int endIdx)
        {
            for (int i = startIdx; i <= endIdx; i++)
            {
                var slice = sliceFunc(i);
                var sequence = slice.Select(s => s.Tile?.Letter ?? ' ').ToList();

                var (valid, invalidChar) = Board.ValidSequence(sequence, IsWordValid);

                if (!valid)
                {
                    throw new InvalidOperationException($"Invalid sequence found in: {sequence} at: {invalidChar}");
                }
            }
        }
    }
    public class HorizontalMove : Move
    {
        public HorizontalMove(Board board, string letters, Func<string, bool> IsWordValid, Coord startFrom)
            : base(letters, IsWordValid)
        {
            var row = startFrom.RowToValue();
            start = startFrom.ColToValue();
            end = start + Length;

            // Validate the rows
            ValidateSlices(board.GetRowSlice, Math.Max(0, row - 1), Math.Min(row + 1, Length - 1));

            // Validate the columns
            ValidateSlices(board.GetColumnSlice, Math.Max(0, start - 1), Math.Min(end + 1, Length - 1));
        }
    }
    
    public class VerticalMove : Move
    {
        public VerticalMove(Board board, string letters, Func<string, bool> IsWordValid, Coord startFrom)
            : base(letters, IsWordValid)
        {
            var col = startFrom.ColToValue();
            start = startFrom.RowToValue();
            end = start + Length;

            // Validate the columns
            ValidateSlices(board.GetColumnSlice, Math.Max(0, col - 1), Math.Min(col + 1, Length - 1));

            // Validate the rows
            ValidateSlices(board.GetRowSlice, Math.Max(0, start - 1), Math.Min(end + 1, Length - 1));
        }
    }
}
