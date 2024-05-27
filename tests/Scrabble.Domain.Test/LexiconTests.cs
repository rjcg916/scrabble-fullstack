using Xunit;

namespace Scrabble.Domain.Tests
{
    public class LexiconTests
    {
        [Fact]
        public void IsWordValid_ReturnsTrue_ForValidWord()
        {
            // Arrange
            var lexicon = new Lexicon();

            // Act
            bool result = lexicon.IsWordValid("Car");

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsWordValid_ReturnsFalse_ForInvalidWord()
        {
            // Arrange
            var lexicon = new Lexicon();

            // Act
            bool result = lexicon.IsWordValid("Bike");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsWordValid_ReturnsTrue_ForAnotherValidWord()
        {
            // Arrange
            var lexicon = new Lexicon();

            // Act
            bool result = lexicon.IsWordValid("Talent");

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsWordValid_ReturnsFalse_ForEmptyString()
        {
            // Arrange
            var lexicon = new Lexicon();

            // Act
            bool result = lexicon.IsWordValid(string.Empty);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsWordValid_IsCaseInsensitive()
        {
            // Arrange
            var lexicon = new Lexicon();

            // Act
            bool resultLowerCase = lexicon.IsWordValid("car");
            bool resultMixedCase = lexicon.IsWordValid("cAr");

            // Assert
            Assert.True(resultLowerCase);
            Assert.True(resultMixedCase);
        }

        [Fact]
        public void IsWordValid_ReturnsFalse_ForWhitespaceOnly()
        {
            // Arrange
            var lexicon = new Lexicon();

            // Act
            bool result = lexicon.IsWordValid("   ");

            // Assert
            Assert.False(result);
        }
    }
}
