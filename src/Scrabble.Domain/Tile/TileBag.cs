﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Scrabble.Domain
{
    public class TileBag : ITileBag
    {
        public static class TileBagFactory
        {
            public static TileBag Create()
            {
                // create starting tiles for the bag
                var tilesList = tilesAndFrequencies
                    .SelectMany(tf => Enumerable.Repeat(new Tile(tf.tile.Letter), tf.freq))
                    .ToList();

                // shuffle the tiles
                return new TileBag(tilesList);//Shuffle(tilesList);
            }
            
            public static TileBag Create(List<Tile> tiles)
            {
                // shuffle the tiles
                return new TileBag(tiles); // Shuffle(tiles);
            }

            public static TileBag Create(TileBag tileBag)
            {
                return new TileBag(tileBag.Tiles);
            }
        }

        public List<Tile> Tiles { get; }

        public int Count => Tiles.Count;

        private TileBag(List<Tile> tiles)
        {
            Tiles = tiles;
            Shuffle();
        }


        public List<Tile> Peek() =>
            [.. Tiles];

        public void Shuffle()
        {
            Random r = new();
            int n = Tiles.Count;

            for (int i = n - 1; i > 0; i--)
            {
                int j = r.Next(0, i + 1);
                (Tiles[j], Tiles[i]) = (Tiles[i], Tiles[j]);
            }          
        }

        public (List<Tile> drawnTiles, TileBag newTileBag) DrawTiles(TileDrawCount count)
        {
            var drawCount = count.Value;

            if (drawCount > Tiles.Count)
            {
                throw new ArgumentException($"Attempt to draw more tiles {drawCount} than present in TileBag");
            }

            var drawnTiles = Tiles.Take(drawCount).ToList();
            var remainingTiles = Tiles.Skip(drawCount).ToList();

            var newTileBag = new TileBag(remainingTiles);

            return (drawnTiles, newTileBag);
        }


        private static readonly List<(Tile tile, ushort freq)> tilesAndFrequencies =
        [
            new(new Tile('A'), 9),
            new(new Tile('B'), 2),
            new(new Tile('C'), 2),
            new(new Tile('D'), 4),
            new(new Tile('E'), 12),
            new(new Tile('F'), 2),
            new(new Tile('G'), 3),
            new(new Tile('H'), 2),
            new(new Tile('I'), 9),
            new(new Tile('J'), 1),
            new(new Tile('K'), 1),
            new(new Tile('L'), 4),
            new(new Tile('M'), 2),
            new(new Tile('N'), 6),
            new(new Tile('O'), 8),
            new(new Tile('P'), 2),
            new(new Tile('Q'), 1),
            new(new Tile('R'), 6),
            new(new Tile('S'), 4),
            new(new Tile('T'), 6),
            new(new Tile('U'), 4),
            new(new Tile('V'), 2),
            new(new Tile('W'), 2),
            new(new Tile('X'), 1),
            new(new Tile('Y'), 2),
            new(new Tile('Z'), 1),
            new(new Tile('?'), 2)
        ];
    }
    public readonly struct TileDrawCount
    {
        public int Value { get; }

        public TileDrawCount(int count)
        {
            if (!IsValid(count))
                throw new ArgumentException($"Not a valid tile draw count {count}");

            Value = count;
        }

        private static bool IsValid(int count)
            => 0 <= count && count <= Rack.Capacity;
    }
}
