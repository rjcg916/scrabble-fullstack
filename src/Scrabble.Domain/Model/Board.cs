using System.Collections.Generic;

namespace Scrabble.Domain.Model
{

    public class Board
    {

        static readonly R lastRow = R._15;
        static readonly C lastCol = C.O;
        static readonly ushort rowCount = R._15 - R._1 + 1;
        static readonly ushort colCount = C.O - C.A + 1;

        readonly Square[,] board = new Square[rowCount, colCount];

        public static List<string> GetRowLabels()
        {
            return [
            "1", "2","3","4","5","6","7","8","9","10","11","12","13","14","15"
            ];
        }

        public static List<string> GetColLabels()
        {
            return [
            "A", "B","C","D","E","F","G","H","I","J","K","L","M","N","O"
            ];
        }

        public Board()
        {
            for (ushort r = 0; r <= (ushort)lastRow; r++)
                for (ushort c = 0; c <= (ushort)lastCol; c++)
                    board[r, c] = new Square();

            this.SetAllSquareTypes();
        }

        public List<CoordSquare> GetCoordSquares(bool filterForOccupied = false)
        {
            List<CoordSquare> squares = [];

            for (ushort r = 0; r <= (ushort)lastRow; r++)
                for (ushort c = 0; c <= (ushort)lastCol; c++)
                    if ((board[r, c].IsOccupied && filterForOccupied) || !filterForOccupied)
                        squares.Add(new CoordSquare(r, c, board[r, c]));

            return squares;
        }

        public Square GetSquare(Coord loc)
        {
            return board[(ushort)loc.Row, (ushort)loc.Col];
        }

        public Tile GetTile(Coord loc)
        {
            return GetSquare(loc).Tile;
        }

        public bool PlaceTile(Coord coord, Tile tile)
        {
            bool isSuccessful;

            var square = this.board[(ushort)coord.Row, (ushort)coord.Col];

            if (square.IsOccupied)
                isSuccessful = false;
            else
            {
                square.Tile = tile;
                isSuccessful = true;
            }

            return isSuccessful;
        }

        private void SetSquareTypes(SquareType t, Coord[] locs)
        {

            foreach (Coord loc in locs)
            {
                this.board[(ushort)loc.Row, (ushort)loc.Col].SquareType = t;
            }

        }

        private void SetAllSquareTypes()
        {

            // start
            this.board[(ushort)R._8, (ushort)C.H].SquareType = SquareType.start;

            // triple letters
            this.SetSquareTypes(SquareType.tl,

              [
                new(R._2, C.F),
                new(R._2, C.J),

                new(R._6, C.B),
                new(R._6, C.F),
                new(R._6, C.J),
                new(R._6, C.N),

                new(R._10, C.B),
                new(R._10, C.F),
                new(R._10, C.J),
                new(R._10, C.N),

                new(R._14, C.F),
                new(R._14, C.J)
              ]);

            // double letters
            this.SetSquareTypes(SquareType.dl,
                [
              new(R._1, C.D),
              new(R._1, C.L),

              new(R._3, C.G),
              new(R._3, C.I),

              new(R._4, C.A),
              new(R._4, C.H),
              new(R._4, C.O),

              new(R._7, C.C),
              new(R._7, C.G),
              new(R._7, C.I),
              new(R._7, C.M),

              new(R._8, C.D),
              new(R._8, C.L),

              new(R._9, C.C),
              new(R._9, C.G),
              new(R._9, C.I),
              new(R._9, C.M),

              new(R._12, C.A),
              new(R._12, C.H),
              new(R._12, C.O),

              new(R._13, C.G),
              new(R._13, C.I),

              new(R._15, C.D),
              new(R._15, C.L)

            ]);

            // double word
            this.SetSquareTypes(SquareType.dw,

              [
                new(R._2, C.B),
                new(R._2, C.N),

                new(R._3, C.C),
                new(R._3, C.M),

                new(R._4, C.D),
                new(R._4, C.L),

                new(R._5, C.E),
                new(R._5, C.K),

                new(R._11, C.E),
                new(R._11, C.K),

                new(R._12, C.D),
                new(R._12, C.L),

                new(R._13, C.C),
                new(R._13, C.M),

                new(R._14, C.B),
                new(R._14, C.N)

              ]);


            // triple word
            this.SetSquareTypes(
              SquareType.tw,

              [
                new(R._1, C.A),
                new(R._1, C.H),
                new(R._1, C.O),

                new(R._8, C.A),
                new(R._8, C.O),

                new(R._15, C.A),
                new(R._15, C.H),
                new(R._15, C.O)
              ]
            );

        }
    }
}