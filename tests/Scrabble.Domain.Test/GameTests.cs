using Scrabble.Domain.Model;
using System;
using System.Collections.Generic;
using Xunit;

namespace Scrabble.Domain.Test
{
    public class GameTests
    {
        [Fact]
        public void GameDefaultsSaved()
        {
            // Arrange
            var gameNames = new List<String> { "player1", "player2" };

            var g = new Game(gameNames);


            // Act

            // Assert


            Assert.False(g.GameDone);
            Assert.Equal(2, g.NumberOfPlayers);
            Assert.NotNull(g.Board);
            Assert.NotNull(g.Players[1]);
            Assert.Equal(86, g.RemainingTileCount);
            Assert.Equal(1, g.TurnOfPlayer);
            Exception ex = Assert.Throws<System.Collections.Generic.KeyNotFoundException>(() => g.Players[3]);
            Assert.Equal("The given key '3' was not present in the dictionary.", ex.Message);

        }

        [Fact]
        public void GamePlayersSpecified()
        {
            // Arrange
            var gameNames = new List<String> { "player1", "player2", "player3", "player4" };
            var g = new Game(gameNames);


            // Act

            // Assert
            Assert.False(g.GameDone);
            Assert.Equal(4, g.NumberOfPlayers);
            Assert.NotNull(g.Players[3]);

        }


    }
}
