using System;
using System.Linq;
using Xunit;

namespace Scrabble.Domain.Tests
{
    public class TileDrawCountTests
    {
        [Fact]
        public void TileDrawCount_ValidCount()
        {
            // Arrange
            int validCount = 7;

            // Act
            var drawCount = new TileDrawCount(validCount);

            // Assert
            Assert.Equal(validCount, drawCount.Value);
        }

        [Fact]
        public void TileDrawCount_InvalidCount_ThrowsArgumentException()
        {
            // Arrange
            int invalidCount = 1000;

            // Act & Assert
            Assert.Throws<ArgumentException>(() => new TileDrawCount(invalidCount));
        }
    }

    public class TileBagTests
    {
        [Fact]
        public void TileBag_InitialCount()
        {
            // Arrange
            var tileBag = TileBag.TileBagFactory.Create();

            // Act
            int initialCount = tileBag.Count;

            // Assert
            Assert.Equal(100, initialCount); // Total tiles should be 100
        }

        [Fact]
        public void TileBag_DrawTiles()
        {
            // Arrange
            var tileBag = TileBag.TileBagFactory.Create();
            var drawCount = new TileDrawCount(7);

            // Act
            var (drawnTiles, newTileBag) = tileBag.DrawTiles(drawCount);

            // Assert
            Assert.Equal(7, drawnTiles.Count);
            Assert.Equal(tileBag.Count - 7, newTileBag.Count);
        }

        [Fact]
        public void TileBag_DrawTooManyTiles_ThrowsException()
        {
            // Arrange
            var tileBag = TileBag.TileBagFactory.Create();
            do
            {
                (_, tileBag) = tileBag.DrawTiles(new TileDrawCount(Rack.Capacity - 1)); 
            } while (tileBag.Count >= Rack.Capacity); 
            // tileBag.Count < Rack.Capacity

            var drawCount = new TileDrawCount(Rack.Capacity); // count to fill empty rack 

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => tileBag.DrawTiles(drawCount));
            Assert.Contains($"Attempt to draw more tiles {drawCount.Value} than present in TileBag", exception.Message);
        }

        [Fact]
        public void TileBag_ShuffleChangesOrder()
        {
            // Arrange
            var originalTileBag = TileBag.TileBagFactory.Create();

            // Act
            var shuffledTileBag  = originalTileBag.Shuffle();

            // Assert
            Assert.NotEqual(originalTileBag, shuffledTileBag); // Not guaranteed to be different but highly likely
        }

        [Fact]
        public void TileBag_FindAll()
        {
            // Arrange
            var tileBag = TileBag.TileBagFactory.Create();

            // Act
            var vowels = tileBag.Peek().FindAll(tile => "AEIOU".Contains(tile.Letter));

            // Assert
            Assert.True(vowels.All(tile => "AEIOU".Contains(tile.Letter)));
        }
    }
}