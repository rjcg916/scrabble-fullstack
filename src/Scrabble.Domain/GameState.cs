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

            game.messages.Add($"Game Starting: Id: {game.Id} starting.");

            game.SetState(new MoveStarting());
        }
    }

    public class MoveStarting : GameState
    {
        public override void Handle(Game game)
        {
           
            var currentPlayer = game.Players.CurrentPlayer;

   
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
                game.messages.Add($"Drawing tiles.");
            }

     
            game.SetState(new MoveFinishing());

            game.messages.Add($"Move Starting: Player: {currentPlayer.Name}");

        }
    }

    public class SkippingMove : GameState
    {
        public override void Handle(Game game)
        {
  
            game.Players.GetNext();

            game.NextMove = null;

            game.SetState(new MoveStarting());

            game.messages.Add($"Skipping Move: Player: {game.Players.CurrentPlayer.Name}");

        }
    }

    public class MoveFinishing : GameState
    {
        public override void Handle(Game game)
        {
            var board = game.Board;
            var move = game.NextMove;
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
           
   
            game.NextMove = null;

            // next player's turn
            game.Players.GetNext();

            game.SetState(new MoveStarting());

            game.messages.Add($"Move Finishing: Move for {player.Name} with tiles {move.Letters} was completed with score {score}");

        }
    }

    public class GameFinishing : GameState
    {
        public override void Handle(Game game)
        {

            var players = game.Players;

            var winner = players.Leader();
   
            game.SetState(new GameCompleted());

            game.messages.Add($"{winner.Name} won with {winner.Score} points");

            game.messages.Add(string.Join(", ", players.Select(p => $"{p.Name}: {p.Score}")));

            game.messages.Add($"Game Finishing: Id: {game.Id}");

        }
    }

    public class GameCompleted : GameState
    {
        public override void Handle(Game game)
        {
            game.messages.Add($"Game Completed: Id: {game.Id}");
        }
    }
}
