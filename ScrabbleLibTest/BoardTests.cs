﻿using ScrabbleLib.Model;
using Xunit;

namespace ScrabbleLibTest
{
    public class BoardTests
    {
        [Fact]
        public void BoardDefaultsSaved()
        {
            // Arrange
            var b = new Board();

            // Act

            // Assert
            // start square correctly set
            Assert.Equal(SquareType.start, b.GetSquare(new Coord(R._8, C._H)).SquareType);
            Assert.Null(b.GetTile(new Coord(R._8, C._H)));


        }

        [Fact]
        public void PlaceTile()
        {
            // Arrange
            var b = new Board();
            var t = new Tile("A");
            var c = new Coord(R._8, C._H);

            // Act
            b.PlaceTile(new TileLocation(c, t));

            // Assert
            Assert.Equal<Tile>(t, b.GetTile(c));


        }

        [Fact]
        public void CantPlaceTileInOccupiedSquare()
        {
            // Arrange
            var b = new Board();
            var t1 = new Tile("A");
            var t2 = new Tile("B");

            var c = new Coord(R._8, C._H);

            // Act
            var result1 = b.PlaceTile(new TileLocation(c, t1));
            var result2 = b.PlaceTile(new TileLocation(c, t2));

            // Assert
            Assert.False(result2);
            Assert.Equal<Tile>(t1, b.GetTile(c));



        }



        [Fact]
        public void GetOccupiedSquareList()
        {
            // Arrange
            var b = new Board();
            var t1 = new Tile("A");
            var t2 = new Tile("B");

            var c1 = new Coord(R._8, C._H);
            var c2 = new Coord(R._9, C._H);

            // Act
            var result1 = b.PlaceTile(new TileLocation(c1, t1));
            var result2 = b.PlaceTile(new TileLocation(c2, t2));

            // Assert
            var list = b.GetCoordSquares(filterForOccupied: true);
            Assert.Equal(2, list.Count);
            var arrayList = list.ToArray();
            Assert.Equal("A", arrayList[0].Square.Tile.Letter);
            Assert.Equal("B", arrayList[1].Square.Tile.Letter);


        }

        [Fact]
        public void GetSquareList()
        {
            // Arrange
            var b = new Board();

            // Act

            // Assert
            var list = b.GetCoordSquares();
            Assert.Equal(15 * 15, list.Count);

        }
    }
}
