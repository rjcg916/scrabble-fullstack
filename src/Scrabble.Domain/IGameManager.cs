using System;
using System.Collections.Generic;

namespace Scrabble.Domain
{
    public interface IGameManager
    {
        public Guid CreateGame(List<string> playerNames);

        public Game GetGame(Guid gameId);

        public bool RemoveGame(Guid gameId);

        public int NumberOfGames();
    }
}