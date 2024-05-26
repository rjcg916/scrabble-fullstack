using System.Collections.Generic;

namespace Scrabble.Domain.Model
{
    public class Games : IGames
    {
        Dictionary<int, Game> GamesDic { get; set; } = [];

        int GetKey()
        {
            return GamesDic.Count + 1;

        }
        public int CreateGame(List<string> playerNames)
        {
            Game g = new(playerNames);
            int key = GetKey();
            GamesDic.Add(key, g);
            return key;
        }

        public Game GetGame(int key)
        {

            GamesDic.TryGetValue(key, out Game g);

            return g;
        }

        public void RemoveGame(int key)
        {
            GamesDic.Remove(key);

        }

        public int Count()
        {
            return GamesDic.Count;
        }

    }

}
