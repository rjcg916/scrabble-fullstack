namespace Scrabble.Domain.Model
{
    public enum R
    {
        _1, _2, _3, _4, _5, _6, _7, _8,
        _9, _10, _11, _12, _13, _14, _15
    }

    public enum C
    {
        _A, _B, _C, _D, _E, _F, _G, _H,
        _I, _J, _K, _L, _M, _N, _O
    }
    public class Coord(R row, C col)
    {
        public R row { get; } = row;
        public C col { get; } = col;
    }

    public class Span
    {
        Coord start { get; }
        Coord end { get; }

        Span(Coord start, Coord end) {
            this.start = start;
            this.end = end;
        }
    }

    public class Endpoints
    {
        int start { get; }
        int end { get; }

        Endpoints(int start, int end)
        {
            this.start = start;
            this.end = end;
        }
    }
}