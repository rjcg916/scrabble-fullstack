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

    public class TileBag
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


        public List<Tile> GetTiles()
        {
            var tiles = new List<Tile>();

            letters.ForEach(letter =>
            {
                for (var c = 0; c < letter.frequency; c++)
                {
                    tiles.Add(new Tile(letter.name, letter.value));
                }
            });

            return tiles;
        }
    }
}
