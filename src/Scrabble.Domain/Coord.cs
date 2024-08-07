﻿using System;

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

        public Coord(int row, int col) : this((R) row, (C) col)
        { }
        
        public static  R[] Rows => (R[])Enum.GetValues(typeof(R));
        public static  C[] Cols => (C[])Enum.GetValues(typeof(C));

        public int RVal => (int) Row;
        public int CVal => (int) Col;

        public override string ToString()
        {
            return $"{RVal}{Col}";            
        }
        public string ToDisplayString()
        {
            return $"{RVal+1}{Col}";
        }

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

        public override int GetHashCode()
        {
            return HashCode.Combine(Row, Col);
        }

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