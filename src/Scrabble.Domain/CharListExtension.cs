using System;
using System.Collections.Generic;
using System.Linq;

namespace Scrabble.Domain
{
    public static class CharListExtension
    {

        public static (bool valid, string invalidWord) IsValidSequence(this List<char> charArray, Func<string, bool> IsWordValid)
        {
            char[] separator = [' ', '\t', '\n', '\r'];

            var input = new string(charArray.ToArray());

            // Split the input string by whitespace
            var words = input.Split(separator, StringSplitOptions.RemoveEmptyEntries);

            // Find the first invalid word using LINQ
            var invalidWord = words.FirstOrDefault(word => !IsWordValid(word));

            // If there is an invalid word, return false and the invalid word
            if (invalidWord != null)
            {
                return (false, invalidWord);
            }

            // If all words are valid, return true
            return (true, string.Empty); // no invalid word
        }
    }
}
