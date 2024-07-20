using System.Collections.Generic;

namespace Scrabble.Domain
{
    public class Lexicon : ILexicon
    {
        private readonly HashSet<string> words;

        public Lexicon() { 
            List<string> list = ["car", "house", "dog", "talent"];
            Initialize(list);
        }

        public Lexicon(IEnumerable<string> list) {
            Initialize(list);
        }
        
        private void Initialize(IEnumerable<string> list)
        {
            foreach (var word in list)
            {
                this.words.Add(word.ToLower());
            }
        }

        public bool IsWordValid(string word) =>
            !(word == null) && words.Contains(word.ToLower());
    }
}