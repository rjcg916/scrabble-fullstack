using System;
using System.Collections.Generic;
using Xunit;

namespace Scrabble.Domain.Tests
{
    public class RackTests
    {
        [Fact]
        public void Rack_Initialization_TileCountIsZero()
        {
            // Arrange
            var rack = new Rack();

            // Act
            int tileCount = rack.TileCount;

            // Assert
            Assert.Equal(0, tileCount);
        }

        [Fact]
        public void Rack_Initialization_SlotCountIsCapacity()
        {
            // Arrange
            var rack = new Rack();

            // Act
            int slotCount = rack.SlotCount;

            // Assert
            Assert.Equal(Rack.Capacity, slotCount);
        }

        [Fact]
        public void Rack_AddTiles_IncreasesTileCount()
        {
            // Arrange
            var rack = new Rack();
            var tilesToAdd = new List<Tile> { new('A', 1), new('B', 3) };

            // Act
            rack.AddTiles(tilesToAdd);

            // Assert
            Assert.Equal(2, rack.TileCount);
        }

        [Fact]
        public void Rack_AddTiles_ThrowsException_WhenExceedingCapacity()
        {
            // Arrange
            var rack = new Rack();
            var tilesToAdd = new List<Tile>
            {
                new('A', 1), new('B', 3), new('C', 3),
                new('D', 2), new('E', 1), new('F', 4),
                new('G', 2), new('H', 4)
            };

            // Act & Assert
            var exception = Assert.Throws<Exception>(() => rack.AddTiles(tilesToAdd));
            Assert.Equal("Attempt to add tiles beyond rack capacity", exception.Message);
        }

        [Fact]
        public void Rack_RemoveTiles_DecreasesTileCount()
        {
            // Arrange
            var rack = new Rack();
            var initialTiles = new List<Tile> { new('A', 1), new('B', 3) };
            rack.AddTiles(initialTiles);
            var tilesToRemove = new List<Tile> { new('A', 1) };

            // Act
            rack.RemoveTiles(tilesToRemove);

            // Assert
            Assert.Equal(1, rack.TileCount);
        }

        [Fact]
        public void Rack_RemoveTiles_ThrowsException_WhenRemovingMoreThanExist()
        {
            // Arrange
            var rack = new Rack();
            var initialTiles = new List<Tile> { new('A', 1) };
            rack.AddTiles(initialTiles);
            var tilesToRemove = new List<Tile> { new('A', 1), new('B', 3) };

            // Act & Assert
            var exception = Assert.Throws<Exception>(() => rack.RemoveTiles(tilesToRemove));
            Assert.Equal("Attempt to remove more tiles than existing in rack.", exception.Message);
        }

        [Fact]
        public void Rack_InRack_ReturnsTrue_IfTileIsInRack()
        {
            // Arrange
            var rack = new Rack();
            var tilesToAdd = new List<Tile> { new('A', 1) };
            rack.AddTiles(tilesToAdd);

            // Act
            bool inRack = rack.InRack('A');

            // Assert
            Assert.True(inRack);
        }

        [Fact]
        public void Rack_InRack_ReturnsFalse_IfTileIsNotInRack()
        {
            // Arrange
            var rack = new Rack();
            var tilesToAdd = new List<Tile> { new('A', 1) };
            rack.AddTiles(tilesToAdd);

            // Act
            bool inRack = rack.InRack('B');

            // Assert
            Assert.False(inRack);
        }
    }
}
