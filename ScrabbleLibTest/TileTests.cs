﻿using ScrabbleLib.Model;
using Xunit;

namespace ScrabbleLibTest
{
    public class TileTests
    {
        [Fact] 
        public void LetterIsCapitalized()
        {
            // Arrange
            var t = new Tile("a");

            // Act

            // Assert
            Assert.Equal("A", t.Letter);

        }

        [Fact]
        public void DefaultValueStored()
        {
            // Arrange
            var t = new Tile("B");

            // Act

            // Assert
            Assert.Equal(1, t.Value);

        }

        [Fact]
        public void ExplictValueStored()
        {
            // Arrange
            var t = new Tile("B", 3);

            // Act

            // Assert
            Assert.Equal(3, t.Value);

        }

    }
}
