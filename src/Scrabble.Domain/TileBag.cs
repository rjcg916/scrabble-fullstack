using System;
using System.Collections.Generic;
using System.Linq;

namespace Scrabble.Domain
{

    public class TileBag : ITileBag
    {
        readonly string TOOMANYERROR = "Attempt to draw more tiles than present in TileBag";

        private static readonly List<(Letter letter, ushort freq)> letters =
        [
            new( new Letter('A', 1), 9),
            new( new Letter('B', 3), 2),
            new( new Letter('C', 3), 2),
            new( new Letter('D', 2), 4),
            new( new Letter('E', 1), 12),
            new( new Letter('F', 4), 2),
            new( new Letter('G', 2), 3),
            new( new Letter('H', 4), 2),
            new( new Letter('I', 1), 9),
            new( new Letter('J', 8), 1),
            new( new Letter('K', 5), 1),
            new( new Letter('L', 1), 4),
            new( new Letter('M', 3), 2),
            new( new Letter('N', 1), 6),
            new( new Letter('O', 1), 8),
            new( new Letter('P', 3), 2),
            new( new Letter('Q', 10), 1),
            new( new Letter('R', 1), 6),
            new( new Letter('S', 1), 4),
            new( new Letter('T', 1), 6),
            new( new Letter('U', 1), 4),
            new( new Letter('V', 4), 2),
            new( new Letter('W', 4), 2),
            new( new Letter('X', 8), 1),
            new( new Letter('Y', 4), 2),
            new( new Letter('Z', 10), 1),
            new( new Letter(' ', 0), 2)
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
                letters.SelectMany(l => Enumerable.Repeat(new Tile(l.letter.Name, l.letter.Value), l.freq))
            );

            // shuffle the bag
            Shuffle();
        }


        public List<Tile> DrawTiles(int drawCount)
        {

            // can't draw more than available
            if (drawCount > Tiles.Count)
                throw new Exception(TOOMANYERROR);

            // Fetch the tiles to draw
            var tiles = Tiles.GetRange(0, drawCount);

            // Remove tiles from bag
            Tiles.RemoveRange(0, drawCount);

            // return the list of drawn tiles
            return tiles;
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