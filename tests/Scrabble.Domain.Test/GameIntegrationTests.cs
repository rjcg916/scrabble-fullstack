using Moq;
using Scrabble.Domain.Interface;
using System.Collections.Generic;
using Xunit;

namespace Scrabble.Domain.Tests
{
    public class GameIntegrationTests
    {
        [Fact]
        public void SimulateCompleteGame_ShouldPlayThroughAllStates()
        {
            // Arrange
            var lexicon = new Mock<ILexicon>();
            lexicon.Setup(l => l.IsWordValid(It.IsAny<string>())).Returns(true);

            var player1 = new Player("Alice");
            var player2 = new Player("Bob");

            var players = new List<Player> { player1, player2 };

            var game = Game.GameFactory.CreateGame(lexicon.Object, players);

            // Act & Assert

            // Initial state should be GameStarting
            game.NextMove = Move.MoveFactory.CreateMove(new(R._8, C.H), new List<Tile> { new Tile('A') }, isHorizontal: true);
            game.Handle();
            Assert.Contains($"Game Starting: Id: {game.Id} starting.", game.messages[^1]);
            Assert.IsType<MoveStarting>(game.GetState());
         
            List<Move> moves = new();
            moves.Add(Move.MoveFactory.CreateMove(new(R._9, C.H), new List<Tile> { new Tile('B') }, isHorizontal: false));
            moves.Add(Move.MoveFactory.CreateMove(new(R._10, C.H), new List<Tile> { new Tile('C') }, isHorizontal: false));

            var moveIndex = 0;

            // Simulate moves for players
            while (!(game.GetState() is GameCompleted))
            {
                var currentState = game.GetState();

                game.Handle();

                // Ensure state transition is happening correctly
                if (currentState is MoveStarting)
                {
                    Assert.Contains("Move Starting:", game.messages[^1]);

                    if (moveIndex < moves.Count)
                    {
                        game.NextMove = moves[moveIndex];
                        moveIndex++;
                    } else
                    {
                        game.SetState(new SkippingMove());
                    }
                }
                else if (currentState is MoveFinishing)
                {
                    Assert.Contains("Move Finishing:", game.messages[^1]);
                }
                else if (currentState is SkippingMove)
                {
                    Assert.Contains("Skipping Move:", game.messages[^1]);

                    game.SetState(new GameFinishing());
                }
                else if (currentState is GameFinishing)
                {
                    Assert.Contains("Game Finishing:", game.messages[^1]);
                }
            }

            // Final state should be GameCompleted
            game.Handle();
            Assert.IsType<GameCompleted>(game.GetState());
            Assert.Contains("Game Completed:", game.messages[^1]);
        }
    }
}
