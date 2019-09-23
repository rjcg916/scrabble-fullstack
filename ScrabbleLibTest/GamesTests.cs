using ScrabbleLib.Model;
using System;
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
            var index = factory.CreateGame(4);
            var g = factory.GetGame(index);


            // Act

            // Assert
            Assert.False(g.gameDone);
            Assert.Equal(4, g.numberOfPlayers);
            Assert.NotNull(g.players[3]);

        }


    }
}
