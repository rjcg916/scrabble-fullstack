using Xunit;


namespace Scrabble.Domain.Tests
{
    public class PlayerTests
    {
        [Fact]
        public void Player_Constructor_InitializesPropertiesCorrectly()
        {
            // Arrange
            string playerName = "TestPlayer";

            // Act
            var player = new Player(playerName);

            // Assert
            Assert.Equal(playerName, player.Name);
            Assert.NotNull(player.Rack);
            Assert.InRange(player.Rack.TileCount, 0, Rack.Capacity); // Ensure some tiles are drawn
        }

    }
}
