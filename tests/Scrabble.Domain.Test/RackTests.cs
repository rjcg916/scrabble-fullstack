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
            Assert.Equal(new List<Tile>(), r.GetTiles());

        }
        [Fact]
        public void RackAddTiles()
        {
            // Arrange
            var r = new Rack();
            var tb = new TileBag();

            var tiles = new List<Tile>()
            {
                new Tile("A"),
                new Tile("B"),
                new Tile("C"),
                new Tile("D"),
                new Tile("E"),
                new Tile("F"),
                new Tile("G")
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
                new Tile("A"),
                new Tile("B"),
                new Tile("C"),
                new Tile("D"),
                new Tile("E"),
                new Tile("F"),
                new Tile("G")
            };
            r.AddTiles(tiles);

            var tilesToRemove = new List<Tile>()
            {
                new Tile("A"),
                new Tile("C"),
                new Tile("E"),
                new Tile("G")
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
                new Tile("A"),
                new Tile("B"),
                new Tile("C"),
                new Tile("D"),
                new Tile("E"),
                new Tile("F")
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
