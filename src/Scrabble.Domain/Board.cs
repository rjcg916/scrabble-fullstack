using System;
using System.Collections.Generic;

namespace Scrabble.Domain
{
    public class EvaluatorAt<T>(ushort row, ushort col, T evaluator = default)
    {
        public T Evaluator { get; set; } = evaluator;
        public ushort Row { get; set; } = row;
        public string RowName { get; set; } = ((R)row).ToString()[1..];
        public ushort Col { get; set; } = col;
        public string ColName { get; set; } = ((C)col).ToString()[0..];
    }

    public static class BoardHelper
    {
        public static IEnumerable<R> GetRows()
        {
            foreach (R row in Enum.GetValues(typeof(R)))
            {
                yield return row;
            }
        }

        public static IEnumerable<ushort> GetRowsAsUshort()
        {
            foreach (R row in Enum.GetValues(typeof(R)))
            {
                yield return (ushort) row;
            }
        }

        public static IEnumerable<C> GetColumns()
        {
            foreach (C col in Enum.GetValues(typeof(C)))
            {
                yield return col;
            }
            
        }

        public static IEnumerable<ushort> GetColumnsAsUshort()
        {
            foreach (C col in Enum.GetValues(typeof(C)))
            {
                yield return (ushort)col;
            }

        }

        public static string GetRowName(R row)
        {
            return row.ToString().Replace("_", "");
        }

        public static string GetColumnName(C col)
        {
            return col.ToString();
        }
    }


    public class Board
    {
      //  static readonly R firstRow = R._1;
      //  static readonly R lastRow = R._15;
      //  static readonly C firstCol = C.A;
      //  static readonly C lastCol = C.O;
        static readonly ushort rowCount = R._15 - R._1 + 1;
        static readonly ushort colCount = C.O - C.A + 1;

        readonly Square[,] board = new Square[rowCount, colCount];

        public Board()
        {
            //for (ushort r = (ushort)firstRow; r <= (ushort)lastRow; r++)
            //    for (ushort c = (ushort)firstCol; c <= (ushort)lastCol; c++)
            //        board[r, c] = new Square();

            foreach (var r in BoardHelper.GetRowsAsUshort())
                foreach (var c in BoardHelper.GetColumnsAsUshort())
                    board[r, c] = new Square();

            SetAllSquareTypes();
        }

        public Square[] GetHorizontalSlice(ushort row)
        {
            // Implementation for getting horizontal slice.
            throw new NotImplementedException();
        }

        public Square[] GetVerticalSlice(ushort col)
        {
            // Implementation for getting vertical slice.
            throw new NotImplementedException();
        }

        public List<EvaluatorAt<Square>> GetCoordSquares(bool filterForOccupied = false)
        {
            List<EvaluatorAt<Square>> squares = [];

            foreach (var r in BoardHelper.GetRowsAsUshort())
                foreach (var c in BoardHelper.GetColumnsAsUshort())
                    if (board[r, c].IsOccupied && filterForOccupied || !filterForOccupied)
                        squares.Add(new EvaluatorAt<Square>(r, c, board[r, c]));

            return squares;
        }

        public Square GetSquare(Coord loc) =>
            board[(ushort)loc.Row, (ushort)loc.Col];

        public Tile GetTile(Coord loc) =>
            GetSquare(loc).Tile;

        public bool IsOccupied(Coord coord) =>
            board[(ushort)coord.Row, (ushort)coord.Col].IsOccupied;

        public bool PlaceTile(Coord coord, Tile tile)
        {
            bool isSuccessful;

            var square = board[(ushort)coord.Row, (ushort)coord.Col];

            if (IsOccupied(coord))
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
                board[(ushort)loc.Row, (ushort)loc.Col].SquareType = t;
            }
        }

        private void SetAllSquareTypes()
        {

            // start
            board[(ushort)R._8, (ushort)C.H].SquareType = SquareType.start;

            // triple letters
            SetSquareTypes(SquareType.tl,

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
            SetSquareTypes(SquareType.dl,
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
            SetSquareTypes(SquareType.dw,

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
            SetSquareTypes(
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