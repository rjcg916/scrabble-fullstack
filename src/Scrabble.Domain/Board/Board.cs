using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using static Scrabble.Domain.Move;

namespace Scrabble.Domain
{
    public partial class Board { 

        [JsonInclude]
        public Square[,] squares = new Square[Coord.RowCount, Coord.ColCount];

        public static readonly Coord STAR = new(R._8, C.H);

        internal Func<string, bool> IsWordValid;

        public int MoveNumber { get; set; } = 0;
        public int TileCount { get; set;  } = 0;

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
        /// Create a new Board with initial move
        /// </summary>>
        public Board(Func<string, bool> IsWordValid,
                     Move move) :
            this(IsWordValid)
        {
            MakeMove(move);
        }

        /// <summary>
        /// Create a new Board with initial move
        /// </summary>
        public Board(Func<string, bool> IsWordValid,
                        Coord startFrom,
                        List<Tile> tiles,
                        bool isHorizontal) :
            this(IsWordValid)
        {
            this.MakeMove(MoveFactory.CreateMove(startFrom, tiles, isHorizontal));
        }

        public Board()
        {
        }

        public Board MakeMove(Move move)
        {
            if (Placement.HasWildcardTile(move.TilePlacements))
                _ = new SystemException("Cannot make Move with wildcard tiles.");

            this.MoveNumber++;

            foreach (var placement in move.TilePlacements)
            {
                int row = placement.Coord.RVal;
                int col = placement.Coord.CVal;
                squares[row, col].Tile = new Tile(placement.Tile.Letter);
                squares[row, col].MoveNumber = MoveNumber;
                TileCount++;
            }
            return this;
        }

        public int ScoreMove(Move move)
        {
            // put tiles on a board and compute score
            var scoreBoard = new Board(this);
            scoreBoard.MakeMove(move);
            return new Score(scoreBoard, move).Calculate();
        }
    }
}