using System;
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
        public const int RowCount = 15;
        public const int ColCount = 15;

        public R Row { get; init; } = row;
        public C Col { get; init; } = col;

        public static  R[] Rows => (R[])Enum.GetValues(typeof(R));
        public static  C[] Cols => (C[])Enum.GetValues(typeof(C));

        public int RVal => (int) Row;

        public int CVal => (int) Col;

        public override string ToString()
        {
            return $"{RVal}{Col}";            
        }

        public List<Coord> GetAdjacent()
            =>
            [
                new((R)( Math.Max( RVal - 1, 0)), Col), // Up
                new((R)( Math.Min( RVal + 1, RowCount - 1) ), Col), // Down
                new(Row, (C)( Math.Max(CVal - 1, 0))), // Left
                new(Row, (C)( Math.Min(CVal + 1, ColCount - 1)))  // Right
            ];

        public bool IsValidCoord() =>
            RVal >= 0 && RVal < RowCount &&
            CVal >= 0 && CVal < ColCount; 
        
        
        // Override Equals
        public override bool Equals(object obj)
        {
            if (obj is Coord other)
            {
                return Row.Equals(other.Row) && Col.Equals(other.Col);
            }
            return false;
        }

        // Override GetHashCode
        public override int GetHashCode()
        {
            return HashCode.Combine(Row, Col);
        }

        // Implement equality operators
        public static bool operator ==(Coord left, Coord right)
        {
            if (left is null)
            {
                return right is null;
            }
            return left.Equals(right);
        }

        public static bool operator !=(Coord left, Coord right)
        {
            return !(left == right);
        }
    }
}