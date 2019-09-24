using System;
using System.Collections.Generic;
using System.Text;

namespace ScrabbleLib.Model
{
 

    public class Board
    {

        static R lastRow = R._15;
        static C lastCol = C._O;
        static int rowCount = (int)R._15 - (int)R._1 + 1;
        static int colCount = (int)C._O - (int)C._A + 1;

        Square[,] board = new Square[rowCount, colCount];

        public Board()
        {
            for (int r = 0; r <= (int)lastRow; r++)
                for (int c = 0; c <= (int)lastCol; c++)
                    board[r, c] = new Square();

            this.SetAllSquareTypes();
        }

        public List<CoordSquare> GetCoordSquares( bool filterForOccupied = false)
        {
            List<CoordSquare> squares = new List<CoordSquare>();

            for (int r = 0; r <= (int)lastRow; r++)
                for (int c = 0; c <= (int)lastCol; c++)
                    if ((board[r, c].IsOccupied && filterForOccupied) || !filterForOccupied )
                        squares.Add(new CoordSquare(r, c, board[r, c]));

            return squares;
        }
   
        public Square GetSquare(Coord loc)
        {
            return board[(int)loc.row, (int)loc.col];
        }

        public Tile GetTile(Coord loc)
        {
            return GetSquare(loc).Tile;
        }

        public bool PlaceTile(TileLocation tileLocation)
        {
            bool isSuccessful;

            var square = this.board[(int) tileLocation.coord.row, (int)tileLocation.coord.col];

            if (square.IsOccupied)
                isSuccessful = false;
            else
            {
                square.Tile = tileLocation.tile;
                isSuccessful = true;
            }

            return isSuccessful;
        }

        private void SetSquareTypes(SquareType t, Coord[] locs)
        {

            foreach (Coord loc in locs)
            {
                this.board[(int)loc.row, (int)loc.col].SquareType = t;
            }

        }

        private void SetAllSquareTypes()
        {

            // start
            this.board[(int)R._8, (int)C._H].SquareType = SquareType.start;

            // triple letters
            this.SetSquareTypes(SquareType.tl, new Coord[]

              {
                new Coord(R._2, C._F),
                new Coord(R._2, C._J),

                new Coord(R._6, C._B),
                new Coord(R._6, C._F),
                new Coord(R._6, C._J),
                new Coord(R._6, C._N),

                new Coord(R._10, C._B),
                new Coord(R._10, C._F),
                new Coord(R._10, C._J),
                new Coord(R._10, C._N),

                new Coord(R._14, C._F),
                new Coord(R._14, C._J)
              });

            // double letters
            this.SetSquareTypes(SquareType.dl, new Coord[]
                {
              new Coord(R._1, C._D),
              new Coord(R._1, C._L),

              new Coord(R._3, C._G),
              new Coord(R._3, C._I),

              new Coord(R._4, C._A),
              new Coord(R._4, C._H),
              new Coord(R._4, C._O),

              new Coord(R._7, C._C),
              new Coord(R._7, C._G),
              new Coord(R._7, C._I),
              new Coord(R._7, C._M),

              new Coord(R._8, C._D),
              new Coord(R._8, C._L),

              new Coord(R._9, C._C),
              new Coord(R._9, C._G),
              new Coord(R._9, C._I),
              new Coord(R._9, C._M),

              new Coord(R._12, C._A),
              new Coord(R._12, C._H),
              new Coord(R._12, C._O),

              new Coord(R._13, C._G),
              new Coord(R._13, C._I),

              new Coord(R._15, C._D),
              new Coord(R._15, C._L)

            });

            // double word
            this.SetSquareTypes(SquareType.dw, new Coord[]

              {
                new Coord(R._2, C._B),
                new Coord(R._2, C._N),

                new Coord(R._3, C._C),
                new Coord(R._3, C._M),

                new Coord(R._4, C._D),
                new Coord(R._4, C._L),

                new Coord(R._5, C._E),
                new Coord(R._5, C._K),

                new Coord(R._11, C._E),
                new Coord(R._11, C._K),

                new Coord(R._12, C._D),
                new Coord(R._12, C._L),

                new Coord(R._13, C._C),
                new Coord(R._13, C._M),

                new Coord(R._14, C._B),
                new Coord(R._14, C._N)

              });


            // triple word
            this.SetSquareTypes(
              SquareType.tw, new Coord[]

              {
                new Coord(R._1, C._A),
                new Coord(R._1, C._H),
                new Coord(R._1, C._O),

                new Coord(R._8, C._A),
                new Coord(R._8, C._O),

                new Coord(R._15, C._A),
                new Coord(R._15, C._H),
                new Coord(R._15, C._O)
              }
            );

        }


    }
}
