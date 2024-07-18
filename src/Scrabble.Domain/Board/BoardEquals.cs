using System;

namespace Scrabble.Domain
{
    public partial class Board
    {
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var other = (Board)obj;

            if (!IsWordValid.Method.Equals(other.IsWordValid.Method))
            {
                return false;
            }

            if (MoveNumber != other.MoveNumber)
            {
                return false;
            }

            for (int r = 0; r < Coord.RowCount; r++)
            {
                for (int c = 0; c < Coord.ColCount; c++)
                {
                    if (!squares[r, c].Equals(other.squares[r, c]))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public override int GetHashCode()
        {
            var hashCode = new HashCode();

            // Combine hash codes of relevant properties
            hashCode.Add(IsWordValid.Method);
            hashCode.Add(MoveNumber);

            for (int r = 0; r < Coord.RowCount; r++)
            {
                for (int c = 0; c < Coord.ColCount; c++)
                {
                    hashCode.Add(squares[r, c]);
                }
            }

            return hashCode.ToHashCode();
        }

    }
}
