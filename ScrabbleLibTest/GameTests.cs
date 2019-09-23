using ScrabbleLib.Model;
using System;
using Xunit;

namespace ScrabbleLibTest
{
    public class GameTests
    {
        [Fact]
        public void GameDefaultsSaved()
        {
            // Arrange
            var g = new Game();


            // Act

            // Assert
            Assert.False(g.gameDone);
            Assert.Equal(2, g.numberOfPlayers);
            Assert.NotNull(g.players[1]);
            Exception ex = Assert.Throws<System.Collections.Generic.KeyNotFoundException>(() => g.players[3] );
            Assert.Equal("The given key '3' was not present in the dictionary.", ex.Message);

        }

        [Fact]
        public void GamePlayersSpecified()
        {
            // Arrange
            var g = new Game(4);


            // Act

            // Assert
            Assert.False(g.gameDone);
            Assert.Equal(4, g.numberOfPlayers);
            Assert.NotNull(g.players[3]);

        }


    }
}
