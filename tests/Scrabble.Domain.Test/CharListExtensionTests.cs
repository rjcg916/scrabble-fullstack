using System.Collections.Generic;
using Xunit;

namespace Scrabble.Domain.Tests
{
    public class CharListExtensionTests
    {

        [Fact]
        public void IValidSequence_ReturnsCorrectValidation()
        {
            var validWordList = new List<string> { "Hello", "World" };
            bool isWordValid(string word) => validWordList.Contains(word);

            var validSequence = new List<char> { 'H', 'e', 'l', 'l', 'o', ' ', 'W', 'o', 'r', 'l', 'd' };
            var invalidSequence = new List<char> { 'H', 'e', 'l', 'l', 'o', ' ', 'X', 'y', 'z' };

            var (valid, blankWord) = validSequence.IsValidSequence(isWordValid);
            Assert.True(valid);
            Assert.Equal("", blankWord);

            var (validFalse, invalidWord) = invalidSequence.IsValidSequence(isWordValid);
            Assert.False(validFalse);
            Assert.Equal("Xyz", invalidWord);
        }
    }
}
