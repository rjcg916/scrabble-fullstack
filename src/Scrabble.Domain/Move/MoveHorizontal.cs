using System;
using System.Collections.Generic;
using System.Linq;

namespace Scrabble.Domain
{

    public class MoveHorizontal : Move
    {
        internal MoveHorizontal(List<TilePlacement> tilePlacements) : base(tilePlacements)
        {
        }

        internal MoveHorizontal(Coord startFrom, List<Tile> tiles)
            : base(tiles.Select((tile, index) =>
                new TilePlacement(new Coord(startFrom.Row, (C)(startFrom.CVal + index)), tile)).ToList())
        {
            var (valid, msg) = IsValidCoordTileList(startFrom, tiles);
            if (!valid)
                throw new Exception(msg);
        }

        internal static (bool valid, string msg) IsValidCoordTileList(Coord startFrom, List<Tile> tiles)
        {
            var count = tiles.Count;

            if (!AllowedNumberOfTiles(count))
                return (false, "Move contains more than number of allowed tiles.");

            if ((startFrom.CVal + count) > (int)C.O)
                return (false, "Move off of board (Right)");

            return (true, "Valid Move");
        }

        public override (bool valid, string msg) IsValid(List<TilePlacement> tilePlacements)
        {
            if (!Placement.IsHorizontal(tilePlacements))
                return (false, "Move must be but is not horizontal");

            var (baseValid, baseMsg) = base.IsValid(tilePlacements);
            if (!baseValid)
                return (baseValid, baseMsg);

            return (true, "Valid Move");
        }
    }

}