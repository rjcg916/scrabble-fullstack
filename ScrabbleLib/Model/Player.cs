using System;
using System.Collections.Generic;
using System.Text;

namespace ScrabbleLib.Model
{
    public class Player
    {
        Rack rack { get; set; }
        String name { get; set; }

        public Player(String name)
        {
            this.name = name;
            this.rack = new Rack();
        }

        public List<Tile> DrawTiles(List<Tile> tiles)
        {
            return this.rack.AddTiles(tiles);
        }
    } 
}

