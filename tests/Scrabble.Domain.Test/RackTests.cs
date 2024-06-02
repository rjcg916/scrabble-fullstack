using System;
using System.Collections.Generic;
using System.Linq;
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
            var tilesToAdd = new List<Tile> { new('A'), new('B') };

            // Act
            var addedToRack = rack.AddTiles(tilesToAdd);

            // Assert
            Assert.Equal(2, addedToRack.TileCount);
        }

        [Fact]
        public void Rack_AddTiles_ThrowsException_WhenExceedingCapacity()
        {
            // Arrange
            var rack = new Rack();
            var tilesToAdd = new List<Tile>
            {
                new('A'), new('B'), new('C'),
                new('D'), new('E'), new('F'),
                new('G'), new('H')
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
            var initialTiles = new List<Tile> { new('A'), new('B') };
            var addedToRack = rack.AddTiles(initialTiles);
            var tilesToRemove = new List<Tile> { new('A') };

            // Act
            var removedFromRack = addedToRack.RemoveTiles(tilesToRemove);

            // Assert
            Assert.Equal(1, removedFromRack.TileCount);
        }

        [Fact]
        public void Rack_RemoveTiles_IgnoresMissingTileInRemovalRequest()
        {
            // Arrange
            var emptyRack = new Rack();
            var initialTiles = new List<Tile> { new('A'), new('C') };
            var rack = emptyRack.AddTiles(initialTiles);
            var tilesToRemove = new List<Tile> { new('A'), new('B') };

            // Act 
            var rackAfterRemoval = rack.RemoveTiles(tilesToRemove);

            // Assert
            Assert.Equal(1, rackAfterRemoval.TileCount);
            var aTile = rackAfterRemoval.GetTiles().FirstOrDefault(t => t == new Tile('C'));
            Assert.Null(aTile);

        }

        [Fact]
        public void Rack_InRack_ReturnsTrue_IfTileIsInRack()
        {
            // Arrange
            var rack = new Rack();
            var tilesToAdd = new List<Tile> { new('A') };
            var nextRack = rack.AddTiles(tilesToAdd);

            // Act
            bool inRack = nextRack.InRack('A');

            // Assert
            Assert.True(inRack);
        }

        [Fact]
        public void Rack_InRack_ReturnsFalse_IfTileIsNotInRack()
        {
            // Arrange
            var rack = new Rack();
            var tilesToAdd = new List<Tile> { new('A') };
            var nextRack = rack.AddTiles(tilesToAdd);

            // Act
            bool inRack = nextRack.InRack('B');

            // Assert
            Assert.False(inRack);
        }
    }
}
