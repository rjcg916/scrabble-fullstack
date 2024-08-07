﻿using System.Collections.Generic;
using Xunit;
using static Scrabble.Domain.Move;

namespace Scrabble.Domain.Tests
{
    public class ScorerTests
    {
        static private bool MockWordValidator(string _) => true;

        [Fact]
        public void ScoreMove_AllBlanks_ReturnsCorrectScore()
        {
            //starting board
            var board = new Board(MockWordValidator);

            // define move
            var tilesAsPlacement = new List<TilePlacement>
            {
                new(new Coord(R._8, C.H), new Tile('?')),
                new(new Coord(R._8, C.I), new Tile('?')),
                new(new Coord(R._8, C.J), new Tile('?')),
            };

            // score initial move
            Assert.Equal(0, board.ScoreMove(MoveFactory.CreateMove(tilesAsPlacement)));
        }

        [Fact]
        public void ScoreMove_SomeBlanks_ReturnsCorrectScore()
        {
            //starting board
            var board = new Board(MockWordValidator);

            // initial move
            var tilesAsPlacement = new List<TilePlacement>
            {
                new(new Coord(R._8, C.H), new Tile('A')),
                new(new Coord(R._8, C.I), new Tile('?')),
                new(new Coord(R._8, C.J), new Tile('Z')),
            };

            // score
            Assert.Equal(11, board.ScoreMove(MoveFactory.CreateMove(tilesAsPlacement)));
        }

        [Fact]
        public void ScoreMove_InitialHorizontal_ReturnsCorrectScore()
        {
            //starting board

            var board = new Board(MockWordValidator);

            // score initial move
            var tilesAsPlacement = new List<TilePlacement>
            {
                new(new Coord(R._8, C.H), new Tile('B')),
                new(new Coord(R._8, C.I), new Tile('Z')),
                new(new Coord(R._8, C.J), new Tile('D')),
            };

            Assert.Equal(15, board.ScoreMove(MoveFactory.CreateMove(tilesAsPlacement)));

        }


        [Fact]
        public void ScoreMove_InitialHorizontalDoubleLetter_ReturnsCorrectScore()
        {
            //starting board
            var board = new Board(MockWordValidator);

            var tilesAsPlacement = new List<TilePlacement>
            {
                new(new Coord(R._8, C.H), new Tile('B')),
                new(new Coord(R._8, C.I), new Tile('C')),
                new(new Coord(R._8, C.J), new Tile('D')),
                new(new Coord(R._8, C.K), new Tile('B')),
                new(new Coord(R._8, C.L), new Tile('C')),
                new(new Coord(R._8, C.M), new Tile('D')),
            };

            Assert.Equal(19, board.ScoreMove(MoveFactory.CreateMove(tilesAsPlacement)));
        }


        [Fact]
        public void ScoreMove_InitialVertical_ReturnsCorrectScore()
        {
            //starting board
            var board = new Board(MockWordValidator);

            var tilesAsPlacement = new List<TilePlacement>
            {
                new(new Coord(R._7, C.H), new Tile('B')),
                new(new Coord(R._8, C.H), new Tile('C')),
                new(new Coord(R._9, C.H), new Tile('D')),

            };
            Assert.Equal(8, board.ScoreMove(MoveFactory.CreateMove(tilesAsPlacement)));
        }

        [Fact]
        public void ScoreMove_HorizontalNextExtends_ReturnsCorrectScore()
        {
            //starting board

            var board = new Board(MockWordValidator);


            var tilesAsPlacement = new List<TilePlacement>
            {
                new(new Coord(R._8, C.H), new Tile('B')),
                new(new Coord(R._8, C.I), new Tile('C')),
                new(new Coord(R._8, C.J), new Tile('D')),
            };
            var firstMove = MoveFactory.CreateMove(tilesAsPlacement);

            // score first move
            var initialScore = board.ScoreMove(firstMove);
            Assert.Equal(8, initialScore);

            // make the first move
            board = board.MakeMove(firstMove);

            // score next move
            var newTiles = new List<TilePlacement>
            {
                new(new Coord(R._8, C.G), new Tile('A')),
                new(new Coord(R._8, C.K), new Tile('J'))
            };

            var moveScore = board.ScoreMove(MoveFactory.CreateMove(newTiles));

            Assert.Equal(17, moveScore);
        }

        [Fact]
        public void ScoreMove_VerticalNextExtends_ReturnsCorrectScore()
        {
            //starting board with tiles
            var board = new Board(MockWordValidator, new Coord(R._8, C.G), [new Tile('J')], isHorizontal: false);


            // next move
            var tileList = new List<TilePlacement>
            {
                new(new Coord(R._7, C.G), new Tile('A')),
                new(new Coord(R._9, C.G), new Tile('Z'))
            };
            var move = MoveFactory.CreateMove(tileList);

            Assert.Equal(30, board.ScoreMove(move));
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


            var initialScore = board.ScoreMove(MoveFactory.CreateMove(tilesAsPlacement));
            Assert.Equal(8, initialScore);


            // next move
            var newTiles = new List<TilePlacement>
            {
                new(new Coord(R._9, C.J), new Tile('A')),
                new(new Coord(R._9, C.K), new Tile('J'))
            };

            var move = MoveFactory.CreateMove(newTiles);

            var moveScore = board.ScoreMove(move);

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
            var initialScore = board.ScoreMove(MoveFactory.CreateMove(tilesAsPlacement));
            Assert.Equal(8, initialScore);

            // make a move
            var newTiles = new List<TilePlacement>
            {
                new(new Coord(R._7, C.J), new Tile('A')),
                new(new Coord(R._9, C.J), new Tile('J'))
            };

            var move = MoveFactory.CreateMove(newTiles);

            // score move
            var moveScore = board.ScoreMove(move);

            Assert.Equal(11, moveScore);
        }

        [Fact]
        public void ScoreMove_AlternateScorers_ReturnSameScore()
        {
            //starting board
            var board = new Board(MockWordValidator);

            var tiles = new List<Tile> { new('B'), new('C'), new('D'), new('B'), new('C'), new('D') };
            var startFrom = new Coord(R._8, C.H);

            var tilesAsPlacement = new List<TilePlacement>
            {
                new(new Coord(R._8, C.H), new Tile('B')),
                new(new Coord(R._8, C.I), new Tile('C')),
                new(new Coord(R._8, C.J), new Tile('D')),
                new(new Coord(R._8, C.K), new Tile('B')),
                new(new Coord(R._8, C.L), new Tile('C')),
                new(new Coord(R._8, C.M), new Tile('D')),
            };

            // score two alternate moves should be equal
            Assert.Equal(board.ScoreMove(MoveFactory.CreateMove(startFrom, tiles, isHorizontal:true)), board.ScoreMove(MoveFactory.CreateMove(tilesAsPlacement)));

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
            var move = MoveFactory.CreateMove(tiles);


            var score = board.ScoreMove(move);
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
            board.MakeMove(MoveFactory.CreateMove(tiles));

            var result = Scorer.GetEndpoints(board.SquareByColumn, (int)R._8, [(int)C.H, (int)C.I]);

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
            board.MakeMove(MoveFactory.CreateMove(tiles));

            var result = Scorer.GetEndpoints(board.SquareByRow, (int)C.H, [((int)R._8), ((int)R._9)]);

            Assert.Equal((((int)R._8), ((int)R._9)), result);
        }
        

    }

}

