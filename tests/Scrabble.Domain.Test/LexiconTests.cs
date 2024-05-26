using Scrabble.Domain.Model;

using Xunit;

namespace Scrabble.Domain.Test
{
    public class LexiconTests
    {
        [Fact]
        public void FindValidWord()
        {
            // Arrange
            var l = new Lexicon();

            // Act


            // Assert

            Assert.True(l.IsWordValid("House"));

        }
        [Fact]
        public void RejectInvalidWord()
        {
            // Arrange
            var l = new Lexicon();

            // Act


            // Assert

            Assert.False(l.IsWordValid("Sport"));

        }



    }
}
