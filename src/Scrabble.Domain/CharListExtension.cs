using System;
using System.Collections.Generic;
using System.Linq;

namespace Scrabble.Domain
{
    public static class CharListExtension
    {

        static public (bool valid, string invalidWord) ValidSequence(this List<char> charArray, Func<string, bool> IsWordValid)
        {
            char[] separator = [' ', '\t', '\n', '\r'];

            var input = new String(charArray.ToArray());

            // Split the input string by whitespace
            var words = input.Split(separator, StringSplitOptions.RemoveEmptyEntries);

            // Check each word using the IsWordValid function
            foreach (var word in words)
            {
                if (!IsWordValid(word))
                {
                    // Return false and the first invalid character
                    return (false, word);
                }
            }

            // If all words are valid, return true
            return (true, ""); // '\0' is the null character indicating no invalid character
        }
    }
}
