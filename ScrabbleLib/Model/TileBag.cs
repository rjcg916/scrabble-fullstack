using System;
using System.Collections.Generic;
using System.Text;

namespace ScrabbleLib.Model
{
    class Letter
    {
        public String name { get; set; }
        public short value { get; set; }
        public short frequency { get; set; }

        public Letter(string name, short value, short frequency)
        {
            this.name = name;
            this.value = value;
            this.frequency = frequency;
        }
    }

    public interface ITileBag
    {
        int count { get; }

        List<Tile> DrawTiles(int drawCount);

    }
    public class TileBag : ITileBag
    {
        private static List<Letter> letters = new List<Letter>()
      {
        new Letter("A", 1, 9),
        new Letter("B", 3, 2),
        new Letter("C", 3, 2),
        new Letter("D", 2, 4),
        new Letter("E", 1, 12),
        new Letter("F", 4, 2),
        new Letter("G", 2, 3),
        new Letter("H", 4, 2),
        new Letter("I", 1, 9),
        new Letter("J", 8, 1),
        new Letter("K", 5, 1),
        new Letter("L", 1, 4),
        new Letter("M", 3, 2),
        new Letter("N", 1, 6),
        new Letter("O", 1, 8),
        new Letter("P", 3, 2),
        new Letter("Q", 10, 1),
        new Letter("R", 1, 6),
        new Letter("S", 1, 4),
        new Letter("T", 1, 6),
        new Letter("U", 1, 4),
        new Letter("V", 4, 2),
        new Letter("W", 4, 2),
        new Letter("X", 8, 1),
        new Letter("Y", 4, 2),
        new Letter("Z", 10, 1),
        new Letter("", 0, 2)
        };

        public List<Tile> tiles { get; set; } = new List<Tile>();

        public int count {
            get { return tiles.Count; }
        }

        public void Shuffle()
        {
            List<Tile> randomList = new List<Tile>();

            Random r = new Random();
            int randomIndex = 0;
            while (this.tiles.Count > 0)
            {
                randomIndex = r.Next(0, this.tiles.Count); //Choose a random object in the list
                randomList.Add(this.tiles[randomIndex]); //add it to the new, random list
                this.tiles.RemoveAt(randomIndex); //remove to avoid duplicates
            }

            this.tiles = randomList;
        }

        public TileBag()
        {

            // add the tiles to the bag
            letters.ForEach(letter =>
            {
                for (var c = 0; c < letter.frequency; c++)
                {
                    this.tiles.Add(new Tile(letter.name, letter.value));
                }
            });

            // shuffle the bag
            this.Shuffle();
        }


        public List<Tile> DrawTiles(int drawCount)
        {

            // can't draw more than available
            if (drawCount > this.tiles.Count)
                throw new Exception("Attemp to draw more tiles than present in TileBag");

            // fetch the tiles to draw
            var tiles = this.tiles.GetRange(0, drawCount);

            // remove tiles from bag
            tiles.ForEach(r =>
            {
                var index = this.tiles.FindIndex(t => t.Letter == r.Letter);
                if (index > -1)
                    this.tiles.RemoveAt(index);
            });

            // return the list of drawn tiles
            return tiles;
        }
    }
}
