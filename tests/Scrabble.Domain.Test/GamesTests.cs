using Scrabble.Domain.Model;
using System;
using System.Collections.Generic;
using Xunit;

namespace ScrabbleLibTest
{
    public class GamesTests
    {
        [Fact]
        public void GameCreateInCollection()
        {
            // Arrange
            var factory = new Games();
            var gameNames = new List<String> { "player1", "player2", "player3", "player4" };
            var index = factory.CreateGame(gameNames);
            var g = factory.GetGame(index);


            // Act

            // Assert
            Assert.Equal(1, factory.Count());
            Assert.False(g.gameDone);
            Assert.Equal(4, g.numberOfPlayers);
            Assert.NotNull(g.players[3]);

        }

        [Fact]
        public void GameRemoveFromCollection()
        {
            // Arrange
            var factory = new Games();
           
            var gameNames = new List<String> { "player1", "player2", "player3", "player4" };
            
            var index = factory.CreateGame(gameNames);
            var g = factory.GetGame(index);
            var index2 = factory.CreateGame(gameNames);
            var g2 = factory.GetGame(index);

            // Act
            factory.RemoveGame(index);

            // Assert
            Assert.Null(factory.GetGame(index));
            Assert.Equal(1, factory.Count());
            Assert.NotNull(factory.GetGame(index2));
            Assert.False(g2.gameDone);
            Assert.Equal(4, g2.numberOfPlayers);
            Assert.NotNull(g2.players[3]);

        }

    }
}
