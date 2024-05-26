using System;
using System.Collections.Generic;

namespace Scrabble.Domain.Model
{
    public class Player(string name)
    {
        public Rack rack { get; set; } = new Rack();
        public string Name { get; set; } = name;

        public List<Tile> DrawTiles(TileBag tileBag)
        {
            var tilesAvailable = tileBag.count;

            if (tilesAvailable == 0)
                throw new Exception("No tiles available to draw.");

            var tilesNeeded = Rack.capacity - this.rack.TileCount;

            var drawCount = tilesAvailable > tilesNeeded ? tilesNeeded : tilesAvailable;

            var tilesToAdd = tileBag.DrawTiles(drawCount);
            this.rack.AddTiles(tilesToAdd);

            return tilesToAdd;
        }

        public static bool PlaceTile(Board board, Coord coord, Tile tile)
        {
            return board.PlaceTile(coord, tile);
        }
    }
}
