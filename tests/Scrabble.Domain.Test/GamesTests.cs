using Scrabble.Domain.Model;
using System;
using System.Collections.Generic;
using Xunit;

namespace Scrabble.Domain.Test
{
    public class GamesTests
    {
        [Fact]
        public void GameCreateInCollection()
        {
            // Arrange
            var gameNames = new List<String> { "player1", "player2", "player3", "player4" };
            var factory = new Game.GameFactory();

            // Act
            var g = factory.CreateGame(gameNames);

            // Assert
            Assert.False(g.GameDone);
            Assert.Equal(4, g.NumberOfPlayers);
            Assert.NotNull(g.Players[3]);

        }

        [Fact]
        public void GameRemoveFromCollection()
        {
            // Arrange
            var gameManager = new GameManager();

            // var factory = new Game.GameFactory();

            var gameNames = new List<String> { "player1", "player2", "player3", "player4" };


            var id1 = gameManager.CreateGame(gameNames);
            var id2 = gameManager.CreateGame(gameNames);
          
            // Act
            var result = gameManager.RemoveGame(id1);

            // Assert

            var g1 = gameManager.GetGame(id1);  
            var g2 = gameManager.GetGame(id2);
            Assert.True(result);
            Assert.Null(g1);
            Assert.Equal(1, gameManager.NumberOfGames());
            Assert.NotNull(g2);
            Assert.False(g2.GameDone);
            Assert.Equal(4, g2.NumberOfPlayers);
            Assert.NotNull(g2.Players[3]);

        }

    }
}
