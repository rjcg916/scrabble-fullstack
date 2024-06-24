using System;
using System.Collections.Generic;
using Xunit;

namespace Scrabble.Domain.Tests
{
    public class MoveTests
    {
        [Fact]
        public void IsValidTilePlacement_WithTooManyTiles_ReturnsFalse()
        {
            var tiles = new List<TilePlacement>
            {
                new(new Coord(R._1, C.A), new Tile('A')),
                new(new Coord(R._1, C.B), new Tile('B')),
                new(new Coord(R._1, C.C), new Tile('C')),
                new(new Coord(R._1, C.D), new Tile('D')),
                new(new Coord(R._1, C.E), new Tile('E')),
                new(new Coord(R._1, C.F), new Tile('F')),
                new(new Coord(R._1, C.G), new Tile('G')),
                new(new Coord(R._1, C.H), new Tile('H'))
            };

            var (valid, msg) = Move.IsValidTilePlacement(tiles);

            Assert.False(valid);
            Assert.Equal("Move includes too many tiles", msg);
        }

        [Fact]
        public void IsValidTilePlacement_WithHorizontalMove_ReturnsTrue()
        {
            var tiles = new List<TilePlacement>
            {
                new(new Coord(R._1, C.A), new Tile('A')),
                new(new Coord(R._1, C.B), new Tile('B'))
            };

            var (valid, msg) = Move.IsValidTilePlacement(tiles);

            Assert.True(valid);
            Assert.Equal("Valid Move", msg);
        }

        [Fact]
        public void IsValidTilePlacement_WithVerticalMove_ReturnsTrue()
        {
            var tiles = new List<TilePlacement>
            {
                new(new Coord(R._1, C.A), new Tile('A')),
                new(new Coord(R._2, C.A), new Tile('B'))
            };

            var (valid, msg) = Move.IsValidTilePlacement(tiles);

            Assert.True(valid);
            Assert.Equal("Valid Move", msg);
        }

        [Fact]
        public void IsValidCoordTileList_WithHorizontalMoveOffBoard_ReturnsFalse()
        {
            var tiles = new List<Tile>
            {
                new('A'),
                new('B'),
                new('C'),
                new('D'),
                new('E'),
                new('F'),
                new('G')
            };

            var startFrom = new Coord(R._1, C.J);

            var (valid, msg) = MoveHorizontal.IsValidCoordTileList(startFrom, tiles);

            Assert.False(valid);
            Assert.Equal("Move off of board (Right)", msg);
        }

        [Fact]
        public void IsValidCoordTileList_WithVerticalMoveOffBoard_ReturnsFalse()
        {
            var tiles = new List<Tile>
            {
                new('A'),
                new('B'),
                new('C'),
                new('D'),
                new('E'),
                new('F'),
                new('G')
            };

            var startFrom = new Coord(R._10, C.A);

            var (valid, msg) = MoveVertical.IsValidCoordTileList(startFrom, tiles);

            Assert.False(valid);
            Assert.Equal("Move off of board (Bottom)", msg);
        }

        [Fact]
        public void MoveConstructor_WithValidTilePlacements_CreatesMove()
        {
            var tiles = new List<TilePlacement>
            {
                new(new Coord(R._1, C.A), new Tile('A')),
                new(new Coord(R._1, C.B), new Tile('B'))
            };

            var move = MoveFactory.CreateMove(tiles);

            Assert.NotNull(move.TilePlacements);
            Assert.Equal(2, move.TilePlacements.Count);
        }

        [Fact]
        public void MoveConstructor_WithInvalidTilePlacements_ThrowsException()
        {
            var tiles = new List<TilePlacement>
            {
                new(new Coord(R._1, C.A), new Tile('A')),
                new(new Coord(R._2, C.B), new Tile('B'))
            };

            var exception = Assert.Throws<Exception>(() => MoveFactory.CreateMove(tiles));

            Assert.Equal("Move is in both horizontal and vertical direction", exception.Message);
        }

        [Fact]
        public void MoveConstructor_WithValidCoordTileList_CreatesMove()
        {
            var tiles = new List<Tile>
            {
                new('A'),
                new('B')
            };

            var startFrom = new Coord(R._1, C.A);

            var move = MoveFactory.CreateMove(startFrom, tiles, true);

            Assert.NotNull(move.TilePlacements);
            Assert.Equal(2, move.TilePlacements.Count);
        }

        [Fact]
        public void MoveConstructor_WithInvalidCoordTileList_ThrowsException()
        {
            var tiles = new List<Tile>
            {
                new('A'),
                new('B')
            };

            var startFrom = new Coord(R._15, C.O);

            var exception = Assert.Throws<Exception>(() => MoveFactory.CreateMove(startFrom, tiles, true));

            Assert.Equal("Move off of board (Right)", exception.Message);
        }
    }
}
