using System;
using System.Collections.Generic;

namespace Scrabble.Domain.Model
{

    public class Slot(Tile tile = null)
    {
        public Tile tile { get; set; } = tile;
    }
    public class Rack
    {
        public static byte capacity = 7;
        readonly List<Tile> tiles;

        public byte TileCount
        {
            get
            {
                return (byte)this.tiles.Count;
            }
        }
        public Rack()
        {
            tiles = [];
        }

        public List<Tile> GetTiles()
        {
            return tiles;
        }


        public List<Tile> AddTiles(List<Tile> tiles)
        {
            if (tiles.Count > (Rack.capacity - this.tiles.Count))
                throw new Exception("Attempt to add tiles beyond rack capacity");

            this.tiles.AddRange(tiles);

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