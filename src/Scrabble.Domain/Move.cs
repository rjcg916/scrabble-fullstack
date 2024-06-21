using System.Collections.Generic;
using System.Linq;

namespace Scrabble.Domain
{
    public class Move
    {
        public List<TilePlacement> TilePlacements { get; private set; }

        public Move(List<TilePlacement> tilePlacements)
        {
            TilePlacements = tilePlacements;
        }

        public Move(Coord startFrom, List<Tile> tiles, bool isHorizontal) =>
            TilePlacements = isHorizontal ? 
                tiles.Select((tile, index) =>
                    new TilePlacement(new Coord((R)(startFrom.RVal + index), startFrom.Col), tile))
                          .ToList() :
                tiles.Select((tile, index) =>
                    new TilePlacement(new Coord(startFrom.Row, (C)(startFrom.CVal + index)), tile))
                         .ToList();        

        // place tiles on board
        public Board Apply(Board board)
        {
            board.MoveNumber++;
    
            foreach (var placement in TilePlacements)
            {
                int row = placement.Coord.RVal;
                int col = placement.Coord.CVal;
                board.squares[row,col].Tile = new Tile( placement.Tile.Letter);
                board.squares[row,col].MoveNumber = board.MoveNumber;
            }

            return board;
        }
    }
}