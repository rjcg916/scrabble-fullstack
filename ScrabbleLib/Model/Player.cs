using System;
using System.Collections.Generic;
using System.Text;

namespace ScrabbleLib.Model
{
    public class Player
    {
        public Rack rack { get; set; } = new Rack();

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

        public bool PlaceTile(Board board, TileLocation tileLocation)
        {
            return board.PlaceTile(tileLocation);
        }
    }
}
