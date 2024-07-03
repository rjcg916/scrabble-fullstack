using System;
using System.Collections.Generic;
using System.Linq;
using static Scrabble.Domain.Move;

namespace Scrabble.Domain
{
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

            if (!locationSet.Any())
                return true;  // No locations, trivially contiguous

            var start = combined[0];

            var visited = Bfs(start, locationSet);

            return locationSet.SetEquals(visited);
        }

        private static HashSet<(int, int)> Bfs((int, int) start, HashSet<(int, int)> locationSet)
        {
            var directions = new (int, int)[]
            {
            (0, 1), // Right
            (1, 0), // Down
            (0, -1), // Left
            (-1, 0) // Up
            };

            var queue = new Queue<(int, int)>();
            var visited = new HashSet<(int, int)>();

            queue.Enqueue(start);

            while (queue.Any())
            {
                var current = queue.Dequeue();

                if (visited.Contains(current))
                    continue;

                visited.Add(current);

                foreach (var direction in directions)
                {
                    var neighbor = (current.Item1 + direction.Item1, current.Item2 + direction.Item2);
                    if (locationSet.Contains(neighbor) && !visited.Contains(neighbor))
                    {
                        queue.Enqueue(neighbor);
                    }
                }
            }

            return visited;
        }
    }
}