using System;
using System.Collections.Generic;

namespace Scrabble.Domain
{
    public class Lexicon : ILexicon
    {

        readonly List<string> words =
        [
            "Car",
            "House",
            "Dog",
            "Talent"
        ];


        public bool IsWordValid(string word) =>
            words.FindIndex(w => w.Equals(word, StringComparison.OrdinalIgnoreCase)) > -1;

    }
}
