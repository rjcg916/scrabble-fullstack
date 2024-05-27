using Scrabble.Domain;
using System.Collections.Generic;
using System;
using Xunit;

namespace Scrabble.Domain.Tests
{
  
    public class BoardTests
    {
        [Fact]
        public void Board_Constructor_InitializesBoardCorrectly()
        {
            // Arrange & Act
            var board = new Board();

            // Assert
            foreach (var r in BoardHelper.GetRows() )
            {
                foreach(var c in BoardHelper.GetColumns())
                {
                    Assert.NotNull(board.GetSquare(new Coord((R)r, (C)c)));
                }
            }
        }

        [Fact]
        public void GetCoordSquares_ReturnsAllSquares_WhenFilterForOccupiedIsFalse()
        {
            // Arrange
            var board = new Board();

            // Act
            var coordSquares = board.GetCoordSquares(false);

            // Assert
            Assert.Equal(225, coordSquares.Count); // 15x15 board
        }

        [Fact]
        public void GetCoordSquares_ReturnsOnlyOccupiedSquares_WhenFilterForOccupiedIsTrue()
        {
            // Arrange
            var board = new Board();
            var coord = new Coord(R._9, C.E);
            board.PlaceTile(coord, new Tile('A', 1));

            // Act
            var coordSquares = board.GetCoordSquares(true);

            // Assert
            Assert.Single(coordSquares);
            Assert.Equal(((ushort)coord.Row), coordSquares[0].Row);
            Assert.Equal(((ushort)coord.Col), coordSquares[0].Col);
            Assert.Equal('A', coordSquares[0].Evaluator.Tile.Letter);
        }

        [Fact]
        public void GetSquare_ReturnsCorrectSquare()
        {
            // Arrange
            var board = new Board();
            var coord = new Coord(R._1, C.B);

            // Act
            var square = board.GetSquare(coord);

            // Assert
            Assert.NotNull(square);
            Assert.Equal(SquareType.reg, square.SquareType);
        }

        [Fact]
        public void GetTile_ReturnsCorrectTile()
        {
            // Arrange
            var board = new Board();
            var coord = new Coord(R._4, C.B);
            var tile = new Tile('A', 1);
            board.PlaceTile(coord, tile);

            // Act
            var retrievedTile = board.GetTile(coord);

            // Assert
            Assert.Equal(tile, retrievedTile);
        }

        [Fact]
        public void IsOccupied_ReturnsTrue_WhenSquareIsOccupied()
        {
            // Arrange
            var board = new Board();
            var coord = new Coord(R._10, C.G);
            board.PlaceTile(coord, new Tile('A', 1));

            // Act
            var isOccupied = board.IsOccupied(coord);

            // Assert
            Assert.True(isOccupied);
        }

        [Fact]
        public void IsOccupied_ReturnsFalse_WhenSquareIsNotOccupied()
        {
            // Arrange
            var board = new Board();
            var coord = new Coord(R._1, C.A);

            // Act
            var isOccupied = board.IsOccupied(coord);

            // Assert
            Assert.False(isOccupied);
        }

        [Fact]
        public void PlaceTile_ReturnsFalse_WhenSquareIsAlreadyOccupied()
        {
            // Arrange
            var board = new Board();
            var coord = new Coord(R._1, C.C);
            board.PlaceTile(coord, new Tile('A', 1));

            // Act
            var result = board.PlaceTile(coord, new Tile('B', 3));

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void PlaceTile_ReturnsTrue_WhenSquareIsNotOccupied()
        {
            // Arrange
            var board = new Board();
            var coord = new Coord(R._1, C.C);

            // Act
            var result = board.PlaceTile(coord, new Tile('A', 1));

            // Assert
            Assert.True(result);
        }
    }
}
