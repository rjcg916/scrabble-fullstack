using System;
using System.Collections.Generic;
using System.Text;

namespace ScrabbleLib.Model
{
    public class Games : IGames
    {


        Dictionary<int, Game> games { get; set; } = new Dictionary<int, Game>();

        int GetKey( )
        {
           return games.Count + 1;

        }
        public int CreateGame(byte numberOfPlayers = 2)
        {
            Game g = new Game(numberOfPlayers);           
            int key = GetKey();
            games.Add(key, g);
            return key;
        }

        public Game GetGame(int key)
        {
            games.TryGetValue(key, out Game g);
            return g;
        }

        public void RemoveGame(int key)
        {
            games.Remove(key);
                        
        }

    }

}
