using System;
using System.Collections.Generic;
using System.Linq;

namespace Scrabble.Domain
{
    public class Rack
    {
        public readonly static int Capacity = 7;
        List<Tile> Tiles { get; init; }

        public int TileCount => Tiles.Count;

        public int SlotCount => Capacity - Tiles.Count;

        public Rack()
        {
            Tiles = [];
        }

        public List<Tile> GetTiles() => new(Tiles);

        public bool InRack(char letter) => Tiles.Any(t => t.Letter == letter);

        public Rack AddTiles(List<Tile> newTiles)
        {
            var _ = new TileDrawCount(newTiles.Count);

            var updatedTiles = new List<Tile>(Tiles);
            updatedTiles.AddRange(newTiles);

            return new Rack { Tiles = updatedTiles };
        }

        public Rack RemoveTiles(int count)
        {
            if (count > TileCount)
                throw new Exception("Attempt to remove more tiles than existing in rack.");

            var updatedTiles = new List<Tile>(Tiles);
            updatedTiles.RemoveRange(0, count);

            return new Rack { Tiles = updatedTiles };
        }

        public Rack RemoveTiles(List<Tile> tilesToRemove)
        {
            var updatedTiles = new List<Tile>(Tiles);

           
            tilesToRemove.ForEach(tileToRemove =>
            {
                var index = updatedTiles.FindIndex(t => t.Letter == tileToRemove.Letter);
                if (index > -1)
                    updatedTiles.RemoveAt(index);
                else
                    throw new InvalidOperationException("Attempt to remove tile not in rack.");
            });

            return new Rack { Tiles = updatedTiles };
        }
    }
}
