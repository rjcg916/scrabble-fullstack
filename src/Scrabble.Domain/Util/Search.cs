using System.Collections.Generic;

namespace Scrabble.Util
{
    public class Search
    {
        public static HashSet<(int, int)> Bfs((int, int) start, HashSet<(int, int)> locationSet)
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

            while (queue.Count != 0)
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
