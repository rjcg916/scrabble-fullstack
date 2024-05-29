﻿namespace Scrabble.Domain
{
    class Letter(char name, ushort value)
    {
        public char Name { get; init; } = name;
        public ushort Value { get; init; } = value;
    }
}