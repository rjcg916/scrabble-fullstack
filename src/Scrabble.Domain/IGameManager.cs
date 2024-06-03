using System;
using System.Collections.Generic;

namespace Scrabble.Domain
{
    public interface IGameManager
    {
        public Guid AddGame(Game game);

        public Game GetGame(Guid gameId);

        public bool RemoveGame(Guid gameId);

        public int NumberOfGames();
    }
}