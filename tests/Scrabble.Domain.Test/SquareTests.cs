using Xunit;

namespace Scrabble.Domain.Tests
{
    public class SquareTests
    {
        [Fact]
        public void Square_DefaultConstructor_SetsDefaultValues()
        {
            // Arrange
            var square = new Square();

            // Assert
            Assert.Equal(SquareType.reg, square.SquareType);
            Assert.False(square.IsFinal);
            Assert.Null(square.Tile);
            Assert.False(square.IsOccupied);
            Assert.Equal(1, square.LetterMultiplier);
            Assert.Equal(1, square.WordMultiplier);
        }

        [Fact]
        public void Square_Constructor_SetsSquareType()
        {
            // Arrange
            var square = new Square(SquareType.dl);

            // Assert
            Assert.Equal(SquareType.dl, square.SquareType);
        }

        [Fact]
        public void Square_IsOccupied_ReturnsFalse_WhenTileIsNull()
        {
            // Arrange
            var square = new Square();

            // Act
            bool isOccupied = square.IsOccupied;

            // Assert
            Assert.False(isOccupied);
        }

        [Fact]
        public void Square_IsOccupied_ReturnsTrue_WhenTileIsNotNull()
        {
            // Arrange
            var square = new Square
            {
                Tile = new Tile('A', 1)
            };

            // Act
            bool isOccupied = square.IsOccupied;

            // Assert
            Assert.True(isOccupied);
        }

        [Fact]
        public void Square_LetterMultiplier_ReturnsCorrectValue()
        {
            // Arrange & Act
            var regSquare = new Square(SquareType.reg);
            var dlSquare = new Square(SquareType.dl);
            var tlSquare = new Square(SquareType.tl);

            // Assert
            Assert.Equal(1, regSquare.LetterMultiplier);
            Assert.Equal(2, dlSquare.LetterMultiplier);
            Assert.Equal(3, tlSquare.LetterMultiplier);
        }

        [Fact]
        public void Square_WordMultiplier_ReturnsCorrectValue()
        {
            // Arrange & Act
            var regSquare = new Square(SquareType.reg);
            var dwSquare = new Square(SquareType.dw);
            var twSquare = new Square(SquareType.tw);

            // Assert
            Assert.Equal(1, regSquare.WordMultiplier);
            Assert.Equal(2, dwSquare.WordMultiplier);
            Assert.Equal(3, twSquare.WordMultiplier);
        }
    }
}
