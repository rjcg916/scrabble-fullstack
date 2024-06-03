using System;
using System.Collections.Generic;
using Xunit;

namespace Scrabble.Domain.Tests
{
    public class GameManagerTests
    {

        [Fact]
        public void CreateGame_AddsGameToDictionary()
        {
            // Arrange
            var gameManager = new GameManager();
            var players = new List<Player> { new("Alice"), new("Bob") };
            var game = Game.GameFactory.CreateGame(new Lexicon(), new PlayerList(players));

            // Act
            var gameId = gameManager.AddGame(game);

            // Assert
            Assert.Equal(1, gameManager.NumberOfGames());
            var getGame = gameManager.GetGame(gameId);
            Assert.NotNull(getGame);
            Assert.Equal(2, getGame.NumberOfPlayers);
        }

        [Fact]
        public void GetGame_ReturnsCorrectGame()
        {
            // Arrange
            var gameManager = new GameManager();
            var players1 = new List<Player> { new("Alice"), new("Bob") };
            var players2 = new List<Player> { new("Alice"), new("Bob"), new("Sam") };

            // Act
            var game1 = Game.GameFactory.CreateGame(new Lexicon(), new PlayerList(players1));
            var _ = Game.GameFactory.CreateGame(new Lexicon(), new PlayerList(players2));
            var gameId = gameManager.AddGame(game1);

            var fetchedGame = gameManager.GetGame(gameId);

            // Assert
            Assert.Equal(2, fetchedGame.NumberOfPlayers);
        }

        [Fact]
        public void GetGame_ThrowsKeyNotFoundException_WhenGameDoesNotExist()
        {
            // Arrange
            var gameManager = new GameManager();
            var nonExistentGameId = Guid.NewGuid();

            // Act & Assert
            var exception = Assert.Throws<KeyNotFoundException>(() => gameManager.GetGame(nonExistentGameId));
            Assert.Equal("Game not found.", exception.Message);
        }

        [Fact]
        public void RemoveGame_RemovesGameFromDictionary()
        {
            // Arrange
            var gameManager = new GameManager();
            var players = new List<Player> { new("Alice"), new("Bob") };
            var game = Game.GameFactory.CreateGame(new Lexicon(), new PlayerList( players));

            var gameId = gameManager.AddGame(game);

            // Act
            var removed = gameManager.RemoveGame(gameId);

            // Assert
            Assert.True(removed);
            Assert.Equal(0, gameManager.NumberOfGames());
        }

        [Fact]
        public void RemoveGame_ReturnsFalse_WhenGameDoesNotExist()
        {
            // Arrange
            var gameManager = new GameManager();
            var nonExistentGameId = Guid.NewGuid();

            // Act
            var result = gameManager.RemoveGame(nonExistentGameId);

            // Assert
            Assert.False(result);
        }


        [Fact]
        public void NumberOfGames_ReturnsCorrectCount()
        {
            // Arrange
            var gameManager = new GameManager();
            var players1 = new List<Player> { new("Alice"), new("Bob") };
            var players2 = new List<Player> { new("Charlie"), new("Dave") };
            var game1 = Game.GameFactory.CreateGame(new Lexicon(), new PlayerList(players1));
            var game2 = Game.GameFactory.CreateGame(new Lexicon(), new PlayerList(players2));

            // Act
            gameManager.AddGame(game1);
            gameManager.AddGame(game2);

            // Assert
            var numberOfGames = gameManager.NumberOfGames();
            Assert.Equal(2, numberOfGames);

        }
    }
}
