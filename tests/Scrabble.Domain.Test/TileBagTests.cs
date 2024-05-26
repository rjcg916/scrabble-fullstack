using Scrabble.Domain.Model;

using Xunit;

namespace Scrabble.Domain.Test
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
