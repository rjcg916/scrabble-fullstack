using System.Collections.Generic;

namespace Scrabble.Domain.Model
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

        public bool IsWordValid(string word)
        {
            return this.words.FindIndex(w => w.ToString() == word.ToString()) > -1;
        }

    }
}
