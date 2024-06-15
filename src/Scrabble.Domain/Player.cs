using System;
using System.Collections.Generic;

namespace Scrabble.Domain
{

    public readonly struct PlayerList
    {
        public List<Player> List { get; }

        public PlayerList (List<Player> players) { 
            
            if (!IsValid(players)) 
                throw new ArgumentException($"{players.Count} is not a valid players list size");

            List = players;       
        }

        private static bool IsValid(List<Player> players) =>
            players.Count > 1 && players.Count <= 4;

    }


    public class Player(string name)
    {
        public Rack Rack { get; set; } = new Rack();
        public string Name { get; set; } = name;
        public int Score { get; } = 0;
    }

}
