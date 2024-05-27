using System;
using Xunit;


namespace Scrabble.Domain.Tests
{
    public class PlayerTests
    {
        [Fact]
        public void Player_Constructor_InitializesPropertiesCorrectly()
        {
            // Arrange
            var tileBag = new TileBag();
            string playerName = "TestPlayer";

            // Act
            var player = new Player(playerName, tileBag);

            // Assert
            Assert.Equal(playerName, player.Name);
            Assert.NotNull(player.Rack);
            Assert.InRange(player.Rack.TileCount, 0, Rack.Capacity); // Ensure some tiles are drawn
        }

        [Fact]
        public void Player_DrawTiles_ThrowsException_WhenNoTilesAvailable()
        {
            // Arrange
            var emptyTileBag = new TileBag();
            emptyTileBag.DrawTiles(emptyTileBag.Count); // Empty the tile bag

            // Act & Assert
            var exception = Assert.Throws<Exception>(() => new Player("TestPlayer", emptyTileBag));
            Assert.Equal("No tiles available to draw.", exception.Message);
        }

        [Fact]
        public void Player_DrawTiles_FillsRackUpToCapacity()
        {
            // Arrange
            var tileBag = new TileBag();
            var player = new Player("TestPlayer", tileBag);

            // Act
            player.DrawTiles(tileBag);

            // Assert
            Assert.Equal(Rack.Capacity, player.Rack.TileCount);
        }

        [Fact]
        public void Player_DrawTiles_DrawsAllAvailableTiles_WhenLessThanCapacity()
        {
            // Arrange
            var tileBag = new TileBag();
            var player = new Player("TestPlayer", tileBag);
            var tilesLeftInBag = tileBag.Count;

            // Draw all but a few tiles from the bag
            tileBag.DrawTiles(tilesLeftInBag - 3);

            // Remove two tile from Rack
            player.Rack.RemoveTiles(2);

            // Act
            player.DrawTiles(tileBag);

            // Assert
            Assert.Equal(Rack.Capacity, player.Rack.TileCount);
        }

        [Fact]
        public void Player_PlaceTile_ReturnsTrue_WhenPlacementIsSuccessful()
        {
            // Arrange
            var tileBag = new TileBag();
            var player = new Player("TestPlayer", tileBag);
            var board = new Board();
            var coord = new Coord(R._1, C.A);
            var tile = new Tile('A', 1);

            // Act
            var result = player.PlaceTile(board, coord, tile);

            // Assert
            Assert.True(result);
            Assert.Equal(tile, board.GetTile(coord));
        }

        [Fact]
        public void Player_PlaceTile_ReturnsFalse_WhenPlacementFails()
        {
            // Arrange
            var tileBag = new TileBag();
            var player = new Player("TestPlayer", tileBag);
            var board = new Board();
            var coord = new Coord(R._1, C.A);
            var tile1 = new Tile('A', 1);
            var tile2 = new Tile('B', 3);

            board.PlaceTile(coord, tile1); // Place the first tile

            // Act
            var result = player.PlaceTile(board, coord, tile2); // Attempt to place another tile on the same square

            // Assert
            Assert.False(result);
            Assert.Equal(tile1, board.GetTile(coord)); // Ensure the original tile is still there
        }
    }
}
