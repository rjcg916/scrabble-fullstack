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


        [Fact]
        public void DrawTiles_ShouldThrowException_WhenNoTilesAvailable()
        {
            // Arrange
            var player = new Player("Player");
            var emptyTileBag = TileBag.TileBagFactory.Create(new List<Tile>());

            // Act & Assert
            Assert.Throws<Exception>(() => player.DrawTiles(emptyTileBag));
        }

        [Fact]
        public void DrawTiles_ShouldDrawCorrectNumberOfTiles_WhenTilesAvailableLessThanNeeded()
        {
            // Arrange
            var player = new Player("Player") { Rack = new Rack (new List<Tile> { new Tile('X'), new Tile('Y'), new Tile('Z') } ) };
            var tileBag = TileBag.TileBagFactory.Create(new List<Tile> { new Tile('A'), new Tile('B') });

            // Act
            var resultTileBag = player.DrawTiles(tileBag);

            // Assert
            Assert.Equal(5, player.Rack.TileCount); // 3 existing + 2 drawn
            Assert.Equal(0, resultTileBag.Count); // 2 drawn from original count of 2
        }

        [Fact]
        public void DrawTiles_ShouldDrawCorrectNumberOfTiles_WhenTilesAvailableMoreThanNeeded()
        {
            // Arrange
            var player = new Player("Player") { Rack = new Rack(new List<Tile> { new Tile('X'), new Tile('Y'), new Tile('Z') }) };
            var tileBag = 
                TileBag.TileBagFactory.Create(new List<Tile> { new Tile('A'), new Tile('B'), new Tile('C'), new Tile('D'), new Tile('E')  });

            // Act
            var resultTileBag = player.DrawTiles(tileBag);

            // Assert
            Assert.Equal(7, player.Rack.TileCount); // 3 existing + 4 drawn (to fill capacity)
            Assert.Equal(1, resultTileBag.Count); // 5 original - 4 drawn
        }

        [Fact]
        public void DrawTiles_ShouldDrawAllTiles_WhenTilesAvailableEqualsNeeded()
        {
            // Arrange
            var player = new Player("Player") { Rack = new Rack(new List<Tile> { new Tile('X'), new Tile('Y'), new Tile('Z') }) };
            var tileBag =
                       TileBag.TileBagFactory.Create(new List<Tile> { new Tile('A'), new Tile('B'), new Tile('C'), new Tile('D') });

            // Act
            var resultTileBag = player.DrawTiles(tileBag);

            // Assert
            Assert.Equal(7, player.Rack.TileCount); // 3 existing + 4 drawn (to fill capacity)
            Assert.Equal(0, resultTileBag.Count); // 4 original - 4 drawn
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
