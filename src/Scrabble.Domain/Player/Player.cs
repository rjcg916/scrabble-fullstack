using System;

namespace Scrabble.Domain
{
    public class Player
    {
        public Rack Rack { get; set; } = new Rack();
        public string Name { get; set; } 
        public int Score { get; set; } = 0;

        public Player(string name) {   
            Name = name;
        }

        public Player( Player player) 
        {
            Rack = new Rack( player.Rack);
            Name = player.Name;
            Score = player.Score;
        }

        public TileBag DrawTiles(TileBag tileBag)
        {
            var tilesAvailable = tileBag.Count;

            if (tilesAvailable == 0)
                throw new Exception("No tiles available to draw.");

            var tilesNeeded = Rack.Capacity - Rack.TileCount;

            var drawCount = tilesAvailable > tilesNeeded ? tilesNeeded : tilesAvailable;

            var (tilesToAddToRack, tileBagAfterRemoval) = tileBag.DrawTiles(new TileDrawCount(drawCount));
            this.Rack = this.Rack.AddTiles(tilesToAddToRack);

            return tileBagAfterRemoval;
        }
    }

}
