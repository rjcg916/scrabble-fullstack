using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;

namespace Scrabble.Domain
{
    public class Move
    {
        public List<TilePlacement> TilePlacements { get; private set; }

        public Move(List<TilePlacement> tilePlacements)
        {
            TilePlacements = tilePlacements;
        }

        public Move(Coord startFrom, List<Tile> tiles, Placement placement)
        {
            TilePlacements = placement switch
            {
                Placement.Vertical  => tiles.Select((tile, index) =>
                                        new TilePlacement(new Coord((R)(startFrom.RVal + index), startFrom.Col), tile)
                                        ).ToList(),
                _                   => tiles.Select((tile, index) =>
                                        new TilePlacement(new Coord(startFrom.Row, (C)(startFrom.CVal + index)), tile)
                                        ).ToList(),
            };
        }

        // place tiles on board
        public void Apply(Board board)
        {
          //  var newBoard = new Board(board);
            board.MoveNumber++;
    
            foreach (var placement in TilePlacements)
            {
                int row = placement.Coord.RVal;
                int col = placement.Coord.CVal;
                board.squares[row,col].Tile = new Tile( placement.Tile.Letter);
                board.squares[row,col].MoveNumber = board.MoveNumber;
            }

           // return board;
        }
    }
}
