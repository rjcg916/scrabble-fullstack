using System;
using System.Collections.Generic;
using System.Linq;

namespace Scrabble.Util
{
    public static class Words
    {
        public static string[] ToWords(this List<char> charArray)
        {
            char[] separator = [' ', '\t', '\n', '\r'];

            var input = new string(charArray.ToArray());

            return input.Split(separator, StringSplitOptions.RemoveEmptyEntries);
        }

        public  static (bool valid, string invalidWord) ValidateWordList(this string[] words, Func<string, bool> IsWordValid)
        {
            var invalidWord = words.FirstOrDefault(word => !IsWordValid(word));

            return invalidWord == null ?
                (true, string.Empty) : (false, invalidWord); // no invalid word
        }
    }
}