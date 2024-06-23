using System.Collections.Generic;
using Xunit;

namespace Scrabble.Domain.Tests
{
    public class ScoreTests
    {
        static private bool MockWordValidator(string _) => true;

        [Fact]
        public void ScoreMove_AllBlanks_ReturnsCorrectScore()
        {
            //starting board

            var startFrom = new Coord(R._8, C.H);
            var tiles = new List<Tile> { new(' '), new(' '), new(' ') };

            var board = new Board(MockWordValidator, startFrom, tiles, isHorizontal: true);

            // score initial move
            var tilesAsPlacement = new List<TilePlacement>
            {
                new(new Coord(R._8, C.H), new Tile(' ')),
                new(new Coord(R._8, C.I), new Tile(' ')),
                new(new Coord(R._8, C.J), new Tile(' ')),
            };

            Assert.Equal(0, board.ScoreMove(tilesAsPlacement));
        }

        [Fact]
        public void ScoreMove_SomeBlanks_ReturnsCorrectScore()
        {
            //starting board

            var startFrom = new Coord(R._8, C.H);
            var tiles = new List<Tile> { new('A'), new(' '), new('Z') };

            var board = new Board(MockWordValidator, startFrom, tiles, isHorizontal: true);

            // score initial move
            var tilesAsPlacement = new List<TilePlacement>
            {
                new(new Coord(R._8, C.H), new Tile('A')),
                new(new Coord(R._8, C.I), new Tile(' ')),
                new(new Coord(R._8, C.J), new Tile('Z')),
            };

            Assert.Equal(11, board.ScoreMove(tilesAsPlacement));
        }

        [Fact]
        public void ScoreMove_InitialHorizontal_ReturnsCorrectScore()
        {
            //starting board
            var startFrom = new Coord(R._8, C.H);
            var tiles = new List<Tile> { new('B'), new('Z'), new('D') };

            var board = new Board(MockWordValidator, startFrom, tiles, isHorizontal: true);

            // score initial move
            var tilesAsPlacement = new List<TilePlacement>
            {
                new(new Coord(R._8, C.H), new Tile('B')),
                new(new Coord(R._8, C.I), new Tile('Z')),
                new(new Coord(R._8, C.J), new Tile('D')),
            };

            Assert.Equal(15, board.ScoreMove(tilesAsPlacement));

        }


        [Fact]
        public void ScoreMove_InitialHorizontalDoubleLetter_ReturnsCorrectScore()
        {
            //starting board

            var tiles = new List<Tile> { new('B'), new('C'), new('D'), new('B'), new('C'), new('D') };
            var startFrom = new Coord(R._8, C.H);
            var tileLocations = new List<int> { ((int)R._8), ((int)R._9), ((int)R._10), ((int)R._11), ((int)R._12), ((int)R._13) };


            var tilesAsPlacement = new List<TilePlacement>
            {
                new(new Coord(R._8, C.H), new Tile('B')),
                new(new Coord(R._8, C.I), new Tile('C')),
                new(new Coord(R._8, C.J), new Tile('D')),
                new(new Coord(R._8, C.K), new Tile('B')),
                new(new Coord(R._8, C.L), new Tile('C')),
                new(new Coord(R._8, C.M), new Tile('D')),
            };

            var board = new Board(MockWordValidator, startFrom, tiles, isHorizontal: true);

            // score initial move
            var expectedScore = 19;

            Assert.Equal(expectedScore, board.ScoreMove(tilesAsPlacement));
        }


        [Fact]
        public void ScoreMove_InitialVertical_ReturnsCorrectScore()
        {
            //starting board

            var tiles = new List<Tile> { new('B'), new('C'), new('D') };
            var startFrom = new Coord(R._7, C.H);
            var tileLocations = new List<int> { ((int)R._7), ((int)R._8), ((int)R._9) };

            var tilesAsPlacement = new List<TilePlacement>
            {
                new(new Coord(R._7, C.H), new Tile('B')),
                new(new Coord(R._8, C.H), new Tile('C')),
                new(new Coord(R._9, C.H), new Tile('D')),

            };

            var board = new Board(MockWordValidator, startFrom, tiles, isHorizontal: false);
            var boardAlt = new Board(MockWordValidator, tilesAsPlacement);

            // score initial move
            var expectedScore = 8;
            Assert.Equal(expectedScore, board.ScoreMove(tilesAsPlacement));
        }

        [Fact]
        public void ScoreMove_HorizontalNextExtends_ReturnsCorrectScore()
        {
            //starting board

            var tiles = new List<Tile> { new('B'), new('C'), new('D') };
            var startFrom = new Coord(R._8, C.H);

            var tilesAsPlacement = new List<TilePlacement>
            {
                new(new Coord(R._8, C.H), new Tile('B')),
                new(new Coord(R._8, C.I), new Tile('C')),
                new(new Coord(R._8, C.J), new Tile('D')),
            };

            var board = new Board(MockWordValidator, startFrom, tiles, isHorizontal: true);

            // score move
            var initialScore = board.ScoreMove(tilesAsPlacement);
            Assert.Equal(8, initialScore);

            // make a move
            var newTiles = new List<TilePlacement>
            {
                new(new Coord(R._8, C.G), new Tile('A')),
                new(new Coord(R._8, C.K), new Tile('J'))
            };

            board.MakeMove(newTiles);

            // score move
            var moveScore = board.ScoreMove(newTiles);

            Assert.Equal(17, moveScore);
        }

        [Fact]
        public void ScoreMove_VerticalNextExtends_ReturnsCorrectScore()
        {
            //starting board
            var board = new Board(MockWordValidator, new Coord(R._8, C.G), [new Tile('J')], isHorizontal: false);


            // make a move

            var tileList = new List<TilePlacement>
            {
                new(new Coord(R._7, C.G), new Tile('A')),
                new(new Coord(R._9, C.G), new Tile('Z'))
            };

            board.MakeMove(tileList);

            // score move

            var initialScore = board.ScoreMove(tileList);

            Assert.Equal(30, initialScore);

        }

        [Fact]
        public void ScoreMove_HorizontalNextParallel_ReturnsCorrectScore()
        {
            //starting board

            var tiles = new List<Tile> { new('B'), new('C'), new('D') };
            var startFrom = new Coord(R._8, C.H);

            var tilesAsPlacement = new List<TilePlacement>
            {
                new(new Coord(R._8, C.H), new Tile('B')),
                new(new Coord(R._8, C.I), new Tile('C')),
                new(new Coord(R._8, C.J), new Tile('D')),
            };

            var board = new Board(MockWordValidator, startFrom, tiles, isHorizontal: true);

            // score move
            var initialScore = board.ScoreMove(tilesAsPlacement);
            Assert.Equal(8, initialScore);


            // make a move
            var newTiles = new List<TilePlacement>
            {
                new(new Coord(R._9, C.J), new Tile('A')),
                new(new Coord(R._9, C.K), new Tile('J'))
            };

            board.MakeMove(newTiles);

            // score move
            var moveScore = board.ScoreMove(newTiles);

            Assert.Equal(12, moveScore);
        }

        [Fact]
        public void ScoreMove_HorizontalNextVertical_ReturnsCorrectScore()
        {
            //starting board

            var tiles = new List<Tile> { new('B'), new('C'), new('D') };
            var startFrom = new Coord(R._8, C.H);

            var tilesAsPlacement = new List<TilePlacement>
            {
                new(new Coord(R._8, C.H), new Tile('B')),
                new(new Coord(R._8, C.I), new Tile('C')),
                new(new Coord(R._8, C.J), new Tile('D')),
            };

            var board = new Board(MockWordValidator, startFrom, tiles, isHorizontal: true);

            // score move
            var initialScore = board.ScoreMove(tilesAsPlacement);
            Assert.Equal(8, initialScore);

            // make a move
            var newTiles = new List<TilePlacement>
            {
                new(new Coord(R._7, C.J), new Tile('A')),
                new(new Coord(R._9, C.J), new Tile('J'))
            };

            board.MakeMove(newTiles);

            // score move
            var moveScore = board.ScoreMove(newTiles);

            Assert.Equal(11, moveScore);
        }

        [Fact]
        public void ScoreMove_AlternateScorers_ReturnSameScore()
        {
            //starting board

            var tiles = new List<Tile> { new('B'), new('C'), new('D'), new('B'), new('C'), new('D') };
            var startFrom = new Coord(R._8, C.H);
            var tileLocations = new List<int> { ((int)R._8), ((int)R._9), ((int)R._10), ((int)R._11), ((int)R._12), ((int)R._13) };

            var tilesAsPlacement = new List<TilePlacement>
            {
                new(new Coord(R._8, C.H), new Tile('B')),
                new(new Coord(R._8, C.I), new Tile('C')),
                new(new Coord(R._8, C.J), new Tile('D')),
                new(new Coord(R._8, C.K), new Tile('B')),
                new(new Coord(R._8, C.L), new Tile('C')),
                new(new Coord(R._8, C.M), new Tile('D')),
            };

            var board = new Board(MockWordValidator, startFrom, tiles, isHorizontal: true);

            // score initial move
            var expectedScore = 19;
            Assert.Equal(expectedScore, board.ScoreMove(tilesAsPlacement));
           // Assert.Equal(expectedScore, board.ScoreMove(new Move(startFrom, tiles, isHorizontal: true));
        }

        [Fact]
        public void ScoreMove_CalculatesCorrectScore()
        {
            var board = new Board(MockWordValidator);

            List<TilePlacement> tiles = [
                    new (new Coord(R._8, C.H), new Tile('A')),
                    new (new Coord(R._8, C.I), new Tile('B')),
                    new (new Coord(R._8, C.J), new Tile('C')),
                    ];
            board.MakeMove(tiles);

            var score = board.ScoreMove(tiles);
            Assert.Equal(7, score);
        }
        [Fact]
        public void GetRun_ReturnsCorrectRunForHorizontal()
        {
            var board = new Board(MockWordValidator);

            List<TilePlacement> tiles =
                [
                new(new Coord(R._8, C.H), new Tile('A')),
                new(new Coord(R._8, C.I), new Tile('B'))
                ];
            board.MakeMove(tiles);

            var result = Score.GetEndpoints(board.SquareByColumn, (int)R._8, [(int)C.H, (int)C.I]);

            Assert.Equal(((int)C.H, (int)C.I), result);
        }

        [Fact]
        public void GetRun_ReturnsCorrectRunForVertical()
        {
            var board = new Board(MockWordValidator);

            List<TilePlacement> tiles =
            [
                new(new Coord(R._8, C.H), new Tile('A')),
                new(new Coord(R._9, C.H), new Tile('B'))
            ];
            board.MakeMove(tiles);

            var result = Score.GetEndpoints(board.SquareByRow, (int)C.H, [((int)R._8), ((int)R._9)]);

            Assert.Equal((((int)R._8), ((int)R._9)), result);
        }

    }
}
