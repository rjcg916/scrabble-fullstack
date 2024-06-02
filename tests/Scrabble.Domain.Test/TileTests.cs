using Xunit;

namespace Scrabble.Domain.Tests
{
    public class TileTests
    {
        [Fact]
        public void LetterIsCapitalized()
        {
            // Arrange
            var t = new Tile('a');

            // Act

            // Assert
            Assert.Equal('A', t.Letter);

        }

        [Fact]
        public void ValueStored()
        {
            // Arrange
            var t = new Tile('B');

            // Act

            // Assert
            Assert.Equal(3, t.Value);

        }

    }
}
