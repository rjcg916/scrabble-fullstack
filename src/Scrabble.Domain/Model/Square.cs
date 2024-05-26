namespace Scrabble.Domain.Model
{
    public enum SquareType
    {
        reg, dl, tl, dw, tw, start
    }

    public class Square(SquareType squareType = SquareType.reg)
    {
        public SquareType SquareType { get; set; } = squareType;

        public bool IsFinal { get; set; } = false;
        public Tile Tile { get; set; }

        public bool IsOccupied
        {
            get
            {
                return Tile != null;
            }

        }
        public int LetterMultiplier
        {
            get
            {
                if (SquareType == SquareType.tl)
                    return 3;
                else if (SquareType == SquareType.dl)
                    return 2;
                else
                    return 1;
            }
        }

        public int WordMultiplier
        {
            get
            {
                if (SquareType == SquareType.tw)
                    return 3;
                else if (SquareType == SquareType.dw)
                    return 2;
                else
                    return 1;
            }
        }

    }
    public class CoordSquare(int row, int col, Square square = null)
    {

        public Square Square { get; set; } = square;
        public int Row { get; set; } = row;
        public string RowName { get; set; } = ((R)row).ToString()[1..];
        public int Col { get; set; } = col;
        public string ColName { get; set; } = ((C)col).ToString()[1..];
    }
}
