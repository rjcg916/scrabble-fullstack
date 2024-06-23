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

        public List<TilePlacement> GetTilePlacements()
        {
            return TilePlacements;
        }
    }
}