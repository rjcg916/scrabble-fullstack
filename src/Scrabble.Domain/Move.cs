using System.Collections.Generic;
using System.Linq;

namespace Scrabble.Domain
{
    public class Move
    {
        public List<TilePlacement> TilePlacements { get; private set; }
        public int MoveNumber { get; private set; }

        public Move(int moveNumber, List<TilePlacement> tilePlacements)
        {
            MoveNumber = moveNumber;
            TilePlacements = tilePlacements;
        }

        public Move(int moveNumber, Coord startFrom, List<Tile> tiles, Placement placement)
        {
            MoveNumber = moveNumber;

            TilePlacements = placement switch
            {
                Placement.Vertical => tiles.Select((tile, index) =>
                                        new TilePlacement(new Coord((R)(startFrom.RVal + index), startFrom.Col), tile)
                                        ).ToList(),
                _ => tiles.Select((tile, index) =>
                                        new TilePlacement(new Coord(startFrom.Row, (C)(startFrom.CVal + index)), tile)
                                        ).ToList(),
            };
        }

        // place tiles on board
        public Board Apply(Board board)
        {
            Board newBoard = new(board);
            newBoard.MoveNumber++;

            foreach (var placement in TilePlacements)
            {
                var loc = newBoard.squares[placement.Coord.RVal, placement.Coord.CVal];
                loc.Tile = placement.Tile;
                loc.MoveOfOccupation = newBoard.MoveNumber;
            }

            return newBoard;
        }
    }
}
