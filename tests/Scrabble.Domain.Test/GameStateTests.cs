using Moq;
using Xunit;
using System.Collections.Generic;

namespace Scrabble.Domain.Test
{

    public class GameStateTests
    {
        [Fact]
        public void Handle_ShouldAddStartingMessage_AndSetNextState()
        {
            // Arrange
            var game = Game.GameFactory.CreateGame(
                new Mock<Lexicon>().Object,
                new List<Player> { new Player("Player1"), new Player("Player2") }
            );

            var gameStarting = new GameStarting();

            // Act
            gameStarting.Handle(game);

            // Assert
            Assert.Contains($"Game {game.Id} starting.", game.messages);
            Assert.IsType<MoveStarting>(game.GetState());
        }

        [Fact]
        public void Handle_ShouldAddMoveMessage_DrawTiles_AndSetNextState()
        {

            var game = Game.GameFactory.CreateGame(
                new Mock<Lexicon>().Object,
                new List<Player> { new Player("Player1"), new Player("Player2") }
            );

            var moveStarting = new MoveStarting();

            // Act
            moveStarting.Handle(game);

            // Assert
            Assert.Contains($"Move in process for Player1", game.messages);
            Assert.Contains($"Drawing tiles", game.messages);
            Assert.IsType<MoveFinishing>(game.GetState());
        }

        [Fact]
        public void Handle_ShouldSetGameFinishingState_IfNoTiles()
        {
            // Arrange

            var game = Game.GameFactory.CreateGame(
                new Mock<Lexicon>().Object,
                new List<Player> { new Player("Player1"), new Player("Player2") }
            );

            game.TileBag = TileBag.TileBagFactory.Create(new List<Tile>());
            var moveStarting = new MoveStarting();

            // Act
            moveStarting.Handle(game);

            // Assert
            Assert.Contains($"No more tiles in bag or rack.", game.messages);
            Assert.IsType<GameFinishing>(game.GetState());
        }


        [Fact]
        public void Handle_ShouldMakeMove_ScoreMove_AndSetNextState()
        {
            // Arrange

            var game = Game.GameFactory.CreateGame(
                new Mock<Lexicon>().Object,
                new List<Player> { new Player("Player1"), new Player("Player2") }
            );

            game.NextMove = 
                Move.MoveFactory.CreateMove( [new TilePlacement(Board.STAR, new Tile('A'))]);

            var moveFinishing = new MoveFinishing();

            // Act
            moveFinishing.Handle(game);

            // Assert
            Assert.Contains($"Move for Player1 with tiles A was completed with score 1", game.messages);
            Assert.IsType<MoveStarting>(game.GetState());
        }

        [Fact]
        public void Handle_ShouldDeclareWinner_AndSetNextState()
        {
            // Arrange
            var player1 = new Player("Player1");
            player1.Score = 100;
            var player2 = new Player("Player2");
            player2.Score = 50;
            var players = new Players(new List<Player> { player1, player2 });

            var game = Game.GameFactory.CreateGame(
                new Mock<Lexicon>().Object,
                new List<Player> { player1, player2 }
            );
            game.Players = players;

            var gameFinishing = new GameFinishing();

            // Act
            gameFinishing.Handle(game);

            // Assert
            Assert.Contains($"Game Completing: {game.Id}", game.messages);
            Assert.Contains($"Player1 won with 100 points", game.messages);
            Assert.IsType<GameCompleted>(game.GetState());
        }
    }
}
