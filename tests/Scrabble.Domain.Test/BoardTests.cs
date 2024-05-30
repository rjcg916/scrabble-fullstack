using System.Collections.Generic;
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

            var newTile = new Tile('A', 1);
            _board.PlaceTile(coord, newTile);
            tile = _board.GetTile(coord);
            Assert.Equal(newTile, tile);
        }

        [Fact]
        public void IsOccupied_ReturnsCorrectStatus()
        {
            var coord = new Coord(R._1, C.A);
            Assert.False(_board.IsOccupied(coord));

            _board.PlaceTile(coord, new Tile('A', 1));
            Assert.True(_board.IsOccupied(coord));
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

            _board.PlaceTile(new Coord(R._1, C.A), new Tile('A', 1));
            squares = _board.GetCoordSquares(true);
            Assert.Single(squares);
        }

        [Fact]
        public void PlaceTile_ReturnsCorrectStatus()
        {
            var coord = new Coord(R._1, C.A);
            var tile = new Tile('A', 1);

            Assert.True(_board.PlaceTile(coord, tile));
            Assert.False(_board.PlaceTile(coord, new Tile('B', 2)));
        }

        [Fact]
        public void ValidSequence_ReturnsCorrectValidation()
        {
            var validWordList = new List<string> { "Hello", "World" };
            bool isWordValid(string word) => validWordList.Contains(word);

            var validSequence = new List<char> { 'H', 'e', 'l', 'l', 'o', ' ', 'W', 'o', 'r', 'l', 'd' };
            var invalidSequence = new List<char> { 'H', 'e', 'l', 'l', 'o', ' ', 'X', 'y', 'z' };

            var result = Board.ValidSequence(validSequence, isWordValid);
            Assert.True(result.valid);
            Assert.Equal('\0', result.invalidChar);

            result = Board.ValidSequence(invalidSequence, isWordValid);
            Assert.False(result.valid);
            Assert.Equal('X', result.invalidChar);
        }
    }
}
