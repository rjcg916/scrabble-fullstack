﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ScrabbleLib.Model
{
    public interface IGames
    {
        int CreateGame(List<String> playerNames);
        Game GetGame(int i);
        void RemoveGame(int i);
        int Count();
    }
}
