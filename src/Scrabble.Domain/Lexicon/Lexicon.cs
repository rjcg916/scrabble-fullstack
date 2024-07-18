using Scrabble.Domain.Interface;
using System.Collections.Generic;

namespace Scrabble.Domain
{
    public class Lexicon : ILexicon
    {
        private readonly HashSet<string> words;

        public Lexicon() { 
            this.words = ["car", "house", "dog", "talent"];
        }
        public Lexicon(IEnumerable<string> list) {
            this.words = new(list);            
        }

        public bool IsWordValid(string word) =>
            !(word == null) && words.Contains(word.ToLower());
    }
}