using System.Collections.Generic;

namespace Scrabble.Domain
{
    public enum Placement
    {
        Horizontal, Vertical, Star
    }

    public record PlacementError(Placement Type, int Location, string Letters);
    public record PlacementSpec(Placement Placement, int FixedLocation, List<(int, Tile)> TileLocations);
    public record TilePlacement(Coord Coord, Tile Tile);
}