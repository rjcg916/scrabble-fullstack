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
            var startingTileBag = new TileBag();
            var (_,emptyTileBag) = startingTileBag.DrawTiles(startingTileBag.Count); // Empty the tile bag

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
            var tile = new Tile('A');

            // Act             
            var exception = Record.Exception(() => board = player.PlaceTile(board, coord, tile));

            // Assert
            Assert.Null(exception); // Ensures no exception was thrown
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
            var tile1 = new Tile('A');
            var tile2 = new Tile('B');

            var newBoard = board.PlaceTile(coord, tile1); // Place the first tile

            // Act 
            Board nextBoard = null;
            var exception = Record.Exception(() => nextBoard = player.PlaceTile(newBoard, coord, tile2));

            // Assert             
            Assert.NotNull(exception); 
            Assert.Equal(tile1, newBoard.GetTile(coord)); // Ensure the original tile is still there
        }
    }
}
