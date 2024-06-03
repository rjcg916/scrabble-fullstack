using System;
using System.Collections.Generic;
using System.Linq;

namespace Scrabble.Domain
{
    public static class TileExtensions
    {
        public static List<Tile> LettersToTiles(this string letters) =>
            letters == null ?
                throw new ArgumentNullException(nameof(letters)) :
                letters.Select(letter => new Tile(letter)).ToList();

        public static string TilesToLetters(this List<Tile> tiles) =>
            string.Concat(tiles.Select(t => t.Letter));
    }
}