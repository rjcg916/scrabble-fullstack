using System;
using System.Collections.Generic;
using Xunit;


namespace Scrabble.Domain.Tests
{
    public class GameTests
    {
        [Fact]
        public void GameFactory_CreateGame_ThrowsException_WhenPlayersLessThanMin()
        {
            // Arrange
            var lexicon = new Lexicon();
            var gameFactory = new Game.GameFactory(lexicon);
            var playerNames = new List<string> { "Alice" }; // Only 1 player

            // Act & Assert
            var exception = Assert.Throws<Exception>(() => gameFactory.CreateGame(playerNames));
            Assert.Equal("Game must have 2, 3 or 4 players.", exception.Message);
        }

        [Fact]
        public void GameFactory_CreateGame_ThrowsException_WhenPlayersMoreThanMax()
        {
            // Arrange
            var lexicon = new Lexicon();
            var gameFactory = new Game.GameFactory(lexicon);
            var playerNames = new List<string> { "Alice", "Bob", "Charlie", "Dave", "Eve" }; // 5 players

            // Act & Assert
            var exception = Assert.Throws<Exception>(() => gameFactory.CreateGame(playerNames));
            Assert.Equal("Game must have 2, 3 or 4 players.", exception.Message);
        }

        [Fact]
        public void GameFactory_CreateGame_CreatesGameWithValidNumberOfPlayers()
        {
            // Arrange
            var lexicon = new Lexicon();
            var gameFactory = new Game.GameFactory(lexicon);
            var playerNames = new List<string> { "Alice", "Bob", "Charlie" }; // 3 players

            // Act
            var game = gameFactory.CreateGame(playerNames);

            // Assert
            Assert.NotNull(game);
            Assert.Equal(3, game.NumberOfPlayers);
            Assert.NotNull(game.Lexicon);
            Assert.NotNull(game.TileBag);
            Assert.NotNull(game.Board);
            Assert.False(game.GameDone);
            Assert.Equal(1, game.TurnOfPlayer); // Turn should start with player 1
        }

        [Fact]
        public void GameFactory_CreateGame_InitializesPlayersCorrectly()
        {
            // Arrange
            var lexicon = new Lexicon();
            var gameFactory = new Game.GameFactory(lexicon);
            var playerNames = new List<string> { "Alice", "Bob" }; // 2 players

            // Act
            var game = gameFactory.CreateGame(playerNames);

            // Assert
            Assert.Equal(playerNames.Count, game.NumberOfPlayers);
            Assert.True(game.Players.ContainsKey(1));
            Assert.True(game.Players.ContainsKey(2));
            Assert.Equal("Alice", game.Players[1].Name);
            Assert.Equal("Bob", game.Players[2].Name);
        }

        [Fact]
        public void GameFactory_CreateGame_DrawsInitialTilesForPlayers()
        {
            // Arrange
            var lexicon = new Lexicon();
            var gameFactory = new Game.GameFactory(lexicon);
            var playerNames = new List<string> { "Alice", "Bob" }; // 2 players

            // Act
            var game = gameFactory.CreateGame(playerNames);

            // Assert
            foreach (var player in game.Players.Values)
            {
                Assert.Equal(player.Rack.TileCount, Rack.Capacity); // Ensure players have full rack
            }
        }
    }
}
