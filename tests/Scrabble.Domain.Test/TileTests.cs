using System;
using Xunit;

namespace Scrabble.Domain.Tests
{
    public class TileTests
    {


        [Fact]
        public void Constructor_ShouldThrowException_ForInvalidCharacter()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentException>(() => new Tile('1'));
        }


        [Theory]
        [InlineData('A', 1)]
        [InlineData('B', 3)]
        [InlineData('C', 3)]
        [InlineData('D', 2)]
        [InlineData('E', 1)]
        [InlineData('F', 4)]
        [InlineData('G', 2)]
        [InlineData('H', 4)]
        [InlineData('I', 1)]
        [InlineData('J', 8)]
        [InlineData('K', 5)]
        [InlineData('L', 1)]
        [InlineData('M', 3)]
        [InlineData('N', 1)]
        [InlineData('O', 1)]
        [InlineData('P', 3)]
        [InlineData('Q', 10)]
        [InlineData('R', 1)]
        [InlineData('S', 1)]
        [InlineData('T', 1)]
        [InlineData('U', 1)]
        [InlineData('V', 4)]
        [InlineData('W', 4)]
        [InlineData('X', 8)]
        [InlineData('Y', 4)]
        [InlineData('Z', 10)]
        [InlineData('?', 0)]
        public void Tile_CorrectValue(char character, int expectedValue)
        {
            // Act
            Tile tile = new(character);

            // Assert
            Assert.Equal(expectedValue, tile.Value);
        }

        [Fact]
        public void Tile_InvalidCharacter_ThrowsArgumentException()
        {
            // Arrange
            char invalidCharacter = '1'; // Not a valid Scrabble tile character

            // Act & Assert
            Assert.Throws<ArgumentException>(() => new Tile(invalidCharacter).Value);
        }

        [Fact]
        public void Tile_UppercaseConversion()
        {
            // Arrange
            char input = 'a';
            char expected = 'A';

            // Act
            Tile tile = new(input);

            // Assert
            Assert.Equal(expected, tile.Letter);
        }

        [Fact]
        public void ChangeLetter_ShouldAllowChange_ForWildcardTile()
        {
            // Arrange
            var wildcardTile = new Tile('?');

            // Act
            wildcardTile.ChangeLetter('Z');

            // Assert
            Assert.Equal('Z', wildcardTile.Letter);
            Assert.Equal(0, wildcardTile.Value); 
        }

        [Fact]
        public void ChangeLetter_ShouldNotAllowChange_ForNonWildcardTile()
        {
            // Arrange
            var tile = new Tile('A');

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() => tile.ChangeLetter('B'));
            Assert.Equal("Only tiles with value 0 can be changed.", exception.Message);
            Assert.Equal('A', tile.Letter); // Ensure the letter hasn't changed
        }

        [Fact]
        public void Value_ShouldReturnCorrectValue_ForValidLetters()
        {
            // Arrange
            var tile = new Tile('K');

            // Act
            var value = tile.Value;

            // Assert
            Assert.Equal(5, value);
        }

        [Fact]
        public void Value_ShouldThrowException_ForInvalidLetters()
        {

            Assert.Throws<ArgumentException>(() => new Tile('1'));
        }
    }
}