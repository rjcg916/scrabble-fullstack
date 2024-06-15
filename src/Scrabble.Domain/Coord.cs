using System.Collections.Generic;

namespace Scrabble.Domain
{

    public enum R : int
    {
        _1 = 0, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15
    }

    public enum C : int
    {
        A = 0, B, C, D, E, F, G, H, I, J, K, L, M, N, O
    }

    public class Coord(R row, C col)
    {
        public R Row { get; init; } = row;
        public C Col { get; init; } = col;
    
        public int RowValue =>
            (int) Row;

        public int ColValue =>
            (int) Col;

        public override string ToString()
        {
            return $"{Col}{RowValue}";
        }
    }

    public record LocationSquare (Coord Coord, Square Square);

    //public class EvaluateAt<T>(int row, int col, T evaluator = default)
    //{
    //    public T Evaluator { get; set; } = evaluator;
    //    public int Row { get; set; } = row;
    //    public string RowName { get; set; } = ((R)row).ToString()[1..];
    //    public int Col { get; set; } = col;
    //    public string ColName { get; set; } = ((C)col).ToString()[0..];
    //}

  

    public static class CoordExtensions
    {

        //public static Placement GetPlacementType(this List<Tile> tiles, Coord start, Coord end)
        //{
        //    bool horizontal = start.Row == end.Row;
        //    int horizontalNumberOfTiles = end.Col - start.Col + 1;
        //    bool vertical = start.Col == end.Col;
        //    int verticalNumberOfTiles = end.Row - start.Row + 1;
        //    bool oneTile = horizontal && vertical;
        //    int numberOfTiles = tiles.Count;

        //    if (horizontal && vertical && !oneTile)
        //        throw new System.Exception("Unknown Placement Type");
        //    if (horizontal && horizontalNumberOfTiles == numberOfTiles)
        //        return Placement.Horizontal;
        //    else if (vertical && verticalNumberOfTiles == numberOfTiles)
        //        return Placement.Vertical;
        //    else 
        //        throw new System.Exception("Unsupported PlacementType");
            
        //}
    }
}