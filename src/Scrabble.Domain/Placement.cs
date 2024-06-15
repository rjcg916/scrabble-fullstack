using System;
using System.Collections.Generic;
using System.Linq;

namespace Scrabble.Domain
{
    public enum Placement
    {
        Horizontal, Vertical, Star, All
    }

    public record PlacementError(Placement Type, Coord Location, string Letters);
    public record PlacementSpec(Placement Placement, int SliceLocation, List<int> TileLocations);
    public record TilePlacement(Coord Coord, Tile Tile);

    public static class PlacementSpecExtension
    {
        public static PlacementSpec ToPlacementSpec(this List<TilePlacement> tileList)
        {
            if (tileList.Select(c => c.Coord.RowValue).Distinct().Count() == 1)
            {
                var fixedLocation = tileList.Select(c => c.Coord.RowValue).First();
                var tileLocations = tileList.Select(tl => tl.Coord.ColValue).ToList();

                return new(Placement.Horizontal, fixedLocation, tileLocations);
            }
            else if (tileList.Select(c => c.Coord.ColValue).Distinct().Count() == 1)
            {
                var fixedLocation = tileList.Select(c => c.Coord.ColValue).First();
                var tileLocations = tileList.Select(tl => tl.Coord.RowValue).ToList();

                return new(Placement.Vertical, fixedLocation, tileLocations);
            }
            else
            {
                throw new Exception("Invalid Move");
            }
        }
    }  
}