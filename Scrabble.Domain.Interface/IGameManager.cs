using Scrabble.Domain;

namespace Scrabble.Domain.Interface
{
    public interface IGameManager
    {
        public Guid AddGame(Game game);

        public Game GetGame(Guid gameId);

        public bool RemoveGame(Guid gameId);

        public int NumberOfGames();
    }
}