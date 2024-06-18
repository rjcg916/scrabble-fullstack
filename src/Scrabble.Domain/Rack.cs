using System;
using System.Collections.Generic;
using System.Linq;

namespace Scrabble.Domain
{
    public class Rack
    {
        public static readonly int Capacity = 7;
        public IReadOnlyList<Tile> Tiles { get; init; }

        public int TileCount => Tiles.Count;

        public int SlotCount => Capacity - Tiles.Count;

        public Rack()
        {
            Tiles = Array.Empty<Tile>();
        }

        public Rack(IEnumerable<Tile> newTiles)
        {
            Tiles = new List<Tile>(newTiles).AsReadOnly();
        }

        public List<Tile> GetTiles() => new(Tiles);

        public bool InRack(char letter) => Tiles.Any(t => t.Letter == letter);

        public Rack AddTiles(IEnumerable<Tile> newTiles)
        {
            if (TileCount + newTiles.Count() > Capacity)
                throw new InvalidOperationException("Exceeding rack capacity");

            var updatedTiles = Tiles.Concat(newTiles).ToList();
            return new Rack(updatedTiles);
        }

        public Rack RemoveTiles(int count)
        {
            if (count > TileCount)
                throw new InvalidOperationException("Attempt to remove more tiles than existing in rack.");

            var updatedTiles = Tiles.Take(TileCount - count).ToList();
            return new Rack(updatedTiles);
        }

        public Rack RemoveTiles(IEnumerable<Tile> tilesToRemove)
        {
            var updatedTiles = Tiles.ToList();

            foreach (var tileToRemove in tilesToRemove)
            {
                var index = updatedTiles.FindIndex(t => t.Letter == tileToRemove.Letter);
                if (index > -1)
                    updatedTiles.RemoveAt(index);
                else
                    throw new InvalidOperationException("Attempt to remove tile not in rack.");
            }

            return new Rack(updatedTiles);
        }
    }
}
