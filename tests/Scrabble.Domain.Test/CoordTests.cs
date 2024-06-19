using System;
using System.Collections.Generic;
using Xunit;

namespace Scrabble.Domain.Tests
{
    public class CoordTests
    {
        [Fact]
        public void Constructor_ShouldInitializeProperties()
        {
            // Arrange
            var row = R._5;
            var col = C.E;

            // Act
            var coord = new Coord(row, col);

            // Assert
            Assert.Equal(row, coord.Row);
            Assert.Equal(col, coord.Col);
        }

        [Fact]
        public void Rows_ShouldContainAllRowValues()
        {
            // Arrange
            var expectedRows = (R[])Enum.GetValues(typeof(R));

            // Act
            var rows = Coord.Rows;

            // Assert
            Assert.Equal(expectedRows, rows);
        }

        [Fact]
        public void Cols_ShouldContainAllColValues()
        {
            // Arrange
            var expectedCols = (C[])Enum.GetValues(typeof(C));

            // Act
            var cols = Coord.Cols;

            // Assert
            Assert.Equal(expectedCols, cols);
        }

        [Fact]
        public void ToString_ShouldReturnCorrectStringRepresentation()
        {
            // Arrange
            var coord = new Coord(R._5, C.E);

            // Act
            var result = coord.ToString();

            // Assert
            Assert.Equal("5E", result);
        }

        [Fact]
        public void GetAdjacent_ShouldReturnCorrectAdjacentCoordinates()
        {
            // Arrange
            var coord = new Coord(R._5, C.E);

            // Act
            var adjacent = coord.GetAdjacent();

            // Assert
            var expectedAdjacent = new List<Coord>
            {
                new(R._4, C.E), // Up
                new(R._6, C.E), // Down
                new(R._5, C.D), // Left
                new(R._5, C.F)  // Right
            };

            Assert.Equal(expectedAdjacent, adjacent);
        }

        [Fact]
        public void IsValidCoord_ShouldReturnTrueForValidCoord()
        {
            // Arrange
            var coord = new Coord(R._5, C.E);

            // Act
            var isValid = coord.IsValidCoord();

            // Assert
            Assert.True(isValid);
        }

        [Fact]
        public void IsValidCoord_ShouldReturnFalseForInvalidRow()
        {
            // Arrange
            var coord = new Coord((R)20, C.E);

            // Act
            var isValid = coord.IsValidCoord();

            // Assert
            Assert.False(isValid);
        }

        [Fact]
        public void IsValidCoord_ShouldReturnFalseForInvalidColumn()
        {
            // Arrange
            var coord = new Coord(R._5, (C)20);

            // Act
            var isValid = coord.IsValidCoord();

            // Assert
            Assert.False(isValid);
        }

        [Fact]
        public void Equals_ShouldReturnTrueForEqualCoords()
        {
            // Arrange
            var coord1 = new Coord(R._5, C.E);
            var coord2 = new Coord(R._5, C.E);

            // Act
            var areEqual = coord1.Equals(coord2);

            // Assert
            Assert.True(areEqual);
        }

        [Fact]
        public void Equals_ShouldReturnFalseForDifferentCoords()
        {
            // Arrange
            var coord1 = new Coord(R._5, C.E);
            var coord2 = new Coord(R._6, C.E);

            // Act
            var areEqual = coord1.Equals(coord2);

            // Assert
            Assert.False(areEqual);
        }

        [Fact]
        public void GetHashCode_ShouldReturnSameHashCodeForEqualCoords()
        {
            // Arrange
            var coord1 = new Coord(R._5, C.E);
            var coord2 = new Coord(R._5, C.E);

            // Act
            var hash1 = coord1.GetHashCode();
            var hash2 = coord2.GetHashCode();

            // Assert
            Assert.Equal(hash1, hash2);
        }

        [Fact]
        public void EqualityOperator_ShouldReturnTrueForEqualCoords()
        {
            // Arrange
            var coord1 = new Coord(R._5, C.E);
            var coord2 = new Coord(R._5, C.E);

            // Act
            var areEqual = coord1 == coord2;

            // Assert
            Assert.True(areEqual);
        }

        [Fact]
        public void InequalityOperator_ShouldReturnTrueForDifferentCoords()
        {
            // Arrange
            var coord1 = new Coord(R._5, C.E);
            var coord2 = new Coord(R._6, C.E);

            // Act
            var areNotEqual = coord1 != coord2;

            // Assert
            Assert.True(areNotEqual);
        }
    }
}
