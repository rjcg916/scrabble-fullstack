using System;
using System.Collections.Generic;
using System.Linq;

namespace Scrabble.Domain
{
    public record PlacementError(Coord Location, string Letters);
    public record PlacementSpec(bool IsHorizontal, int SliceLocation, List<int> TileLocations);
    public record TilePlacement(Coord Coord, Tile Tile);

    public static class PlacementExtension
    {
        public static Move ToMove(this List<TilePlacement> placements) =>
            MoveFactory.CreateMove(placements);
                
        public static bool IsHorizontal(this List<TilePlacement> tileList) =>
            tileList.Select(c => c.Coord.RVal).Distinct().Count() == 1;
        public static bool IsVertical(this List<TilePlacement> tileList) =>
            tileList.Select(c => c.Coord.CVal).Distinct().Count() == 1;

        public static PlacementSpec ToPlacementSpec(this List<TilePlacement> tileList)
        {
            var isHorizontal = IsHorizontal(tileList);
            var isVertical   = IsVertical(tileList);
            
            if (!isHorizontal && !isVertical) 
                throw new Exception("Invalid Move");

            var fixedLocation = tileList.Select(c => isHorizontal ? c.Coord.RVal : c.Coord.CVal).First();
            var tileLocations = tileList.Select( c => isHorizontal ? c.Coord.CVal : c.Coord.RVal).ToList();
            
            return new(isHorizontal, SliceLocation:fixedLocation, TileLocations: tileLocations);

        }
    }  
}