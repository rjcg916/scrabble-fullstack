using System;
using Xunit;

namespace Scrabble.Domain.Tests
{
    public class BoardTests
    {
        private readonly Board _board;

        public BoardTests()
        {
            _board = new Board();
        }

        [Fact]
        public void GetSquare_ReturnsCorrectSquare()
        {
            var coord = new Coord(R._1, C.A);
            var square = _board.GetSquare(coord);
            Assert.NotNull(square);
        }

        [Fact]
        public void GetTile_ReturnsCorrectTile()
        {
            var coord = new Coord(R._1, C.A);
            var tile = _board.GetTile(coord);
            Assert.Null(tile);

            var newTile = new Tile('A');
            _board.PlaceTile(coord, newTile);
            tile = _board.GetTile(coord);
            Assert.Equal(newTile, tile);
        }



        [Fact]
        public void IsOccupied_SingleUnoccupiedCoord_ReturnsFalse()
        {
            var coord = new Coord(R._1, C.A);
            bool result = _board.IsOccupied(coord);
            Assert.False(result);
        }

        [Fact]
        public void IsOccupied_SingleOccupiedCoord_ReturnsTrue()
        {
            var coord = new Coord(R._1, C.A);
            _board.PlaceTile(coord, new Tile('A'));
            bool result = _board.IsOccupied(coord);
            Assert.True(result);
        }

        [Fact]
        public void IsOccupied_RowRange_Unoccupied_ReturnsFalse()
        {
            var startCoord = new Coord(R._1, C.A);
            var endCoord = new Coord(R._1, C.E);
            _board.PlaceTile(new Coord(R._1, C.F), new Tile('A'));
            bool result = _board.IsOccupied(startCoord, endCoord);
            Assert.False(result);
        }

        [Fact]
        public void IsOccupied_RowRange_Occupied_ReturnsTrue()
        {
            var startCoord = new Coord(R._1, C.A);
            var endCoord = new Coord(R._1, C.E);
            _board.PlaceTile(new Coord(R._1, C.C), new Tile('A'));
            bool result = _board.IsOccupied(startCoord, endCoord);
            Assert.True(result);
        }

        [Fact]
        public void IsOccupied_ColumnRange_Unoccupied_ReturnsFalse()
        {
            var startCoord = new Coord(R._1, C.A);
            var endCoord = new Coord(R._5, C.A);
            _board.PlaceTile(new Coord(R._6, C.A), new Tile('A'));
            bool result = _board.IsOccupied(startCoord, endCoord);
            Assert.False(result);
        }

        [Fact]
        public void IsOccupied_ColumnRange_Occupied_ReturnsTrue()
        {
            var startCoord = new Coord(R._1, C.A);
            var endCoord = new Coord(R._5, C.A);
            _board.PlaceTile(new Coord(R._3, C.A), new Tile('A'));
            bool result = _board.IsOccupied(startCoord, endCoord);
            Assert.True(result);
        }

        [Fact]
        public void IsOccupied_NonRowNonColumn_ThrowsNotImplementedException()
        {
            var startCoord = new Coord(R._1, C.A);
            var endCoord = new Coord(R._5, C.E);
            Assert.Throws<NotImplementedException>(() => _board.IsOccupied(startCoord, endCoord));
        }

        [Fact]
        public void GetRowSlice_ReturnsCorrectRow()
        {
            var row = _board.GetRowSlice((int)R._1);
            Assert.Equal(Board.colCount, row.Count);
        }

        [Fact]
        public void GetColumnSlice_ReturnsCorrectColumn()
        {
            var column = _board.GetColumnSlice((int)C.A);
            Assert.Equal(Board.rowCount, column.Count);
        }

        [Fact]
        public void GetCoordSquares_ReturnsCorrectSquares()
        {
            var squares = _board.GetCoordSquares();
            Assert.Equal(Board.rowCount * Board.colCount, squares.Count);

            _board.PlaceTile(new Coord(R._1, C.A), new Tile('A'));
            squares = _board.GetCoordSquares(true);
            Assert.Single(squares);
        }

        [Fact]
        public void PlaceTile_ReturnsCorrectStatus()
        {
            var coord = new Coord(R._1, C.A);
            var tile = new Tile('A');

            Assert.True(_board.PlaceTile(coord, tile));
            Assert.False(_board.PlaceTile(coord, new Tile('B')));
        }
    

        [Fact]
        public void Copy_CreatesDeepCopyOfBoard()
        {
            // Arrange
            var originalBoard = new Board();

            // Act
            var copiedBoard = originalBoard.Copy();

            // Assert
            // Verify that the boards are not the same instance
            Assert.NotSame(originalBoard, copiedBoard);

            // Verify that the internal board arrays are not the same instance
            for (int r = 0; r < Board.rowCount; r++)
            {
                for (int c = 0; c < Board.colCount; c++)
                {
                    Assert.NotSame(originalBoard.GetSquare(new Coord((R)r, (C)c)), copiedBoard.GetSquare(new Coord((R)r, (C)c)));
                }
            }

            // Verify that the contents are the same
            for (int r = 0; r < Board.rowCount; r++)
            {
                for (int c = 0; c < Board.colCount; c++)
                {
                    var originalSquare = originalBoard.GetSquare(new Coord((R)r, (C)c));
                    var copiedSquare = copiedBoard.GetSquare(new Coord((R)r, (C)c));

                    Assert.Equal(originalSquare.Tile, copiedSquare.Tile);
                    Assert.Equal(originalSquare.SquareType, copiedSquare.SquareType);
                }
            }
        }

        [Fact]
        public void Copy_ModifyingCopiedBoardDoesNotAffectOriginalBoard()
        {
            // Arrange
            var originalBoard = new Board();
            var copiedBoard = originalBoard.Copy();

            var coord = new Coord(R._8, C.H);
            var tile = new Tile('a'); 

            // Act
            copiedBoard.PlaceTile(coord, tile);

            // Assert
            // Verify that the original board's square is not occupied
            Assert.False(originalBoard.IsOccupied(coord));

            // Verify that the copied board's square is occupied
            Assert.True(copiedBoard.IsOccupied(coord));
            Assert.Equal(tile, copiedBoard.GetTile(coord));
        }
    }
}
