using System;

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
        public int MoveNumber { get; set; } = 0;

        public Square(Square other) : 
            this(other.SquareType)
        {
            Tile = other.Tile != null ? new Tile(other.Tile.Letter) : null;
            MoveNumber = other.MoveNumber;
            IsFinal = other.IsFinal;
        }
        
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

        // Override Equals method
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var other = (Square)obj;

            // Compare Tile
            if (Tile != null && other.Tile != null)
            {
                if (!Tile.Equals(other.Tile))
                {
                    return false;
                }
            }
            else if (Tile != other.Tile) // One is null and the other is not
            {
                return false;
            }

            // Compare SquareType, MoveNumber, and IsFinal
            return SquareType == other.SquareType &&
                   MoveNumber == other.MoveNumber &&
                   IsFinal == other.IsFinal;
        }

        // Override GetHashCode method
        public override int GetHashCode()
        {
            var hashCode = new HashCode();
            hashCode.Add(SquareType);
            hashCode.Add(MoveNumber);
            hashCode.Add(IsFinal);
            if (Tile != null)
            {
                hashCode.Add(Tile);
            }
            return hashCode.ToHashCode();
        }
    }
}
