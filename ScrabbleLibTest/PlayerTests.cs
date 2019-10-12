using ScrabbleLib.Model;
using System.Collections.Generic;
using Xunit;

namespace ScrabbleLibTest
{
    public class PlayerTests
    {
        [Fact]
        public void PlayerInitialRack()
        {
            // Arrange
            var p = new Player("player 1");
            var tb = new TileBag();

            // Act
            p.DrawTiles(tb);
                

            // Assert            
            Assert.Equal(7, p.rack.TileCount);

        }

    }
}
