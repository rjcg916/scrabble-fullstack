using System;
using System.Collections.Generic;
using Xunit;

namespace Scrabble.Domain.Tests
{
    public class HorizontalMoveTests
    {
        private readonly Board _board;

        public HorizontalMoveTests()
        {
            _board = new Board();
        }

        [Fact]
        public void HorizontalMove_ValidMove_DoesNotThrowException()
        {
            var validWordList = new List<string> { "hello", "world" };
            bool isWordValid(string word) => validWordList.Contains(word.ToLower());

            var startCoord = new Coord(R._1, C.A);
            var move = new HorizontalMove(_board, "hello", isWordValid, startCoord);

            Assert.NotNull(move); // Test passes if no exception is thrown
        }

        [Fact]
        public void HorizontalMove_InvalidMove_ThrowsInvalidOperationException()
        {
            var validWordList = new List<string> { "hello", "world" };
            bool isWordValid(string word) => validWordList.Contains(word.ToLower());

            var startCoord = new Coord(R._1, C.A);
            _board.PlaceTile(new Coord(R._1, C.B), new Tile('x', 1));

            Assert.Throws<InvalidOperationException>(() =>
            {
                var move = new HorizontalMove(_board, "hello", isWordValid, startCoord);
            });
        }
    }

    public class VerticalMoveTests
    {
        private readonly Board _board;

        public VerticalMoveTests()
        {
            _board = new Board();
        }

        [Fact]
        public void VerticalMove_ValidMove_DoesNotThrowException()
        {
            var validWordList = new List<string> { "hello", "world" };
            bool isWordValid(string word) => validWordList.Contains(word.ToLower());

            var startCoord = new Coord(R._1, C.A);
            var move = new VerticalMove(_board, "hello", isWordValid, startCoord);

            Assert.NotNull(move); // Test passes if no exception is thrown
        }

        [Fact]
        public void VerticalMove_InvalidMove_ThrowsInvalidOperationException()
        {
            var validWordList = new List<string> { "hello", "world" };
            bool isWordValid(string word) => validWordList.Contains(word.ToLower());

            var startCoord = new Coord(R._1, C.A);
            _board.PlaceTile(new Coord(R._2, C.A), new Tile('x', 1));

            Assert.Throws<InvalidOperationException>(() =>
            {
                var move = new VerticalMove(_board, "hello", isWordValid, startCoord);
            });
        }
    }
}
