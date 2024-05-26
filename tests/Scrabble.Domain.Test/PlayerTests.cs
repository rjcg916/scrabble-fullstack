using Scrabble.Domain.Model;

using Xunit;

namespace Scrabble.Domain.Test
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
            Assert.Equal(7, p.Rack.TileCount);

        }

    }
}
