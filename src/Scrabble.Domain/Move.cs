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
            end =  start + Length;

            // Validate the rows
            ValidateSlices(board.GetRowSlice, row - 1, row + 1);

            // Validate the columns
            ValidateSlices(board.GetColumnSlice, start - 1, end + 1);
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
                ValidateSlices(board.GetColumnSlice, col - 1, col + 1);

                // Validate the rows
                ValidateSlices(board.GetRowSlice, start - 1, end + 1);

            }
        } 
    }
}
 