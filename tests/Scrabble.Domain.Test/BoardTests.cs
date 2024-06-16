using System.Collections.Generic;
using Xunit;

namespace Scrabble.Domain.Tests
{
    public class BoardTests
    {
        static private bool MockWordValidator(string _) => true;
  
        [Fact]
        public void Board_Constructor_InitializesCorrectly()
        {
            var board = new Board(MockWordValidator);

            Assert.NotNull(board.squares);
            Assert.Equal(Coord.RowCount, board.squares.GetLength(0));
            Assert.Equal(Coord.ColCount, board.squares.GetLength(1));
            for (int r = 0; r < Coord.RowCount; r++)
            {
                for (int c = 0; c < Coord.ColCount; c++)
                {
                    Assert.NotNull(board.squares[r, c]);
                }
            }
        }

        [Fact]
        public void Board_Constructor_WithInitialMove_PlacesTilesCorrectly()
        {
            var tiles = new List<Tile> { new('A'), new('B'), new('C') };
            var startFrom = new Coord(R._8, C.H);
            var board = new Board(MockWordValidator, startFrom, tiles, Placement.Horizontal);

            Assert.Equal('A', board.GetTile(new Coord(R._8, C.H)).Letter);
            Assert.Equal('B', board.GetTile(new Coord(R._8, C.I)).Letter);
            Assert.Equal('C', board.GetTile(new Coord(R._8, C.J)).Letter);
        }

        [Fact]
        public void Board_Copy_CreatesIdenticalBoard()
        {
            var board = new Board(MockWordValidator);
            board.PlaceTiles(
        [
            new(new Coord(R._8, C.H), new Tile('A'))
        ]);

            var copy = new Board(board); //board.Copy();

            Assert.NotSame(board, copy);
            Assert.Equal('A', copy.GetTile(new Coord(R._8, C.H)).Letter);
        }

        [Fact]
        public void DoesMoveTouchStar_ReturnsTrue_WhenMoveTouchesStar()
        {
            var tiles = new List<TilePlacement>
            {
                new(Board.STAR, new Tile('A'))
            };

            var result = Board.DoesMoveTouchSTAR(tiles);

            Assert.True(result);
        }

        [Fact]
        public void DoesMoveTouchStar_ReturnsFalse_WhenMoveDoesNotTouchStar()
        {
            var tiles = new List<TilePlacement>
            {
                new(new Coord(R._7, C.G), new Tile('A'))
            };

            var result = Board.DoesMoveTouchSTAR(tiles);

            Assert.False(result);
        }

        [Fact]
        public void IsOccupied_ReturnsTrue_WhenSquareIsOccupied()
        {
            var board = new Board(MockWordValidator);
            var coord = new Coord(R._8, C.H);
            board.PlaceTiles(
        [
            new(coord, new Tile('A'))
        ]);

            var result = board.IsOccupied(coord);

            Assert.True(result);
        }

        [Fact]
        public void IsOccupied_ReturnsFalse_WhenSquareIsNotOccupied()
        {
            var board = new Board(MockWordValidator);
            var coord = new Coord(R._8, C.H);

            var result = board.IsOccupied(coord);

            Assert.False(result);
        }

        [Fact]
        public void AreOccupied_ReturnsTrue_WhenAnySquareIsOccupied()
        {
            var board = new Board(MockWordValidator);
            var coord = new Coord(R._8, C.H);
            board.PlaceTiles(
        [
            new(coord, new Tile('A'))
        ]);

            var result = board.AreOccupied([coord, new(R._7, C.G)]);

            Assert.True(result);
        }

        [Fact]
        public void AreOccupied_ReturnsFalse_WhenNoSquaresAreOccupied()
        {
            var board = new Board(MockWordValidator);

            var result = board.AreOccupied([new(R._8, C.H), new(R._7, C.G)]);

            Assert.False(result);
        }

        [Fact]
        public void TilesContiguousOnBoard_ReturnsTrue_WhenTilesAreContiguous()
        {
            var board = new Board(MockWordValidator);
            var tiles = new List<TilePlacement>
            {
                new(new Coord(R._8, C.H), new Tile('A')),
                new(new Coord(R._8, C.I), new Tile('B'))
            };
            board.PlaceTiles(tiles);

            var result = board.TilesContiguousOnBoard(tiles).valid;

            Assert.True(result);
        }

        [Fact]
        public void TilesContiguousOnBoard_ReturnsFalse_WhenTilesAreNotContiguous()
        {
            var board = new Board(MockWordValidator);
            var tiles = new List<TilePlacement>
            {
                new(new Coord(R._8, C.H), new Tile('A')),
                new(new Coord(R._8, C.J), new Tile('B'))
            };
            board.PlaceTiles(tiles);

            var result = board.TilesContiguousOnBoard(tiles).valid;

            Assert.False(result);
        }

        [Fact]
        public void ScoreMove_InitialHorizontal_ReturnsCorrectScore()
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

            var board = new Board(MockWordValidator, startFrom, tiles, Placement.Horizontal);

            // score initial move
            var initialScore = board.ScoreMove(tilesAsPlacement);
            Assert.Equal(8, initialScore);

        }
        [Fact]
        public void ScoreMove_InitialHorizontalDoubleLetter_ReturnsCorrectScore()
        {
            //starting board

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

            var board = new Board(MockWordValidator, startFrom, tiles, Placement.Horizontal);

            // score initial move
            var initialScore = board.ScoreMove(tilesAsPlacement);
            Assert.Equal(19, initialScore);

        }

        [Fact]
        public void ScoreMove_InitialVertical_ReturnsCorrectScore()
        {
            //starting board

            var tiles = new List<Tile> { new('B'), new('C'), new('D') };
            var startFrom = new Coord(R._7, C.H);

            var tilesAsPlacement = new List<TilePlacement>
            {
                new(new Coord(R._7, C.H), new Tile('B')),
                new(new Coord(R._8, C.H), new Tile('C')),
                new(new Coord(R._9, C.H), new Tile('D')),

            };

            var board = new Board(MockWordValidator, startFrom, tiles, Placement.Vertical);

            // score initial move
            var initialScore = board.ScoreMove(tilesAsPlacement);
            Assert.Equal(8, initialScore);

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

            var board = new Board(MockWordValidator, startFrom, tiles, Placement.Horizontal);

            // score move
            var initialScore = board.ScoreMove(tilesAsPlacement);
            Assert.Equal(8, initialScore);


            // make a move
            var newTiles = new List<TilePlacement>
            {
                new(new Coord(R._8, C.G), new Tile('A')),
                new(new Coord(R._8, C.K), new Tile('J'))
            };

            var boardForScoring = new Board(board); // board.Copy();
            boardForScoring.PlaceTiles(newTiles);

            // score move
            var moveScore = boardForScoring.ScoreMove(newTiles);

            Assert.Equal(17, moveScore);
        }

        [Fact]
        public void ScoreMove_VerticalNextExtends_ReturnsCorrectScore()
        {
            //starting board
            var board = new Board(MockWordValidator, new Coord(R._8, C.G), [new Tile('J')], Placement.Vertical);


            // make a move

            var tileList = new List<TilePlacement>
            {
                new(new Coord(R._7, C.G), new Tile('A')),
                new(new Coord(R._9, C.G), new Tile('Z'))
            };

            board.PlaceTiles(tileList);

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

            var board = new Board(MockWordValidator, startFrom, tiles, Placement.Horizontal);

            // score move
            var initialScore = board.ScoreMove(tilesAsPlacement);
            Assert.Equal(8, initialScore);


            // make a move
            var newTiles = new List<TilePlacement>
            {
                new(new Coord(R._9, C.J), new Tile('A')),
                new(new Coord(R._9, C.K), new Tile('J'))
            };

            var boardForScoring = new Board(board);// board.Copy();
            boardForScoring.PlaceTiles(newTiles);

            // score move
            var moveScore = boardForScoring.ScoreMove(newTiles);

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

            var board = new Board(MockWordValidator, startFrom, tiles, Placement.Horizontal);

            // score move
            var initialScore = board.ScoreMove(tilesAsPlacement);
            Assert.Equal(8, initialScore);

            // make a move
            var newTiles = new List<TilePlacement>
            {
                new(new Coord(R._7, C.J), new Tile('A')),
                new(new Coord(R._9, C.J), new Tile('J'))
            };

            var boardForScoring = new Board(board); // board.Copy();
            boardForScoring = boardForScoring.MakeMove(newTiles);

            // score move
            var moveScore = boardForScoring.ScoreMove(newTiles);

            Assert.Equal(11, moveScore);
        }


            // Tests for GetRun method
            [Fact]
            public void GetRun_ReturnsCorrectRunForHorizontal()
            {
                var board = new Board(MockWordValidator);
                board.PlaceTiles(
            [
                new(new Coord(R._8, C.H), new Tile('A')),
                new(new Coord(R._8, C.I), new Tile('B'))
            ]);

            var result = Board.GetEndpoints(board.SquareByColumn,(int)R._8, [(int)C.H, (int)C.I]);

            Assert.Equal(((int)C.H, (int)C.I), result);
            }

            [Fact]
            public void GetRun_ReturnsCorrectRunForVertical()
            {
                var board = new Board(MockWordValidator);
                board.PlaceTiles(
                [
                    new(new Coord(R._8, C.H), new Tile('A')),
                    new(new Coord(R._9, C.H), new Tile('B'))
                ]);

                var result = Board.GetEndpoints(board.SquareByRow, (int)C.H, [((int)R._8), ((int)R._9)]);

                Assert.Equal((((int)R._8), ((int)R._9)), result);
            }

            // Tests for GetSlice method
            [Fact]
            public void GetSlice_ReturnsCorrectRowSlice()
            {
                var board = new Board(MockWordValidator);
                board.PlaceTiles(
                [
                    new(new Coord(R._8, C.H), new Tile('A')),
                    new(new Coord(R._8, C.I), new Tile('B'))
                ]);

            var result = Board.GetSquares(board.SquareByColumn, (int) R._8, (0, Coord.ColCount - 1));

            Assert.Equal(2, result.Count);
            }

            [Fact]
            public void GetSlice_ReturnsCorrectColumnSlice()
            {
                var board = new Board(MockWordValidator);
                board.PlaceTiles(
                [
                    new(new Coord(R._7, C.H), new Tile('A')),
                    new(new Coord(R._8, C.H), new Tile('B'))
                ]);

            var result = Board.GetSquares(board.SquareByRow, (int)C.H, (0, Coord.RowCount - 1));

            Assert.Equal(2, result.Count);
            }

            // Tests for ValidateBoardSlices method
            [Fact]
            public void ValidateBoardSlices_NoErrorsOnEmptyBoard()
            {
                var board = new Board(MockWordValidator);
                var errors = new List<PlacementError>();

            errors = board.ValidateWordSlices( r => Board.GetSquares(board.SquareByColumn, r, (0, Coord.ColCount - 1)), Coord.RowCount, Placement.Horizontal);

            Assert.Empty(errors);
            }


            // Tests for ScoreMove method with fixed location
            [Fact]
            public void ScoreMove_CalculatesCorrectScore()
            {
                var board = new Board(MockWordValidator);
                List<TilePlacement> tiles = [
                    new (new Coord(R._8, C.H), new Tile('A')),
                    new (new Coord(R._8, C.I), new Tile('B')),
                    new (new Coord(R._8, C.J), new Tile('C')),
                    ];
                board.PlaceTiles(tiles);

                var score = board.ScoreMove(tiles);
                Assert.Equal(7, score);
            }
        }
    }



