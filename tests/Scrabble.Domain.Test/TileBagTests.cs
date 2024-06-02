using System;
using System.Collections.Generic;
using Xunit;

namespace Scrabble.Domain.Tests
{
    public class TileBagTests
    {
        [Fact]
        public void TileBag_Initialization_CorrectTileCount()
        {
            // Arrange
            var tileBag = new TileBag();

            // Act
            int actualTileCount = tileBag.Count;

            // Assert
            Assert.Equal(100, actualTileCount); // Assuming the total tiles should be 100
        }

        [Fact]
        public void TileBag_DrawTiles_DecreasesCount()
        {
            // Arrange
            var tileBag = new TileBag();
            int initialCount = tileBag.Count;

            // Act
            var (drawnTiles, nextTileBag) = tileBag.DrawTiles(5);

            // Assert
            Assert.Equal(5, drawnTiles.Count);
            Assert.Equal(initialCount - 5, nextTileBag.Count);
        }

        [Fact]
        public void TileBag_DrawTiles_ThrowsException_WhenDrawingTooManyTiles()
        {
            // Arrange
            var tileBag = new TileBag();

            // Act & Assert
            var exception = Assert.Throws<Exception>(() => tileBag.DrawTiles(101));
            Assert.Equal("Attempt to draw more tiles than present in TileBag", exception.Message);
        }

        [Fact]
        public void TileBag_DrawTiles_DrawsExpectedTiles()
        {
            // Arrange
            var tileBag = new TileBag();
            tileBag.Shuffle(); 

            // Act
            var (drawnTiles, _) = tileBag.DrawTiles(7);

            // Assert
            Assert.Equal(7, drawnTiles.Count);
        }

        [Fact]
        public void TileBag_Shuffle_ChangesTileOrder()
        {
            // Arrange
            var tileBag = new TileBag();
            var initialTiles = new List<Tile>(tileBag.FindAll(t => true)); // Copy initial order

            // Act
            tileBag.Shuffle();
            var shuffledTiles = tileBag.FindAll(t => true);

            // Assert
            Assert.NotEqual(initialTiles, shuffledTiles); // This test might occasionally fail due to rare chance of the shuffle resulting in the same order
        }

        [Fact]
        public void TileBag_FindAll_ReturnsMatchingTiles()
        {
            // Arrange
            var tileBag = new TileBag();

            // Act
            var tiles = tileBag.FindAll(t => t.Letter == 'A');

            // Assert
            Assert.Equal(9, tiles.Count);
        }
    }
}