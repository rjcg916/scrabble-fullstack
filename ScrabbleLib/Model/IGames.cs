using System;
using System.Collections.Generic;
using System.Text;

namespace ScrabbleLib.Model
{
    public interface IGames
    {
            int CreateGame(byte p);
            Game GetGame(int i);
            void RemoveGame(int i);
    }
}
