using ScrabbleLib.Model;
using System.Collections.Generic;
using Xunit;

namespace ScrabbleLibTest
{
    public class TileBagTests
    {
        [Fact]
        public void TileBagCorrect()
        {
            // Arrange
            var tb = new TileBag();

            // Act
            var tiles = tb.GetTiles();

            // Assert
            // tiles generated
            Assert.Equal(100, tiles.Count);
            Assert.Equal(6, tiles.FindAll(t => t.Letter == "R").Count);

        }


    }
}
