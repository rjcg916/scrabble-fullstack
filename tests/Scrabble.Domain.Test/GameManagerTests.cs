using System;
using System.Collections.Generic;
using Xunit;
using Scrabble.Domain;

namespace Scrabble.Domain.Tests
{
    public class GameManagerTests
    {
        [Fact]
        public void CreateGame_ReturnsValidGuid()
        {
            // Arrange
            var gameManager = new GameManager();
            var playerNames = new List<string> { "Alice", "Bob" };

            // Act
            var gameId = gameManager.CreateGame(playerNames);

            // Assert
            Assert.NotEqual(Guid.Empty, gameId);
        }

        [Fact]
        public void CreateGame_AddsGameToDictionary()
        {
            // Arrange
            var gameManager = new GameManager();
            var playerNames = new List<string> { "Alice", "Bob" };

            // Act
            var gameId = gameManager.CreateGame(playerNames);

            // Assert
            Assert.Equal(1, gameManager.NumberOfGames());
            var game = gameManager.GetGame(gameId);
            Assert.NotNull(game);
            Assert.Equal(2, game.NumberOfPlayers);
        }

        [Fact]
        public void GetGame_ReturnsCorrectGame()
        {
            // Arrange
            var gameManager = new GameManager();
            var playerNames = new List<string> { "Alice", "Bob" };
            var gameId = gameManager.CreateGame(playerNames);

            // Act
            var game = gameManager.GetGame(gameId);

            // Assert
            Assert.NotNull(game);
            Assert.Equal(2, game.NumberOfPlayers);
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
            var playerNames = new List<string> { "Alice", "Bob" };
            var gameId = gameManager.CreateGame(playerNames);

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
        public void GetAllGames_ReturnsAllGames()
        {
            // Arrange
            var gameManager = new GameManager();
            var playerNames1 = new List<string> { "Alice", "Bob" };
            var playerNames2 = new List<string> { "Charlie", "Dave" };
            gameManager.CreateGame(playerNames1);
            gameManager.CreateGame(playerNames2);

            // Act
            var allGames = gameManager.GetAllGames();

            // Assert
            Assert.Equal(2, allGames.Count);
        }

        [Fact]
        public void NumberOfGames_ReturnsCorrectCount()
        {
            // Arrange
            var gameManager = new GameManager();
            var playerNames1 = new List<string> { "Alice", "Bob" };
            var playerNames2 = new List<string> { "Charlie", "Dave" };
            gameManager.CreateGame(playerNames1);
            gameManager.CreateGame(playerNames2);

            // Act
            var numberOfGames = gameManager.NumberOfGames();

            // Assert
            Assert.Equal(2, numberOfGames);
        }
    }
}
