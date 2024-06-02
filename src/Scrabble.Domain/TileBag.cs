using System;
using System.Collections.Generic;
using System.Linq;

namespace Scrabble.Domain
{

    public class TileBag : ITileBag
    {
        readonly string TOOMANYERROR = "Attempt to draw more tiles than present in TileBag";

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

        public TileBag()
        {
            // add the tiles to the bag

            Tiles.AddRange(
                tiles.SelectMany(tf => Enumerable.Repeat(new Tile(tf.tile.Letter), tf.freq))
            );

            // shuffle the bag
            Shuffle();
        }

        public (List<Tile>, TileBag) DrawTiles(int drawCount)
        {

            // can't draw more than available
            if (drawCount > Tiles.Count)
                throw new Exception(TOOMANYERROR);

            // Fetch the tiles to draw
            var drawnTiles = Tiles.GetRange(0, drawCount);

            // Create a new instance of TileBag with remaining tiles
            var remainingTiles = new List<Tile>(Tiles);
            remainingTiles.RemoveRange(0, drawCount);
            var newTileBag = new TileBag { Tiles = remainingTiles };

            // Return the drawn tiles and the new instance of TileBag
            return (drawnTiles, newTileBag);

        }

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
    }
}