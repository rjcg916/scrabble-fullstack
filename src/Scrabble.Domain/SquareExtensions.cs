using System.Collections.Generic;

namespace Scrabble.Domain
{    public enum Placement
    {
        Horizontal,
        Vertical,
        Invalid
    }

    public static class SquareExtensions
    {

        public static Placement GetPlacementType(this List<Tile> tiles, Coord start, Coord end)
        {
            bool horizontal = start.Row == end.Row;
            int horizontalNumberOfTiles = end.Col - start.Col + 1;
            bool vertical = start.Col == end.Col;
            int verticalNumberOfTiles = end.Row - start.Row + 1;
            bool oneTile = horizontal && vertical;
            int numberOfTiles = tiles.Count;

            if (horizontal && vertical && !oneTile)
                return Placement.Invalid;
            else
            {
                if (horizontal && horizontalNumberOfTiles == numberOfTiles)
                    return Placement.Horizontal;
                else if (vertical && verticalNumberOfTiles == numberOfTiles)
                    return Placement.Vertical;
                else
                    return Placement.Invalid;
            }
        }
    }
}