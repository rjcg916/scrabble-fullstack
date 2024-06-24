using System.Collections.Generic;
using System.Linq;

namespace Scrabble.Domain
{
    public class Move
    {
        private static readonly int MAX_TILES_IN_MOVE = 7;

        public List<TilePlacement> TilePlacements { get; private set; }

        public static (bool valid, string msg) IsValidTilePlacement(List<TilePlacement> tilePlacements)
        {
            if (!AllowedNumberOfTiles(tilePlacements.Count))
                return (false, "Move includes too many tiles");

            if (!UniDirectionalMove(tilePlacements))
                return (false, "Move is in both horizontal and vertical direction");

            return (true, "Valid Move");
        }

        public static (bool valid, string msg) IsValidCoordTileList(Coord startFrom, List<Tile> tiles, bool isHorizontal)
        {
            var count = tiles.Count;

            if (!AllowedNumberOfTiles(count))
                    return (false, "Move contains more than number of allowed tiles.");
                
            if ((isHorizontal) && !OnBoardRight(startFrom.CVal, count))
                return (false, "Move off of board (Right)");
                            
            if ((!isHorizontal) && !OnBoardBottom(startFrom.RVal, count))
                return (false, "Move off of board (Bottom)");

            return (true, "Valid Move");
  
        }

        public static bool IsHorizontal(List<TilePlacement> tileList) =>
            tileList.Select(c => c.Coord.RVal).Distinct().Count() == 1;
        public static bool IsVertical(List<TilePlacement> tileList) =>
            tileList.Select(c => c.Coord.CVal).Distinct().Count() == 1;

        private static bool AllowedNumberOfTiles(int tileCount) =>
            tileCount <= MAX_TILES_IN_MOVE;
        private static bool UniDirectionalMove(List<TilePlacement> tilePlacements) =>
              (IsHorizontal(tilePlacements)  && !IsVertical(tilePlacements))  ||
              (!IsHorizontal(tilePlacements) && IsVertical(tilePlacements))   ||
              (tilePlacements.Count == 1);
        private static bool OnBoardRight(int startingLocation, int numberOfTiles) =>
            (startingLocation + numberOfTiles) <= (int)C.O;

        private static bool OnBoardBottom(int startingLocation, int numberOfTiles) =>
            (startingLocation + numberOfTiles) <= (int)R._15;
    
        public Move(List<TilePlacement> tilePlacements)
        {
            var (valid, msg) = IsValidTilePlacement(tilePlacements);
            if (!valid)
                throw new System.Exception(msg);

            TilePlacements = tilePlacements;
        }

        public Move(Coord startFrom, List<Tile> tiles, bool isHorizontal)
        {
            var (valid, msg) = IsValidCoordTileList(startFrom, tiles, isHorizontal);
            if (!valid)
                throw new System.Exception(msg);

            TilePlacements = isHorizontal
                    ? tiles.Select((tile, index) =>
                        new TilePlacement(new Coord(startFrom.Row, (C)(startFrom.CVal + index)), tile))
                         .ToList()
                    : tiles.Select((tile, index) =>
                        new TilePlacement(new Coord((R)(startFrom.RVal + index), startFrom.Col), tile))
                            .ToList();                     
        }

        public List<TilePlacement> GetTilePlacements() =>
            TilePlacements;
        
    }
}