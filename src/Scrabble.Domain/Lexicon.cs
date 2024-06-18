using System;
using System.Collections.Generic;
using System.Linq;

namespace Scrabble.Domain
{
    public static class LexiconExtension
    {
        public static (bool valid, string invalidWord) IsValidWordList(this List<char> charArray, Func<string, bool> IsWordValid)
        {
            char[] separator = [' ', '\t', '\n', '\r'];

            var input = new string(charArray.ToArray());

            var words = input.Split(separator, StringSplitOptions.RemoveEmptyEntries);

            var invalidWord = words.FirstOrDefault(word => !IsWordValid(word));

            if (invalidWord?.Length > 1) // single char are valid
            {
                return (false, invalidWord);
            }

            return (true, string.Empty); // no invalid word
        }
    }
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