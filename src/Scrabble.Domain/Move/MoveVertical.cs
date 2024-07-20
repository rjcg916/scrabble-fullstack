using System;
using System.Collections.Generic;
using System.Linq;

namespace Scrabble.Domain
{
    public class MoveVertical : Move
    {
        internal MoveVertical(List<TilePlacement> tilePlacements) : base(tilePlacements)
        {
        }

        internal MoveVertical(Coord startFrom, List<Tile> tiles)
            : base(tiles.Select((tile, index) =>
                    new TilePlacement(new Coord((R)(startFrom.RVal + index), startFrom.Col), tile)).ToList())
        {
            var (valid, msg) = IsValidCoordTileList(startFrom, tiles);
            if (!valid)
                throw new Exception(msg);
        }

        internal static (bool valid, string msg) IsValidCoordTileList(Coord startFrom, List<Tile> tiles)
        {
            var count = tiles.Count;

            if (!AllowedNumberOfTiles(count))
                return (false, "Move includes too many tiles.");

            if ((startFrom.RVal + count) > (int)R._15)
                return (false, "Move off of board (Bottom)");

            return (true, "Valid Move");
        }

        public override (bool valid, string msg) IsValid(List<TilePlacement> tilePlacements)
        {
            if (!Placement.IsVertical(tilePlacements))
                return (false, "Move must be but is not vertical");

            var (baseValid, baseMsg) = base.IsValid(tilePlacements);
            if (!baseValid)
                return (baseValid, baseMsg);

            return (true, "Valid Move");
        }
    }
}