using System;
using System.Collections.Generic;

namespace Scrabble.Domain.Model
{
    public class GameManager : IGameManager
    {
        private readonly Game.GameFactory _gameFactory;
        private readonly Dictionary<Guid, Game> _games;

        public GameManager()
        {
            _gameFactory = new Game.GameFactory();
            _games = [];
        }

        public Guid CreateGame(List<string> playerNames)
        {
            var game = _gameFactory.CreateGame(playerNames);
            var gameId = Guid.NewGuid();
            _games[gameId] = game;
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