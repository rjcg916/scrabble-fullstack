using Scrabble.Domain.Model;
using System.Collections.Generic;
using Xunit;

namespace ScrabbleLibTest
{
    public class TileBagTests
    {
        [Fact]
        public void TileBagCorrect()
        {
            // Arrange

            var tb = new TileBag();

            // Assert
            // tiles generated
            Assert.Equal(100, tb.count);
            Assert.Equal(6, tb.FindAll(t => t.Letter == "R").Count);

        }

    }
}
