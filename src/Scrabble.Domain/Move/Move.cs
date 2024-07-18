using System;
using System.Collections.Generic;
using System.Linq;

namespace Scrabble.Domain
{
    public abstract class Move
    {
        public List<TilePlacement> TilePlacements { get; init; }

        public string Letters =>
            TilePlacements.Select(tp => tp.Tile).ToList().TilesToLetters();

        public static class MoveFactory
        {
            public static Move CreateMove(List<TilePlacement> tilePlacements)
            {
                if (tilePlacements.Count == 0)
                    throw new Exception("Invalid Move specified.");

                if (!Move.UniDirectionalMove(tilePlacements))
                    throw new Exception("Move cannot be in both horizontal and vertical direction.");

                return Move.IsHorizontal(tilePlacements) ?
                    new MoveHorizontal(tilePlacements) :
                    new MoveVertical(tilePlacements);
            }
            public static Move CreateMove(Coord startFrom, List<Tile> tiles, bool isHorizontal) =>
                isHorizontal    ? new MoveHorizontal(startFrom, tiles) 
                                : new MoveVertical(startFrom, tiles);
        }

        protected Move(List<TilePlacement> tilePlacements)
        {
            var (valid, msg) = IsValidTilePlacement(tilePlacements);
            if (!valid)
                throw new Exception(msg);

            TilePlacements = tilePlacements;
        }
        
        public static (bool valid, string msg) IsValidTilePlacement(List<TilePlacement> tilePlacements)
        {
            if (tilePlacements.Count == 0)
                return (false, "Invalid Move specified.");

            if (!AllowedNumberOfTiles(tilePlacements.Count))
                return (false, "Move includes too many tiles.");

            if (!UniDirectionalMove(tilePlacements))
                return (false, "Move cannot be in both horizontal and vertical direction.");

            return (true, "Valid Move");
        }

        public virtual (bool valid, string msg) IsValid(List<TilePlacement> tilePlacements) =>
            Move.IsValidTilePlacement(tilePlacements);

        protected static bool AllowedNumberOfTiles(int tileCount) =>
            tileCount <= Rack.Capacity;

        public static bool UniDirectionalMove(List<TilePlacement> tilePlacements) =>
            tilePlacements.Count == 1 || IsHorizontal(tilePlacements) ^ IsVertical(tilePlacements);

        public static bool IsHorizontal(List<TilePlacement> tileList) =>
            tileList.All(c => c.Coord.RVal == tileList[0].Coord.RVal);

        public static bool IsVertical(List<TilePlacement> tileList) =>
            tileList.All(c => c.Coord.CVal == tileList[0].Coord.CVal);

        public static bool HasWildcardTile(List<TilePlacement> tileList) =>
            tileList.Select(x => x.Tile.Value == 0).Any();

    }

}