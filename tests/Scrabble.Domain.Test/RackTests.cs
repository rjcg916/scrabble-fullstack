using System.Collections.Generic;

using Scrabble.Domain.Model;

using Xunit;

namespace Scrabble.Domain.Test
{
    public class RackTests
    {
        [Fact]
        public void RackDefaultsSaved()
        {
            // Arrange
            var r = new Rack();

            // Act

            // Assert
            // start square correctly set
            Assert.Equal([], r.GetTiles());

        }
        [Fact]
        public void RackAddTiles()
        {
            // Arrange
            var r = new Rack();
            var tb = new TileBag();

            var tiles = new List<Tile>()
            {
                new("A"),
                new("B"),
                new("C"),
                new("D"),
                new("E"),
                new("F"),
                new("G")
            };

            // Act
            r.AddTiles(tiles);

            // Assert
            // start square correctly set
            Assert.NotNull(r.GetTiles());
            Assert.Equal(7, r.TileCount);

        }

        [Fact]
        public void RackRemoveTiles()
        {
            // Arrange
            var r = new Rack();


            var tiles = new List<Tile>()
            {
                new("A"),
                new("B"),
                new("C"),
                new("D"),
                new("E"),
                new("F"),
                new("G")
            };
            r.AddTiles(tiles);

            var tilesToRemove = new List<Tile>()
            {
                new("A"),
                new("C"),
                new("E"),
                new("G")
            };


            // Act

            r.RemoveTiles(tilesToRemove);


            // Assert
            // start square correctly set
            Assert.NotNull(r.GetTiles());
            Assert.Equal(3, r.TileCount);



        }

        [Fact]
        public void GetEmptySlots()
        {
            // Arrange
            var r = new Rack();

            var slots = r.GetSlots();

            Assert.NotNull(slots);
            Assert.Equal(7, slots.Length);

        }

        [Fact]
        public void GetSlotsWithTiles()
        {
            // Arrange
            var r = new Rack();

            // Act
            var tiles = new List<Tile>()
            {
                new("A"),
                new("B"),
                new("C"),
                new("D"),
                new("E"),
                new("F")
            };
            r.AddTiles(tiles);

            var slots = r.GetSlots();

            // Assert
            Assert.NotNull(slots);
            Assert.Equal(7, slots.Length);
            Assert.Equal("B", slots[1].tile.Letter);
            Assert.Null(slots[6].tile);

        }


    }
}
