using System;
using System.Collections.Generic;
using System.Linq;

namespace Scrabble.Domain
{
    public static class LexiconExtensions
    {
        public static (bool valid, string invalidWord) IsValidWordList(this List<char> charArray, Func<string, bool> IsWordValid)
        {
            char[] separator = [' ', '\t', '\n', '\r'];

            var input = new string(charArray.ToArray());

            var words = input.Split(separator, StringSplitOptions.RemoveEmptyEntries);

            var invalidWord = words.FirstOrDefault(word => !IsWordValid(word));

            return invalidWord?.Length > 0 ? 
                    (false, invalidWord) : (true, string.Empty); // no invalid word
        }
    }
}
