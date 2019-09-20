using ScrabbleLib.Model;
using Xunit;

namespace ScrabbleLibTest
{
    public class SquareTests
    {
        [Fact]
        public void SquareDefaultsSaved()
        {
            // Arrange
            var s = new Square();

            // Act

            // Assert
            Assert.False(s.IsFinal);
            Assert.False(s.IsOccupied);
            Assert.Equal<SquareType>(SquareType.reg, s.SquareType);
            Assert.Equal(1, s.LetterMultiplier);
            Assert.Equal(1, s.WordMultiplier);
            Assert.Null(s.Tile);

        }

        [Fact]
        public void SquareContainingTile()
        {
            // Arrange
            var s = new Square();
            var t = new Tile("A");

            // Act
            s.Tile = t;

            // Assert
            Assert.Equal<Tile>(t, s.Tile);
            Assert.Equal("A", s.Tile.Letter);

        }

    }
}
