using System;
using System.Collections.Generic;
using System.Linq;

namespace Scrabble.Domain
{
    public interface IMove
    {
        Span FindParallelRun();
        Span FindPerpendicularRun(Coord coord);
        List<Span> FindAllPerpendicularRuns();
    }

    public abstract class Move(Board board, Coord start, string letters)
    {
        protected List<Tile> Tiles = letters.LettersToTiles();
        protected ushort Length = (ushort)letters.Length;
        protected Coord StartCoord = start;
        protected Coord EndCoord;
        protected Board Board = board;

        public Coord GetEndCoord() 
            => EndCoord;
    }

    public class HorizontalMove : Move, IMove
    {
        public HorizontalMove(Board board, Coord start, string letters)
            : base(board, start, letters)
        {
            EndCoord = new Coord(start.Row, (C) ((ushort)start.Col + Length));
        }

        public Span FindParallelRun()
        {
            var slice = Board.GetHorizontalSlice((ushort)StartCoord.Row);
            var endpoints = Util.GenerateRun(slice, (ushort) StartCoord.Col, (ushort) EndCoord.Col);

            if (endpoints == null)
                return null;

            var startCoord = new Coord(StartCoord.Row, (C) endpoints.Value.Start);
            var endCoord = new Coord(StartCoord.Row, (C )endpoints.Value.End);

            return new Span { Start = startCoord, End = endCoord };
        }

        public Span FindPerpendicularRun(Coord coord)
        {
            var slice = Board.GetVerticalSlice((ushort)coord.Col);
            var endpoints = Util.GenerateRun(slice, (ushort) coord.Row, (ushort) coord.Row);

            if (endpoints == null)
                return null;

            var startCoord = new Coord((R)endpoints.Value.Start, coord.Col);
            var endCoord = new Coord((R)endpoints.Value.End, coord.Col);

            return new Span { Start = startCoord, End = endCoord };
        }

        public List<Span> FindAllPerpendicularRuns()
        {
            var runs = new List<Span>();
            for (ushort c = (ushort)StartCoord.Col; c <= (ushort)EndCoord.Col; c++)
            {
                var run = FindPerpendicularRun(new Coord(StartCoord.Row,(C) c));
                if (run != null)
                    runs.Add(run);
            }
            return runs;
        }
    }

    public class VerticalMove : Move, IMove
    {
        public VerticalMove(Board board, Coord start, string letters)
            : base(board, start, letters)
        {
            EndCoord = new Coord((R) (((ushort)start.Row) + Length), start.Col);
        }

        public Span FindParallelRun()
        {
            var slice = Board.GetVerticalSlice((ushort)StartCoord.Col);
            var endpoints = Util.GenerateRun(slice, (ushort)StartCoord.Row, (ushort)EndCoord.Row);

            if (endpoints == null)
                return null;

            return new Span
            {
                Start = new Coord((R)endpoints.Value.Start, StartCoord.Col),
                End = new Coord((R)endpoints.Value.End, StartCoord.Col)
            };
        }

        public Span FindPerpendicularRun(Coord coord)
        {
            var slice = Board.GetHorizontalSlice((ushort)coord.Row);
            var endpoints = Util.GenerateRun(slice, (ushort)coord.Col, (ushort)coord.Col);

            if (endpoints == null)
                return null;

            return new Span
            {
                Start = new Coord(coord.Row, (C)endpoints.Value.Start),
                End = new Coord(coord.Row, (C)endpoints.Value.End)
            };
        }

        public List<Span> FindAllPerpendicularRuns()
        {
            var runs = new List<Span>();
            for (ushort r = (ushort)StartCoord.Row; r <= (ushort)EndCoord.Row; r++)
            {
                var run = FindPerpendicularRun(new Coord((R) r, StartCoord.Col));
                if (run != null)
                    runs.Add(run);
            }
            return runs;
        }
    }

    public static class Util
    {
        public static List<Tile> LettersToTiles(this string letters)
        {
            ArgumentNullException.ThrowIfNull(letters);

            return letters.Select(letter => new Tile(letter)).ToList();
        }

        public static (ushort Start, ushort End)? GenerateRun(Square[] slice, ushort start, ushort end)
        {
            // Implementation for generating run endpoints.
            throw new NotImplementedException();
        }
    }

    //public class Coord
    //{
    //    public ushort Row { get; set; }
    //    public ushort Col { get; set; }

    //    public Coord(ushort row, ushort col)
    //    {
    //        Row = row;
    //        Col = col;
    //    }
    //}

    public class Span
    {
        public Coord Start { get; set; }
        public Coord End { get; set; }
    }

}
