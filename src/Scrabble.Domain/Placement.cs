using System;
using System.Collections.Generic;
using System.Linq;
using Scrabble.Util;
using static Scrabble.Domain.Move;

namespace Scrabble.Domain
{
    public record LocationSquare(Coord Coord, Square Square);
    public record PlacementError(Coord Location, string Letters);
    public record PlacementSpec(bool IsHorizontal, int SliceLocation, List<int> TileLocations);
    public record TilePlacement(Coord Coord, Tile Tile);

    public static class Placement
    {
        public static Move ToMove(this List<TilePlacement> placements) =>
            MoveFactory.CreateMove(placements);

        public static bool IsHorizontal(this List<TilePlacement> tileList) =>
            tileList.Select(c => c.Coord.RVal).Distinct().Count() == 1;
        public static bool IsVertical(this List<TilePlacement> tileList) =>
            tileList.Select(c => c.Coord.CVal).Distinct().Count() == 1;

        public static bool UniDirectionalMove(List<TilePlacement> tileList) =>
            tileList.Count == 1 || IsHorizontal(tileList) ^ IsVertical(tileList);

        public static bool HasWildcardTile(List<TilePlacement> tileList) =>
            tileList.Select(x => x.Tile.Value == 0).Any();

        public static PlacementSpec ToPlacementSpec(this List<TilePlacement> tileList)
        {
            var isHorizontal = IsHorizontal(tileList);
            var isVertical = IsVertical(tileList);

            if (!isHorizontal && !isVertical)
                throw new Exception("Invalid Move");

            var fixedLocation = tileList.Select(c => isHorizontal ? c.Coord.RVal : c.Coord.CVal).First();
            var tileLocations = tileList.Select(c => isHorizontal ? c.Coord.CVal : c.Coord.RVal).ToList();

            return new(isHorizontal, SliceLocation: fixedLocation, TileLocations: tileLocations);
        }

        public static bool IsContiguous(List<(int, int)> occupied, List<(int, int)> proposed)
        {
            var combined = occupied.Concat(proposed).ToList();
            var locationSet = new HashSet<(int, int)>(combined);

            if (locationSet.Count == 0)
                return true;  // No locations, trivially contiguous

            var start = combined[0];

            var visited = Search.Bfs(start, locationSet);

            return locationSet.SetEquals(visited);
        }

    }
}