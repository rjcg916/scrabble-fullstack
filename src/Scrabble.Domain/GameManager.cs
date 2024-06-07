using System;
using System.Collections.Generic;

namespace Scrabble.Domain
{
    public class GameManager : IGameManager
    {
      
        private readonly Dictionary<Guid, Game> _games = [];

    
        public Guid AddGame(Game game)
        {
            var gameId = Guid.NewGuid();
            _games.Add(gameId, game);
            return gameId;
        }

        public Game GetGame(Guid gameId)
        {
            if (_games.TryGetValue(gameId, out Game value))
            {
                return value;
            }
            throw new KeyNotFoundException("Game not found.");
        }

        public bool RemoveGame(Guid gameId)
        {
            return _games.Remove(gameId);
        }

        public IReadOnlyDictionary<Guid, Game> GetAllGames()
        {
            return _games;
        }

        public int NumberOfGames() => _games.Count;
    }
}