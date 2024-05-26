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
        public int LetterMultiplier => SquareType switch
        {
            SquareType.tl => 3,
            SquareType.dl => 2,
            _ => 1
        };


        public int WordMultiplier => SquareType switch
        {
            SquareType.tw => 3,
            SquareType.dw => 2,
            _ => 1
        };

    }
    public class CoordSquare(ushort row, ushort col, Square square = null)
    {

        public Square Square { get; set; } = square;
        public ushort Row { get; set; } = row;
        public string RowName { get; set; } = ((R)row).ToString()[1..];
        public ushort Col { get; set; } = col;
        public string ColName { get; set; } = ((C)col).ToString()[1..];
    }
}
