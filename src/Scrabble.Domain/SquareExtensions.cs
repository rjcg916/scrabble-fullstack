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

        //public static Endpoints GenerateSecondaryRun(this List<Square> slice, ushort square) =>
        //    GenerateRun(slice, square, square);

        //public static Endpoints GenerateRun(this List<Square> slice, ushort startSquare, ushort endSquare)
        //{
        //    ushort firstSquare = startSquare;

        //    if (startSquare > 0)
        //    {
        //        firstSquare = 0;
        //        for (ushort l = (ushort)(startSquare - 1); l >= 0; l--)
        //        {
        //            if (!slice[l].IsOccupied)
        //            {
        //                firstSquare = (ushort) (l + 1);
        //                break;
        //            }
        //        }
        //    }

        //    ushort lastSquare = endSquare;

        //    if (endSquare < slice.Count - 1)
        //    {
        //        lastSquare = (ushort) (slice.Count - 1);
        //        for (ushort r = (ushort)(endSquare + 1); r <= slice.Count - 1; r++)
        //        {
        //            if (!slice[r].IsOccupied)
        //            {
        //                lastSquare = (ushort) (r - 1);
        //                break;
        //            }
        //        }
        //    }

        //    if (firstSquare == startSquare && lastSquare == endSquare)
        //        return default;

        //    return new Endpoints(firstSquare, lastSquare);
        //}

     
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