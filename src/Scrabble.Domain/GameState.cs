using System.Linq;

namespace Scrabble.Domain
{

    public abstract class GameState
    {
        public abstract void Handle(Game game);
    }

    public class GameStarting : GameState
    {
        public override void Handle(Game game)
        {

            game.messages.Add($"Game {game.Id} starting.");

            game.SetState(new MoveStarting());
        }
    }

    public class MoveStarting : GameState
    {
        public override void Handle(Game game)
        {
           
            var currentPlayer = game.Players.CurrentPlayer;

            game.messages.Add($"Move in process for {currentPlayer.Name}");

            // if no more tiles in bag or rack
            if ((game.TileBag.Count == 0) && (currentPlayer.Rack.TileCount == 0))
            {
                game.messages.Add($"No more tiles in bag or rack.");
                game.SetState(new GameFinishing());
                return;
            }

            if (game.TileBag.Count > 0)
            {
                // get tiles
                game.TileBag = currentPlayer.DrawTiles(game.TileBag);
                game.messages.Add($"Drawing tiles");
            }

     
            game.SetState(new MoveFinishing());
                      
        }
    }

    public class SkippingMove : GameState
    {
        public override void Handle(Game game)
        {
            game.messages.Add($"Skipping turn for {game.Players.CurrentPlayer.Name}");
            
            game.Players.GetNext();

            game.SetState(new MoveStarting());
        }
    }

    public class MoveFinishing : GameState
    {
        public override void Handle(Game game)
        {
            var board = game.Board;
            var move = Move.MoveFactory.CreateMove(game.NextMove.TilePlacements);
            var player = game.Players.CurrentPlayer;

            // can move be made
            if (!move.IsValid(move.TilePlacements).valid)
                game.SetState(new MoveFinishing());

            // make and score move
            board.MakeMove(move);
            var score = board.ScoreMove(move);
            game.Players.CurrentPlayer.Score =+ score;

            // record move
            game.Moves.Add((move, score, player.Name));
           
            game.messages.Add($"Move for {player.Name} with tiles {move.Letters} was completed with score {score}");

            // next player's turn
            game.Players.GetNext();

            game.SetState(new MoveStarting());
  
        }
    }

    public class GameFinishing : GameState
    {
        public override void Handle(Game game)
        {

            var players = game.Players;

            var winner = players.Leader();
   
            game.SetState(new GameCompleted());

            game.messages.Add($"Game Completing: {game.Id}");

            game.messages.Add($"{winner.Name} won with {winner.Score} points");

            game.messages.Add(string.Join(", ", players.Select(p => $"{p.Name}: {p.Score}")));

        }
    }

    public class GameCompleted : GameState
    {
        public override void Handle(Game game)
        {
            game.messages.Add($"Game Completed: {game.Id}");
        }
    }
}
