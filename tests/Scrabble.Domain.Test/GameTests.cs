using System;
using System.Collections.Generic;
using System.Linq;
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
            var tileBag = TileBag.TileBagFactory.Create();
            var players = new List<Player> { new("A")};

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => Game.GameFactory.CreateGame(lexicon,players));
            Assert.Contains("is not a valid players list size", exception.Message);
        }

        [Fact]
        public void GameFactory_CreateGame_ThrowsException_WhenPlayersMoreThanMax()
        {
            // Arrange
            var lexicon = new Lexicon();
            var tileBag = TileBag.TileBagFactory.Create();
            var players = new List<Player> {  new("A"), new("B"), new("C"), new("D"), new("E"), new("Alice") }; // 5 players

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => Game.GameFactory.CreateGame(lexicon, players));
            Assert.Contains("is not a valid players list size", exception.Message);
        }

        [Fact]
        public void GameFactory_CreateGame_CreatesGameWithValidNumberOfPlayers()
        {
            // Arrange
            var lexicon = new Lexicon();
            var players = new List<Player> { new("A"), new("B"), new("C") };

            // Act
            var game = Game.GameFactory.CreateGame(lexicon, players);

            // Assert
            Assert.NotNull(game);
            Assert.Equal(3, game.Players.Count);
            Assert.NotNull(game.Lexicon);
            Assert.NotNull(game.TileBag);
            Assert.NotNull(game.Board);
            //Assert.False(game.GameDone);
            Assert.Equal("A", game.Players.CurrentPlayer.Name); // Turn should start with player 1
        }

        [Fact]
        public void GameFactory_CreateGame_InitializesPlayersCorrectly()
        {
            // Arrange
            var lexicon = new Lexicon();            
            var players = new List<Player> { new("A"), new("B") };

            // Act
            var game = Game.GameFactory.CreateGame(lexicon, players);

            // Assert
            Assert.Equal(players.Count, game.Players.Count);
            Assert.Equal("A", game.Players.First().Name);            
            Assert.Equal("B", game.Players.GetNext().Name );
        }

        [Fact]
        public void GameFactory_CreateGame_DrawsInitialTilesForPlayers()
        {
            // Arrange
            var lexicon = new Lexicon();
            var players = new List<Player> { new("A"), new("B") };

            // Act
            var game = Game.GameFactory.CreateGame(lexicon, players);

            // Assert
            foreach (var player in game.Players)
            {
                Assert.Equal(player.Rack.TileCount, Rack.Capacity); // Ensure players have full rack
            }
        }
    }
}
