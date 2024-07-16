using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Scrabble.Domain
{

    public class Player
    {
        public Rack Rack { get; set; } = new Rack();
        public string Name { get; set; } 
        public int Score { get; set; } = 0;

        public Player(string name) {   
            Name = name;
        }

        public Player( Player player) 
        {
            Rack = new Rack( player.Rack);
            Name = player.Name;
            Score = player.Score;
        }
    }

    public class Players : IEnumerable<Player>
    {
        private readonly Player[] _players;
        private int _index;

        public Players(List<Player> players)
        {
            if (players.Count < 2 || players.Count > 4)
            {
                throw new ArgumentException("Number of players must be between 2 and 4.");
            }

            _players = new Player[players.Count];
            for (int i = 0; i < players.Count; i++)
            {
                _players[i] = new Player( players[i]);
            }

            _index = 0;
        }

        public Player CurrentPlayer
        {
            get { return _players[_index]; }
        }

        public Player GetByName(string name) =>
            _players.First(p => p.Name == name);

        public Player GetNext()
        {
            _index = (_index + 1) % _players.Length;
            return _players[_index];
        }

        public int Count =>
            _players.Length;

        public Player Leader()
        {
            return _players.OrderByDescending(p => p.Score).FirstOrDefault();
        }

    
        public IEnumerator<Player> GetEnumerator()
        {
            return ((IEnumerable<Player>)_players).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _players.GetEnumerator();
        }
    }

}
