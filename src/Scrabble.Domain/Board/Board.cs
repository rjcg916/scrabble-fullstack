using System;
using System.Collections.Generic;
using static Scrabble.Domain.Move;

namespace Scrabble.Domain
{
    public partial class Board
    {
        public Square[,] squares = new Square[Coord.RowCount, Coord.ColCount];

        public static readonly Coord STAR = new(R._8, C.H);

        internal readonly Func<string, bool> IsWordValid;

        public int MoveNumber = 0;
        public int TileCount = 0;

        public Board(Func<string, bool> IsWordValid)
        {
            this.IsWordValid = IsWordValid;

            foreach (var r in Coord.Rows)
                foreach (var c in Coord.Cols)
                    squares[(int)r, (int)c] = new Square();

            Initialize();
        }

        public Board(Board other)
        {
            IsWordValid = other.IsWordValid;

            MoveNumber = other.MoveNumber;
            TileCount = other.TileCount;

            foreach (var r in Coord.Rows)
                foreach (var c in Coord.Cols)
                    squares[(int)r, (int)c] = new Square(other.squares[(int)r, (int)c]);
        }

        /// <summary>
        /// create a new Board with initial move
        /// </summary>>
        public Board(Func<string, bool> IsWordValid,
                     Move move) :
            this(IsWordValid)
        {
            MakeMove(move);
        }

        /// <summary>
        /// create a new Board with initial move
        /// </summary>
        public Board(Func<string, bool> IsWordValid,
                        Coord startFrom,
                        List<Tile> tiles,
                        bool isHorizontal) :
            this(IsWordValid)
        {
            this.MakeMove(MoveFactory.CreateMove(startFrom, tiles, isHorizontal));
        }

    }
}