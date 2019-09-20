using System;
using System.Collections.Generic;
using System.Text;

namespace ScrabbleLib.Model
{
    public enum SquareType
    {
        reg, dl, tl, dw, tw, start
    }


    public class Square
    {

        public SquareType SquareType { get; set; }

        public Square(SquareType squareType = SquareType.reg)
        {
            SquareType = squareType;
        }

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
    public class CoordSquare
    {

        public Square Square { get; set; }
        public int Row { get; set; }
        public int Col { get; set; }

        public CoordSquare(int row, int col, Square square = null)
        {
            this.Row = row;
            this.Col = col;
            this.Square = square;
        }
    }
}
