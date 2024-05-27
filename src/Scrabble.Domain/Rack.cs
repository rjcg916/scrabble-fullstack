using System;
using System.Collections.Generic;
using System.Linq;

namespace Scrabble.Domain
{
    public class Rack
    {
        public readonly static int Capacity = 7;
        readonly List<Tile> tiles;

        public int TileCount
        {
            get
            {
                return tiles.Count;
            }
        }

        public int SlotCount
        {
            get
            {
                return Capacity - tiles.Count;
            }
        }

        public Rack()
        {
            tiles = [];
        }

        public List<Tile> GetTiles() =>
            tiles;

        public bool InRack(char letter) =>
            tiles.Select(t => t.Letter).First() == letter;

        public List<Tile> AddTiles(List<Tile> tiles)
        {
            if (tiles.Count > Capacity - this.tiles.Count)
                throw new Exception("Attempt to add tiles beyond rack capacity");

            this.tiles.AddRange(tiles);

            return this.tiles;
        }

        public List<Tile> RemoveTiles(ushort count)
        {
            this.tiles.RemoveRange(0, count);
            return this.tiles;
        }
        
        public List<Tile> RemoveTiles(List<Tile> tiles)
        {

            if (tiles.Count > this.tiles.Count)
                throw new Exception("Attempt to remove more tiles than existing in rack.");

            tiles.ForEach(r =>
           {
               var index = this.tiles.FindIndex(t => t.Letter == r.Letter);
               if (index > -1)
                   this.tiles.RemoveAt(index);
           });

            return this.tiles;
        }
    }
}