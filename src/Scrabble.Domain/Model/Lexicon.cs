using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Scrabble.Domain.Model
{
    public class Lexicon : ILexicon
    {

        //    private _words = new Array<string>();
        List<string> words = new List<string>
        {
            "Car",
            "House",
            "Dog",
            "Talent"
        };

        public bool IsWordValid(string word)
        {
            return this.words.FindIndex( w => w.ToString() == word.ToString()) > -1;
        }

    }
}
