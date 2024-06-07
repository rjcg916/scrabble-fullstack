using System.Collections.Generic;
using Xunit;

namespace Scrabble.Domain.Tests
{
    public class BoardTests
    {
        private static bool MockWordValidator(string word) => true;

        [Fact]
        public void Board_Initialization()
        {
            // Arrange & Act
            var board = new Board(MockWordValidator);

            // Assert
            Assert.Equal(Board.rowCount, board.squares.GetLength(0));
            Assert.Equal(Board.colCount, board.squares.GetLength(1));
        }

        [Fact]
        public void Board_Copy()
        {
            // Arrange
            var board = new Board(MockWordValidator);
            var originalTile = new Tile('A');
            board.squares[0, 0].Tile = originalTile;

            // Act
            var copiedBoard = board.Copy();

            // Assert
            Assert.Equal(originalTile, copiedBoard.squares[0, 0].Tile);
            Assert.NotSame(board.squares, copiedBoard.squares);
        }

        [Fact]
        public void Board_IsFirstMove()
        {
            // Arrange
            var board = new Board(MockWordValidator);

            // Act & Assert
            Assert.True(board.IsFirstMove());
        }

        [Fact]
        public void Board_IsOccupied()
        {
            // Arrange
            var board = new Board(MockWordValidator);
            var coord = new Coord(R._1, C.A);
            board.squares[coord.RowValue, coord.ColValue].Tile = new Tile('A');

            // Act & Assert
            Assert.True(board.IsOccupied(coord));
        }

        [Fact]
        public void Board_DoesMoveTouchStart()
        {
            // Arrange
            var tiles = new List<(Coord, Tile)>
            {
                (new Coord(R._8, C.H), new Tile('A'))
            };

            // Act
            var result = Board.DoesMoveTouchStar(tiles);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Board_AreAllTilesContiguous()
        {
            // Arrange
            var board = new Board(MockWordValidator);
            var tiles = new List<(Coord, Tile)>
            {
                (new Coord(R._8, C.H), new Tile('A')),
                (new Coord(R._8, C.I), new Tile('B')),
                (new Coord(R._8, C.J), new Tile('C'))
            };

            // Act
            var result = board.AreAllTilesContiguous(tiles);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Board_PlaceTiles()
        {
            // Arrange
            var board = new Board(MockWordValidator);
            var tiles = new List<(Coord, Tile)>
            {
                (new Coord(R._8, C.H), new Tile('A')),
                (new Coord(R._8, C.I), new Tile('B')),
                (new Coord(R._8, C.J), new Tile('C'))
            };

            // Act
            board.PlaceTiles(tiles);

            // Assert
            Assert.Equal('A', board.squares[(int)R._8, (int)C.H].Tile.Letter);
            Assert.Equal('B', board.squares[(int)R._8, (int)C.I].Tile.Letter);
            Assert.Equal('C', board.squares[(int)R._8, (int)C.J].Tile.Letter);
        }

        [Fact]
        public void Board_IsBoardValid()
        {
            // Arrange
            static bool wordValidator(string word) => word == "AB" || word == "CD";
            var board = new Board(wordValidator);

            // Place valid words
            var tiles = new List<(Coord, Tile)>
            {
                (new Coord(R._8, C.H), new Tile('A')),
                (new Coord(R._8, C.I), new Tile('B'))
            };
            board.PlaceTiles(tiles);

            // Act
            var (isValid, invalidMessage) = board.IsBoardValid();

            // Assert
            Assert.True(isValid);
            Assert.Null(invalidMessage);
        }

        [Fact]
        public void Board_IsBoardInvalid()
        {
            // Arrange
            static bool wordValidator(string word) => word == "AB";
            var board = new Board(wordValidator);

            // Place an invalid word
            var tiles = new List<(Coord, Tile)>
            {
                (new Coord(R._8, C.H), new Tile('A')),
                (new Coord(R._8, C.I), new Tile('C'))
            };
            board.PlaceTiles(tiles);

            // Act
            var (isValid, invalidMessage) = board.IsBoardValid();

            // Assert
            Assert.False(isValid);
            Assert.Equal([(Placement.Horizontal, (int)R._8, "AC")], invalidMessage);
        }
    }
}
