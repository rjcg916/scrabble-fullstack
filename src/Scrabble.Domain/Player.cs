using System;
using System.Collections.Generic;

namespace Scrabble.Domain
{
    public class Player
    {
        public Rack Rack { get; set; } = new Rack();
        public string Name { get; set; }

        public Player(string name, TileBag tileBag)
        {
            Name = name;
            DrawTiles(tileBag);
        }

        public (List<Tile>, TileBag) DrawTiles(TileBag tileBag)
        {
            var tilesAvailable = tileBag.Count;

            if (tilesAvailable == 0)
                throw new Exception("No tiles available to draw.");

            var tilesNeeded = Rack.Capacity - Rack.TileCount;

            var drawCount = tilesAvailable > tilesNeeded ? tilesNeeded : tilesAvailable;

            var (tilesToAdd, newBag) = tileBag.DrawTiles(drawCount);
            Rack.AddTiles(tilesToAdd);

            return (tilesToAdd, newBag);
        }
        
        public Board PlaceTile(Board board, Coord coord, Tile tile)
        {
            Rack =  Rack.RemoveTiles([tile]);
            
            return board.PlaceTile(coord, tile);
        }
    }
}
