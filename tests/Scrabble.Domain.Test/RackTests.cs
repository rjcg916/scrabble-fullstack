using Scrabble.Domain.Model;
using System.Collections.Generic;
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
            Assert.Equal([], r.GetTiles());

        }
        [Fact]
        public void RackAddTiles()
        {
            // Arrange
            var r = new Rack();
            //  var tb = new TileBag();

            var tiles = new List<Tile>()
            {
                new('A'),
                new('B'),
                new('C'),
                new('D'),
                new('E'),
                new('F'),
                new('G')
            };

            // Act
            r.AddTiles(tiles);

            // Assert
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
                new('A'),
                new('B'),
                new('C'),
                new('D'),
                new('E'),
                new('F'),
                new('G')
            };
            r.AddTiles(tiles);

            var tilesToRemove = new List<Tile>()
            {
                new('A'),
                new('C'),
                new('E'),
                new('G')
            };


            // Act

            r.RemoveTiles(tilesToRemove);


            // Assert
            Assert.NotNull(r.GetTiles());
            Assert.Equal(3, r.TileCount);



        }

        [Fact]
        public void GetEmptySlots()
        {
            // Arrange
            var r = new Rack();

            var slotCount = r.TileCount;
            
            Assert.Equal(0, slotCount);

        }

        [Fact]
        public void GetSlotsWithTiles()
        {
            // Arrange
            var r = new Rack();

            // Act
            var tiles = new List<Tile>()
            {
                new('A'),
                new('B'),
                new('C'),
                new('D'),
                new('E'),
                new('F')
            };
            r.AddTiles(tiles);


            // Assert
            Assert.True(r.InRack('B'));
            Assert.False(r.InRack('Z'));

        }


    }
}
