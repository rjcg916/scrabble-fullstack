using System;
using System.Collections.Generic;

namespace Scrabble.Domain.Model
{

    public class TileBag : ITileBag
    {
        readonly string TOOMANYERROR = "Attempt to draw more tiles than present in TileBag";

        private static readonly List<(Letter letter, short freq)> letters =
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

        public List<Tile> FindAll(Predicate<Tile> match) => Tiles.FindAll(match);

        public void Shuffle()
        {
            List<Tile> randomList = [];

            Random r = new();
            int randomIndex;
            while (this.Tiles.Count > 0)
            {
                randomIndex = r.Next(0, this.Tiles.Count); //Choose a random object in the list
                randomList.Add(this.Tiles[randomIndex]); //add it to the new, random list
                this.Tiles.RemoveAt(randomIndex); //remove to avoid duplicates
            }

            this.Tiles = randomList;
        }

        public TileBag()
        {

            // add the tiles to the bag
            letters.ForEach(l =>
            {
                for (var c = 0; c < l.freq; c++)
                {
                    this.Tiles.Add(new Tile(l.letter.Name, l.letter.Value));
                }
            });

            // shuffle the bag
            this.Shuffle();
        }


        public List<Tile> DrawTiles(int drawCount)
        {

            // can't draw more than available
            if (drawCount > this.Tiles.Count)
                throw new Exception(TOOMANYERROR);

            // fetch the tiles to draw
            var tiles = this.Tiles.GetRange(0, drawCount);

            // remove tiles from bag
            tiles.ForEach(r =>
            {
                var index = this.Tiles.FindIndex(t => t.Letter == r.Letter);
                if (index > -1)
                    this.Tiles.RemoveAt(index);
            });

            // return the list of drawn tiles
            return tiles;
        }
    }
}
