using System;
using System.Collections.Generic;
using System.Linq;

namespace Scrabble.Domain
{
    public readonly struct TileDrawCount
    {
        public int Value { get; }

        public TileDrawCount(int count)
        {
            if (!IsValid(count))
                throw new ArgumentException($"Not a valid tile count {count}");

            Value = count;
        }

        private static bool IsValid(int count)
            => 0 <= count && count <= Rack.Capacity;
    }

    public class TileBag : ITileBag
    {
        // readonly string TOOMANYTILES = "Attempt to draw more tiles than present in TileBag";

        private static readonly List<(Tile tile, ushort freq)> tiles =
        [
            new( new Tile('A'), 9),
            new( new Tile('B'), 2),
            new( new Tile('C'), 2),
            new( new Tile('D'), 4),
            new( new Tile('E'), 12),
            new( new Tile('F'), 2),
            new( new Tile('G'), 3),
            new( new Tile('H'), 2),
            new( new Tile('I'), 9),
            new( new Tile('J'), 1),
            new( new Tile('K'), 1),
            new( new Tile('L'), 4),
            new( new Tile('M'), 2),
            new( new Tile('N'), 6),
            new( new Tile('O'), 8),
            new( new Tile('P'), 2),
            new( new Tile('Q'), 1),
            new( new Tile('R'), 6),
            new( new Tile('S'), 4),
            new( new Tile('T'), 6),
            new( new Tile('U'), 4),
            new( new Tile('V'), 2),
            new( new Tile('W'), 2),
            new( new Tile('X'), 1),
            new( new Tile('Y'), 2),
            new( new Tile('Z'), 1),
            new( new Tile(' '), 2)
        ];

        private List<Tile> Tiles { get; set; } = [];

        public int Count
        {
            get { return Tiles.Count; }
        }

        public List<Tile> FindAll(Predicate<Tile> match) =>
            Tiles.FindAll(match);


        private TileBag() { }

        public static class TileBagFactory
        {
            public static TileBag Create()
            {
                
                // create empty bag
                TileBag tileBag = new();

                // add starting tiles to the bag
                tileBag.Tiles.AddRange(
                   tiles.SelectMany(tf => Enumerable.Repeat(new Tile(tf.tile.Letter), tf.freq))
                );

                // shuffle
                tileBag.Shuffle();

                return tileBag;
            }

            public static TileBag Copy(TileBag tileBag)
            {
                TileBag copyTileBag = new ();

                copyTileBag.Tiles.AddRange(tileBag.Tiles);

                return copyTileBag;
            }

        }

        public (List<Tile>, TileBag) DrawTiles(TileDrawCount count)
        {

            var drawCount = count.Value;

            if (drawCount > Tiles.Count)
            {
                throw new ArgumentException($"Attempt to draw more tiles {drawCount} than present in TileBag");
            }

            var drawnTiles = Tiles.GetRange(0, drawCount);

            var remainingTiles = new List<Tile>(Tiles);
            remainingTiles.RemoveRange(0, drawCount);
            var newTileBag = new TileBag { Tiles = remainingTiles };

            return (drawnTiles, newTileBag);

        }

        public List<Tile> Peek() { 
            return Tiles; 
        }

        public void Shuffle()
        {
            Random r = new();
            int n = this.Tiles.Count;

            for (int i = n - 1; i > 0; i--)
            {
                int j = r.Next(0, i + 1);
                (this.Tiles[j], this.Tiles[i]) = (this.Tiles[i], this.Tiles[j]);
            }
        } 
    }
}