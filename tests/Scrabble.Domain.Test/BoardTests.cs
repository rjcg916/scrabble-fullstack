using System.Collections.Generic;
using Xunit;
using static Scrabble.Domain.Move;

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
            Assert.Equal(0, board.MoveNumber);
        }

        [Fact]
        public void Board_Constructor_WithInitialMove_PlacesTilesCorrectly()
        {
            var tiles = new List<Tile> { new('A'), new('B'), new('C') };
            var startFrom = new Coord(R._8, C.H);
            var board = new Board(MockWordValidator, startFrom, tiles, isHorizontal: true);

            Assert.Equal('A', board.GetTile(new Coord(R._8, C.H)).Letter);
            Assert.Equal('B', board.GetTile(new Coord(R._8, C.I)).Letter);
            Assert.Equal('C', board.GetTile(new Coord(R._8, C.J)).Letter);

            Assert.Equal(1, board.MoveNumber);

        }

        [Fact]
        public void Board_Copy_CreatesIdenticalBoard()
        {
            var board = new Board(MockWordValidator);
            List<TilePlacement> tiles =
            [
                new(new Coord(R._8, C.H), new Tile('A'))
            ];
            board.MakeMove(MoveFactory.CreateMove(tiles));

            var copy = new Board(board);

            Assert.NotSame(board, copy);
            Assert.Equal('A', copy.GetTile(new Coord(R._8, C.H)).Letter);
            Assert.Equal(board.MoveNumber, copy.MoveNumber);
        }

        [Fact]
        public void DoesMoveTouchStar_ReturnsTrue_WhenMoveTouchesStar()
        {
            var tiles = new List<TilePlacement>
            {
                new(Board.STAR, new Tile('A'))
            };

            Assert.True(Board.DoesMoveTouchSTAR(tiles));
        }

        [Fact]
        public void DoesMoveTouchStar_ReturnsFalse_WhenMoveDoesNotTouchStar()
        {
            var tiles = new List<TilePlacement>
            {
                new(new Coord(R._7, C.G), new Tile('A'))
            };


            Assert.False(Board.DoesMoveTouchSTAR(tiles));
        }

        [Fact]
        public void IsOccupied_ReturnsTrue_WhenSquareIsOccupied()
        {
            var board = new Board(MockWordValidator);

            var coord = new Coord(R._8, C.H);
          
            List<TilePlacement> tiles =
            [
                new(coord, new Tile('A'))
            ];
            board.MakeMove(MoveFactory.CreateMove(tiles));

            Assert.True(board.IsOccupied(coord));
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

            List<TilePlacement> tiles =
            [
                new(coord, new Tile('A'))
            ];
            board.MakeMove(MoveFactory.CreateMove(tiles));

            Assert.True(board.AreOccupied([coord, new(R._7, C.G)]));
        }

        [Fact]
        public void AreOccupied_ReturnsFalse_WhenNoSquaresAreOccupied()
        {
            var board = new Board(MockWordValidator);

            Assert.False(board.AreOccupied([new(R._8, C.H), new(R._7, C.G)]));
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
            board.MakeMove(MoveFactory.CreateMove(tiles));

            var proposedTiles = new List<TilePlacement>
            {
                new(new Coord(R._8, C.J), new Tile('A')),
                new(new Coord(R._8, C.K), new Tile('B'))
            };
            
            Assert.True(board.TilesContiguous(proposedTiles).valid);
        }

        [Fact]
        public void TilesContiguousOnBoard_ReturnsFalse_WhenTilesAreNotContiguous()
        {
            var board = new Board(MockWordValidator);
            var tiles = new List<TilePlacement>
            {
                new(new Coord(R._8, C.G), new Tile('A')),
                new(new Coord(R._8, C.H), new Tile('B'))
            };
            board.MakeMove(MoveFactory.CreateMove(tiles));

            var proposedTiles = new List<TilePlacement>
            {
                new(new Coord(R._8, C.J), new Tile('A')),
                new(new Coord(R._8, C.K), new Tile('B'))
            };

            Assert.False(board.TilesContiguous(proposedTiles).valid);
        }

        [Fact]
        public void WordOnBoardValid_RealValidator_ReturnValid()
        {
            var lex = new Lexicon(["ball", "bat"]);

            var board = new Board(lex.IsWordValid);
            var tiles = new List<TilePlacement>
            {
                new(new Coord(R._8, C.H), new Tile('B')),
                new(new Coord(R._8, C.I), new Tile('A')),
                new(new Coord(R._8, C.J), new Tile('L')),
                new(new Coord(R._8, C.K), new Tile('L'))
            };
            board.MakeMove(MoveFactory.CreateMove(tiles));

            Assert.True(board.OnlyValidWords().valid);
        }
        [Fact]
        public void WordOnBoardInValid_RealValidator_ReturnInvalid()
        {
            var lex = new Lexicon(["mitt", "base"]);

            var board = new Board(lex.IsWordValid);
            var tiles = new List<TilePlacement>
            {
                new(new Coord(R._8, C.H), new Tile('B')),
                new(new Coord(R._8, C.I), new Tile('A')),
                new(new Coord(R._8, C.J), new Tile('L')),
                new(new Coord(R._8, C.K), new Tile('L'))
            };
            board.MakeMove(MoveFactory.CreateMove(tiles));

            var(valid, errorList) = board.OnlyValidWords();
            Assert.False(valid);
            Assert.Equal("BALL", errorList[0].Letters);
            Assert.Equal<Coord>(new Coord(R._8, C.A), errorList[0].Location);
        }


        [Fact]
        public void GetSlice_ReturnsCorrectRowSlice()
        {
            var board = new Board(MockWordValidator);
            List<TilePlacement> tiles =
            [
                new(new Coord(R._8, C.H), new Tile('A')),
                new(new Coord(R._8, C.I), new Tile('B'))
            ];
            board.MakeMove(MoveFactory.CreateMove(tiles));

            var result = Board.GetSquares(board.SquareByColumn, (int)R._8, (0, Coord.ColCount - 1));

            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void GetSlice_ReturnsCorrectColumnSlice()
        {
            var board = new Board(MockWordValidator);
            List<TilePlacement> tiles = 
            [
                new(new Coord(R._7, C.H), new Tile('A')),
                new(new Coord(R._8, C.H), new Tile('B'))
            ];

            board.MakeMove(MoveFactory.CreateMove(tiles));

            var result = Board.GetSquares(board.SquareByRow, (int)C.H, (0, Coord.RowCount - 1));

            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void ValidateBoardSlices_NoErrorsOnEmptyBoard()
        {
            var board = new Board(MockWordValidator);
            var errors = new List<PlacementError>();

            errors = board.ValidateWordSlices(r => Board.GetSquares(board.SquareByColumn, r, (0, Coord.ColCount - 1)), Coord.RowCount, isHorizontal:true);

            Assert.Empty(errors);
        }

        [Fact]
        public void IsMoveValid_DoesNotChangeBoard_True()
        {
            var tiles = new List<TilePlacement>
            {
                new(new Coord(R._8, C.G), new Tile('A')),
                new(new Coord(R._8, C.H), new Tile('B')),
                new(new Coord(R._8, C.I), new Tile('C')),
            };
            var board = new Board(MockWordValidator, Move.MoveFactory.CreateMove(tiles));
            var originalBoard = new Board(board);

            var moveTiles = new List<TilePlacement>
            {
                new(new Coord(R._8, C.A), new Tile('A')),
                new(new Coord(R._8, C.K), new Tile('C')),
            };
            var result = board.IsMoveValid(moveTiles);

            Assert.Equal(originalBoard, board);
        }

        [Fact]
        public void IsMoveValid_FirstMoveOnStar_True()
        {
            var board = new Board(MockWordValidator);
            var tiles = new List<TilePlacement>
            {
                new(new Coord(R._8, C.G), new Tile('A')),
                new(new Coord(R._8, C.H), new Tile('B'))
            };
            Assert.True(board.IsMoveValid(tiles).valid);
        }

        [Fact]
        public void IsMoveValid_FirstMoveNotOnStar_False()
        {
            var board = new Board(MockWordValidator);
            var tiles = new List<TilePlacement>
            {
                new(new Coord(R._8, C.A), new Tile('A')),
                new(new Coord(R._8, C.B), new Tile('B'))
            };
            Assert.False(board.IsMoveValid(tiles).valid);
        }

        [Fact]
        public void IsMoveValid_FirstMoveInvalidWord_False()
        {
            var board = new Board((s) => false); // any word invalid
            var tiles = new List<TilePlacement>
            {
                new(new Coord(R._8, C.G), new Tile('A')),
                new(new Coord(R._8, C.H), new Tile('B'))
            };
            Assert.False(board.IsMoveValid(tiles).valid);
        }

        [Fact]
        public void IsMoveValid_FirstHorizontalMoveNotContiguous_False()
        {
            var board = new Board(MockWordValidator);
            var tiles = new List<TilePlacement>
            {               
                new(new Coord(R._8, C.G), new Tile('A')),
                new(new Coord(R._8, C.H), new Tile('B')),
                new(new Coord(R._8, C.J), new Tile('C')),
            };
            Assert.False(board.IsMoveValid(tiles).valid);
        }

        [Fact]
        public void IsMoveValid_FirstVerticalMoveNotContiguous_False()
        {
            var board = new Board(MockWordValidator);
            var tiles = new List<TilePlacement>
            {
                new(new Coord(R._7, C.H), new Tile('A')),
                new(new Coord(R._8, C.H), new Tile('B')),
                new(new Coord(R._10, C.H), new Tile('C')),
            };
            var result = board.IsMoveValid(tiles);

            Assert.False(result.valid);
        }

        [Fact]
        public void IsMoveValid_FirstMoveOnDiagonal_False()
        {
            var board = new Board(MockWordValidator);
            var tiles = new List<TilePlacement>
            {
                new(new Coord(R._8, C.G), new Tile('A')),
                new(new Coord(R._8, C.H), new Tile('B')),
                new(new Coord(R._9, C.I), new Tile('C')),
            };
            var result = board.IsMoveValid(tiles);

            Assert.False(result.valid);
        }

        [Fact]
        public void IsMoveValid_NextMoveHorizontalExtends_True()
        {
            var tiles = new List<TilePlacement>
            {
                new(new Coord(R._8, C.G), new Tile('A')),
                new(new Coord(R._8, C.H), new Tile('B')),
                new(new Coord(R._8, C.I), new Tile('C')),
            };
            var board = new Board(MockWordValidator, Move.MoveFactory.CreateMove(tiles));


            var moveTiles = new List<TilePlacement>
            {
                new(new Coord(R._8, C.F), new Tile('A')),
                new(new Coord(R._8, C.J), new Tile('B')),
                new(new Coord(R._8, C.K), new Tile('C')),
            };
            var result = board.IsMoveValid(moveTiles);

            Assert.True(result.valid);
        }

        [Fact]
        public void IsMoveValid_NextMoveHorizontalGap_False()
        {
            var tiles = new List<TilePlacement>
            {
                new(new Coord(R._8, C.G), new Tile('A')),
                new(new Coord(R._8, C.H), new Tile('B')),
                new(new Coord(R._8, C.I), new Tile('C')),
            };
            var board = new Board(MockWordValidator, Move.MoveFactory.CreateMove(tiles));


            var moveTiles = new List<TilePlacement>
            {
                new(new Coord(R._8, C.A), new Tile('A')),
                new(new Coord(R._8, C.K), new Tile('C')),
            };
            var result = board.IsMoveValid(moveTiles);

            Assert.False(result.valid);
        }
    }
}