using System;
using System.Collections.Generic;
using System.Text;

namespace ScrabbleLib.Model
{

    public class Slot
    {
        public Tile tile { get; set; }
        public Slot(Tile tile = null)
        {
            this.tile = tile;
        }
    }
    public class Rack
    {
        public static byte capacity = 7;
        List<Tile> tiles;

        public byte TileCount
        {
            get
            {
                return (byte)this.tiles.Count;
            }
        }
        public Rack()
        {
            tiles = new List<Tile>();
        }

        public List<Tile> GetTiles()
        {
            return tiles;
        }

        public List<Tile> AddTiles(List<Tile> tiles)
        {
            var tilesNeeded = Rack.capacity - this.tiles.Count;
            var tilesAvailable = tiles.Count;
            var drawCount = tilesAvailable > tilesNeeded ? tilesNeeded : tilesAvailable;

            var tilesToAdd = tiles.GetRange(0, drawCount);

            this.tiles.AddRange(tilesToAdd);

            return GetTiles();
        }

        public List<Tile> RemoveTiles(List<Tile> tiles)
        {
            tiles.ForEach(r =>
           {
              var index = this.tiles.FindIndex( t => t.Letter == r.Letter);
               if (index > -1) 
                   this.tiles.RemoveAt(index);
           });

            return this.tiles;
        }

        public Slot[] GetSlots()
        {
            Slot[] slots = new Slot[Rack.capacity];

            for (int s = 0; s < Rack.capacity; s++)
                slots[s] = new Slot();

            byte i = 0;
            this.tiles.ForEach(t =>
           {
               slots[i++].tile = t;
           });

            return slots;
        }
    }
}