using System;

namespace Scrabble.Domain
{
    public partial class Board
    {
        public Board MakeMove(Move move)
        {
            if (Move.HasWildcardTile(move.TilePlacements))
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
