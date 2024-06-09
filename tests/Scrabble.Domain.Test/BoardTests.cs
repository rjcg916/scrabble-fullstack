using System.Collections.Generic;
using Xunit;

namespace Scrabble.Domain.Tests
{
    public class BoardTests
    {

        static private bool MockWordValidator(string _) => true;
        //static private bool MockWordValidatorAlwaysFalse(string _) => false;

        [Fact]
        public void Board_Constructor_InitializesCorrectly()
        {
            var board = new Board(MockWordValidator);

            Assert.NotNull(board.squares);
            Assert.Equal(Board.rowCount, board.squares.GetLength(0));
            Assert.Equal(Board.colCount, board.squares.GetLength(1));
            for (int r = 0; r < Board.rowCount; r++)
            {
                for (int c = 0; c < Board.colCount; c++)
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

            var copy = board.Copy();

            Assert.NotSame(board, copy);
            Assert.Equal('A', copy.GetTile(new Coord(R._8, C.H)).Letter);
        }

        [Fact]
        public void DoesMoveTouchStar_ReturnsTrue_WhenMoveTouchesStar()
        {
            var tiles = new List<TilePlacement>
            {
                new(Board.Star, new Tile('A'))
            };

            var result = Board.DoesMoveTouchStar(tiles);

            Assert.True(result);
        }

        [Fact]
        public void DoesMoveTouchStar_ReturnsFalse_WhenMoveDoesNotTouchStar()
        {
            var tiles = new List<TilePlacement>
            {
                new(new Coord(R._7, C.G), new Tile('A'))
            };

            var result = Board.DoesMoveTouchStar(tiles);

            Assert.False(result);
        }

        [Fact]
        public void IsFirstMove_ReturnsTrue_WhenNoMovesMade()
        {
            var board = new Board(MockWordValidator);

            var result = board.IsFirstMove();

            Assert.True(result);
        }

        [Fact]
        public void IsFirstMove_ReturnsFalse_AfterMoveIsMade()
        {
            var board = new Board(MockWordValidator);
            board.PlaceTiles(
        [
            new(Board.Star, new Tile('A'))
        ]);

            var result = board.IsFirstMove();

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
        public void AreAllTilesContiguous_ReturnsTrue_WhenTilesAreContiguous()
        {
            var board = new Board(MockWordValidator);
            var tiles = new List<TilePlacement>
            {
                new(new Coord(R._8, C.H), new Tile('A')),
                new(new Coord(R._8, C.I), new Tile('B'))
            };

            var result = board.AreAllTilesContiguous(tiles);

            Assert.True(result);
        }

        [Fact]
        public void AreAllTilesContiguous_ReturnsFalse_WhenTilesAreNotContiguous()
        {
            var board = new Board(MockWordValidator);
            var tiles = new List<TilePlacement>
            {
                new(new Coord(R._8, C.H), new Tile('A')),
                new(new Coord(R._8, C.J), new Tile('B'))
            };

            var result = board.AreAllTilesContiguous(tiles);

            Assert.False(result);
        }

        [Fact]
        public void ScoreMove_ReturnsCorrectScore()
        {
            var board = new Board(MockWordValidator);
            var tiles = new List<TilePlacement>
            {
                new(new Coord(R._8, C.H), new Tile('A')),
                new(new Coord(R._8, C.I), new Tile('B')),
                new(new Coord(R._8, C.J), new Tile('C'))
            };

            var result = board.ScoreMove(tiles);

            // Assuming the score is calculated correctly according to your rules.
            // Replace `expectedScore` with the actual expected score.
            int expectedScore = 0; // Calculate based on your scoring rules.
            Assert.Equal(expectedScore, result);
        }

        [Fact]
        public void NextBoard_ReturnsNewBoardWithTilesPlaced()
        {
            var board = new Board(MockWordValidator);
            var tiles = new List<TilePlacement>
            {
                new(new Coord(R._8, C.H), new Tile('A')),
                new(new Coord(R._8, C.I), new Tile('B'))
            };

            var nextBoard = board.NextBoard(tiles);

            Assert.Equal('A', nextBoard.GetTile(new Coord(R._8, C.H)).Letter);
            Assert.Equal('B', nextBoard.GetTile(new Coord(R._8, C.I)).Letter);
            Assert.True(nextBoard.IsOccupied(new Coord(R._8, C.H)));
            Assert.True(nextBoard.IsOccupied(new Coord(R._8, C.I)));
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

                var result = board.GetRun(true, (int)R._8, [(int)C.H, (int)C.I]);

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

                var result = board.GetRun(false, (int)C.H, [((int)R._8), ((int)R._9)]);

                Assert.Equal((((int)R._8), ((int)R._9)), result);
            }

            // Tests for GetSlice method
            [Fact]
            public void GetSlice_ReturnsCorrectRowSlice()
            {
                var board = new Board(MockWordValidator);
                var result = board.GetSlice(true, (int) R._8);

                Assert.Equal(Board.colCount, result.Count);
            }

            [Fact]
            public void GetSlice_ReturnsCorrectColumnSlice()
            {
                var board = new Board(MockWordValidator);
                var result = board.GetSlice(false, (int) C.H);

                Assert.Equal(Board.rowCount, result.Count);
            }

            // Tests for ValidateBoardSlices method
            [Fact]
            public void ValidateBoardSlices_NoErrorsOnEmptyBoard()
            {
                var board = new Board(MockWordValidator);
                var errors = new List<PlacementError>();

                board.ValidateBoardSlices(board.GetRowSlice, Placement.Horizontal, errors);
                
                Assert.Empty(errors);
            }


            // Tests for GetRowSlice method
            [Fact]
            public void GetRowSlice_ReturnsCorrectRow()
            {
                var board = new Board(MockWordValidator);
                var rowSlice = board.GetRowSlice(((int)R._8));

                Assert.Equal(Board.colCount, rowSlice.Count);
            }

            // Tests for GetColSlice method
            [Fact]
            public void GetColSlice_ReturnsCorrectColumn()
            {
                var board = new Board(MockWordValidator);
                var colSlice = board.GetColSlice(((int)C.H));

                Assert.Equal(Board.rowCount, colSlice.Count);
            }

            // Tests for ScoreMove method with fixed location
            [Fact]
            public void ScoreMove_WithFixedLocationCalculatesCorrectScore()
            {
                var board = new Board(MockWordValidator);
                var fixedLocation = (int)R._8;
                var tileLocations = new List<(int location, Tile tile)>
            {
                (((int)C.H), new Tile('A')),
                (((int)C.I), new Tile('B')),
                (((int)C.J), new Tile('C'))
            };

                var score = board.ScoreMove(fixedLocation, tileLocations, false);

                // Assuming some scoring rules, replace expectedScore with the correct value
                int expectedScore = 0; // Replace with correct score calculation
                Assert.Equal(expectedScore, score);
            }
        }
    }



