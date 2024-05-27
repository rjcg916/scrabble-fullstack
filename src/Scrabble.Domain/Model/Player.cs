using System;
using System.Collections.Generic;

namespace Scrabble.Domain.Model
{
    public class Player
    {
        public Rack Rack { get; set; } = new Rack();
        public string Name { get; set; }

        public Player(string name, TileBag tileBag) { 
            Name = name;
            DrawTiles(tileBag);
        }

        public List<Tile> DrawTiles(TileBag tileBag)
        {
            var tilesAvailable = tileBag.Count;

            if (tilesAvailable == 0)
                throw new Exception("No tiles available to draw.");

            var tilesNeeded = Rack.Capacity - this.Rack.TileCount;

            var drawCount = tilesAvailable > tilesNeeded ? tilesNeeded : tilesAvailable;

            var tilesToAdd = tileBag.DrawTiles(drawCount);
            this.Rack.AddTiles(tilesToAdd);

            return tilesToAdd;
        }

        public static bool PlaceTile(Board board, Coord coord, Tile tile)
        {
            return board.PlaceTile(coord, tile);
        }
    }
}
