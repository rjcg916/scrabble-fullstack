namespace Scrabble.Domain
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
}