using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Scrabble.Domain
{
    public enum SquareType
    {
        reg, dl, tl, dw, tw, start
    }

    public static class SquareExtensions
    {

        static public List<char>ToCharList(this List<Square> slice ) =>
            slice.Select(square => square.Tile?.Letter ?? ' ').ToList();

        static public int ScoreRun(this List<Square> squares)
        {
            int score = 0;

            int cumulativeWordMultiplier = 1;

            foreach (var location in squares)
            {
                score += (location.Tile.Value * location.LetterMultiplier);
                cumulativeWordMultiplier *= location.WordMultiplier;                       
            }

            return score * cumulativeWordMultiplier;
        }
    }

    public class Square(SquareType squareType = SquareType.reg)
    {
        public Square Copy()
        {
            return new Square
            {
                Tile = this.Tile,
                SquareType = this.SquareType,
                MoveOfOccupation = this.MoveOfOccupation,
                IsFinal = this.IsFinal             
            };
        }
        public SquareType SquareType { get; set; } = squareType;

        public bool IsFinal { get; set; } = false;
        public Tile Tile { get; set; }

        public int MoveOfOccupation { get; set; } = 0;

        public bool IsOccupied
        {
            get
            {
                return Tile != null;
            }
        }

        public int LetterMultiplier => SquareType switch
        {
            SquareType.tl => 3,
            SquareType.dl => 2,
            _ => 1
        };

        public int WordMultiplier => SquareType switch
        {
            SquareType.tw => 3,
            SquareType.dw => 2,
            _ => 1
        };

    }
}