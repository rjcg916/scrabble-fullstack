using System;
using System.Collections.Generic;
using System.Linq;

namespace Scrabble.Domain
{
    public record PlacementError(Coord Location, string Letters);
    public record PlacementSpec(bool IsHorizontal, int SliceLocation, List<int> TileLocations);
    public record TilePlacement(Coord Coord, Tile Tile);

    public static class PlacementSpecExtension
    {
        public static bool IsHorizontal(this List<TilePlacement> tileList) =>
            tileList.Select(c => c.Coord.RVal).Distinct().Count() == 1;
        public static bool IsVertical(this List<TilePlacement> tileList) =>
            tileList.Select(c => c.Coord.CVal).Distinct().Count() == 1;

        public static PlacementSpec ToPlacementSpec(this List<TilePlacement> tileList)
        {
            if (tileList.IsHorizontal())
            {
                var fixedLocation = tileList.Select(c => c.Coord.RVal).First();
                var tileLocations = tileList.Select(tl => tl.Coord.CVal).ToList();

                return new(IsHorizontal:true, fixedLocation, tileLocations);
            }
            else if (tileList.IsVertical())
            {
                var fixedLocation = tileList.Select(c => c.Coord.CVal).First();
                var tileLocations = tileList.Select(tl => tl.Coord.RVal).ToList();

                return new(IsHorizontal:false, fixedLocation, tileLocations);
            }
            else
            {
                throw new Exception("Invalid Move");
            }
        }
    }  
}