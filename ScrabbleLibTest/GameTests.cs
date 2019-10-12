using ScrabbleLib.Model;
using System;
using System.Collections.Generic;
using Xunit;

namespace ScrabbleLibTest
{
    public class GameTests
    {
        [Fact]
        public void GameDefaultsSaved()
        {
            // Arrange
            var gameNames = new List<String> { "player1", "player2"};

            var g = new Game(gameNames);


            // Act

            // Assert
            
            
            Assert.False(g.gameDone);
            Assert.Equal(2, g.numberOfPlayers);
            Assert.NotNull(g.board);
            Assert.NotNull(g.players[1]);
            Assert.Equal(86, g.remainingTileCount);
            Assert.Equal(1, g.turnOfPlayer);
            Exception ex = Assert.Throws<System.Collections.Generic.KeyNotFoundException>(() => g.players[3] );
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
            Assert.False(g.gameDone);
            Assert.Equal(4, g.numberOfPlayers);
            Assert.NotNull(g.players[3]);

        }


    }
}
