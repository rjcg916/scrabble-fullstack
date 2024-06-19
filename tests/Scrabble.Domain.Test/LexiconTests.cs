using System.Collections.Generic;
using Xunit;

namespace Scrabble.Domain.Tests
{
    public class LexiconTests
    {
        private readonly Lexicon _lexicon;

        public LexiconTests()
        {
            _lexicon = new Lexicon();
        }

        [Fact]
        public void IsWordValid_ReturnsTrue_ForExistingWord()
        {
            Assert.True(_lexicon.IsWordValid("Car"));
            Assert.True(_lexicon.IsWordValid("House"));
            Assert.True(_lexicon.IsWordValid("Dog"));
            Assert.True(_lexicon.IsWordValid("Talent"));
        }

        [Fact]
        public void IsWordValid_ReturnsFalse_ForNonExistingWord()
        {
            Assert.False(_lexicon.IsWordValid("Cat"));
            Assert.False(_lexicon.IsWordValid("Building"));
            Assert.False(_lexicon.IsWordValid("Elephant"));
        }

        [Fact]
        public void IsWordValid_IgnoresCase()
        {
            Assert.True(_lexicon.IsWordValid("car"));
            Assert.True(_lexicon.IsWordValid("HOUSE"));
            Assert.True(_lexicon.IsWordValid("dOg"));
            Assert.True(_lexicon.IsWordValid("tAlEnT"));
        }

        [Fact]
        public void IsWordValid_ReturnsFalse_ForEmptyString()
        {
            Assert.False(_lexicon.IsWordValid(string.Empty));
        }

        [Fact]
        public void IsWordValid_ReturnsFalse_ForNullString()
        {
            Assert.False(_lexicon.IsWordValid(null));
        }

        [Fact]
        public void IValidSequence_ReturnsCorrectValidation()
        {
            var validWordList = new List<string> { "Hello", "World" };
            bool isWordValid(string word) => validWordList.Contains(word);

            var validSequence = new List<char> { 'H', 'e', 'l', 'l', 'o', ' ', 'W', 'o', 'r', 'l', 'd' };
            var invalidSequence = new List<char> { 'H', 'e', 'l', 'l', 'o', ' ', 'X', 'y', 'z' };

            var (valid, blankWord) = validSequence.IsValidWordList(isWordValid);
            Assert.True(valid);
            Assert.Equal("", blankWord);

            var (validFalse, invalidWord) = invalidSequence.IsValidWordList(isWordValid);
            Assert.False(validFalse);
            Assert.Equal("Xyz", invalidWord);
        }
    }
}
