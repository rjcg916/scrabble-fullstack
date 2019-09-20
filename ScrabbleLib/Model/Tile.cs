using System;
using System.Collections.Generic;
using System.Text;

namespace ScrabbleLib.Model
{
    public class Tile
    {
        public string Letter { get; }
        public int Value { get; } = 1;
    
        public Tile(String letter, int Value = 1) {
            Letter = letter.ToUpper();
            this.Value = Value;
        }
  }

}




