using ScrabbleLib.Model;
using System.Collections.Generic;
using Xunit;

namespace ScrabbleLibTest
{
    public class TileBagServiceTests
    {
        [Fact]
        public void TileBagCorrect()
        {
            // Arrange
            var tbs = new TileBagService();

            // Act
            var tb = tbs.GetTileBag();

            // Assert
            // tiles generated
            Assert.Equal(100, tb.Count);
            Assert.Equal(6, tb.FindAll(t => t.Letter == "R").Count);

        }


    }
}
