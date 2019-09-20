using System;
using System.Collections.Generic;
using System.Text;

namespace ScrabbleLib.Model
{
    interface ILexicon
    {
        bool IsWordValid(string word);
    }
}
