using System;
using System.Collections.Generic;
using System.Text;

namespace Scrabble.Domain.Model
{
    interface ILexicon
    {
        bool IsWordValid(string word);
    }
}
