using System.Collections.Generic;

namespace Scrabble.Domain.Model
{
    public class Games : IGames
    {

        Dictionary<int, Game> games { get; set; } = [];

        int GetKey()
        {
            return games.Count + 1;

        }
        public int CreateGame(List<string> playerNames)
        {
            Game g = new(playerNames);
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

        public int Count()
        {
            return games.Count;
        }

    }

}
