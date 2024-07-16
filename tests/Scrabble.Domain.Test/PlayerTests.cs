using System;
using System.Collections.Generic;
using Xunit;


namespace Scrabble.Domain.Tests
{

    public class PlayerTests
    {
        [Fact]
        public void Player_Initialization_ShouldSetPropertiesCorrectly()
        {
            // Arrange
            var playerName = "John Doe";

            // Act
            var player = new Player(playerName);

            // Assert
            Assert.Equal(playerName, player.Name);
            Assert.Equal(0, player.Score);
            Assert.NotNull(player.Rack);
        }

        [Fact]
        public void Player_CopyConstructor_ShouldCopyPropertiesCorrectly()
        {
            // Arrange
            var originalPlayer = new Player("Jane Doe")
            {
                Score = 100
            };
            originalPlayer.Rack = new Rack(); // Assuming Rack has a copy constructor

            // Act
            var copiedPlayer = new Player(originalPlayer);

            // Assert
            Assert.Equal(originalPlayer.Name, copiedPlayer.Name);
            Assert.Equal(originalPlayer.Score, copiedPlayer.Score);
            Assert.NotSame(originalPlayer.Rack, copiedPlayer.Rack);
        }
    }

    public class PlayersTests
    {
        [Fact]
        public void Players_Initialization_ShouldThrowExceptionForInvalidPlayerCount()
        {
            // Arrange
            var tooFewPlayers = new List<Player> { new Player("Player 1") };
            var tooManyPlayers = new List<Player>
            {
                new Player("Player 1"),
                new Player("Player 2"),
                new Player("Player 3"),
                new Player("Player 4"),
                new Player("Player 5")
            };

            // Act & Assert
            Assert.Throws<ArgumentException>(() => new Players(tooFewPlayers));
            Assert.Throws<ArgumentException>(() => new Players(tooManyPlayers));
        }

        [Fact]
        public void Players_Initialization_ShouldInitializeCorrectly()
        {
            // Arrange
            var playersList = new List<Player>
            {
                new Player("Player 1"),
                new Player("Player 2")
            };

            // Act
            var players = new Players(playersList);

            // Assert
            Assert.Equal(2, players.Count);
            Assert.Equal("Player 1", players.CurrentPlayer.Name);
        }

        [Fact]
        public void Players_GetNext_ShouldCycleThroughPlayersCorrectly()
        {
            // Arrange
            var playersList = new List<Player>
            {
                new Player("Player 1"),
                new Player("Player 2")
            };
            var players = new Players(playersList);

            // Act & Assert
            Assert.Equal("Player 1", players.CurrentPlayer.Name);
            Assert.Equal("Player 2", players.GetNext().Name);
            Assert.Equal("Player 1", players.GetNext().Name);
        }

        [Fact]
        public void Players_GetByName_ShouldReturnCorrectPlayer()
        {
            // Arrange
            var playersList = new List<Player>
            {
                new Player("Player 1"),
                new Player("Player 2")
            };
            var players = new Players(playersList);

            // Act
            var player = players.GetByName("Player 2");

            // Assert
            Assert.Equal("Player 2", player.Name);
        }

        [Fact]
        public void Players_Leader_ShouldReturnPlayerWithHighestScore()
        {
            // Arrange
            var playersList = new List<Player>
            {
                new Player("Player 1") { Score = 100 },
                new Player("Player 2") { Score = 200 }
            };
            var players = new Players(playersList);

            // Act
            var leader = players.Leader();

            // Assert
            Assert.Equal("Player 2", leader.Name);
        }
    }
}
