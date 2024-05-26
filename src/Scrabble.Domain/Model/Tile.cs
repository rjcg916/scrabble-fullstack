using System;

namespace Scrabble.Domain.Model
{
    public class Tile(String letter, int Value = 1)
    {
        public string Letter { get; } = letter.ToUpper();
        public int Value { get; } = Value;
    }

    // public class TileLocation
    // {
    //     public Coord coord { get; set; }
    //     public Tile tile { get; set; }
    //     public TileLocation(Coord coord, Tile tile)
    //     {
    //         this.coord = coord;
    //         this.tile = tile;
    //     }
    // }
}




