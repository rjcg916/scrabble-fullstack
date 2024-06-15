using System;
using System.Collections.Generic;
using System.Drawing;

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
        static private readonly int RowCount = 15;
        static private readonly int ColCount = 15;

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

        // Check the four adjacent squares (up, down, left, right)
        public List<Coord> GetAdjacent()
            =>
            [
                new((R)( Math.Max( RowValue - 1, 0)), Col), // Up
                new((R)( Math.Min( RowValue + 1, RowCount - 1) ), Col), // Down
                new(Row, (C)( Math.Max(ColValue - 1, 0))), // Left
                new(Row, (C)( Math.Min(ColValue + 1, ColCount - 1)))  // Right
            ];

        public bool IsValidCoord() =>
            RowValue >= 0 && RowValue < RowCount &&
                        ColValue >= 0 && ColValue < ColCount;
    }
    public record LocationSquare (Coord Coord, Square Square);

}