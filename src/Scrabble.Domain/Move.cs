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
                return (false, "Move not defined.");

            if (!AllowedNumberOfTiles(tilePlacements.Count))
                return (false, "Move includes too many tiles");

            if (!UniDirectionalMove(tilePlacements))
                return (false, "Move is in both horizontal and vertical direction");

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

        public static class MoveFactory
        {
            public static Move CreateMove(List<TilePlacement> tilePlacements)
            {
                if (Move.UniDirectionalMove(tilePlacements))
                {
                    return Move.IsHorizontal(tilePlacements) ?
                        new MoveHorizontal(tilePlacements) :
                        new MoveVertical(tilePlacements);                
                }
                else
                {
                    throw new Exception("Move is in both horizontal and vertical direction");
                }
            }

            public static Move CreateMove(Coord startFrom, List<Tile> tiles, bool isHorizontal) =>
                isHorizontal ? new MoveHorizontal(startFrom, tiles) : new MoveVertical(startFrom, tiles);
            
        }
    }

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

            if (!IsHorizontal(tilePlacements))
                return (false, "Move is not horizontal");

            var (baseValid, baseMsg) = base.IsValid(tilePlacements);
            if (!baseValid)
                return (baseValid, baseMsg);

            return (true, "Valid Move");
        }
    }
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
                return (false, "Move contains more than number of allowed tiles.");

            if ((startFrom.RVal + count) > (int)R._15)
                return (false, "Move off of board (Bottom)");

            return (true, "Valid Move");
        }

        public override (bool valid, string msg) IsValid(List<TilePlacement> tilePlacements)
        {
            if (!IsVertical(tilePlacements))
                return (false, "Move is not vertical");

            var (baseValid, baseMsg) = base.IsValid(tilePlacements);
            if (!baseValid)
                return (baseValid, baseMsg);

            return (true, "Valid Move");
        }
    }
}