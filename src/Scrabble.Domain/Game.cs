using System;
using System.Collections.Generic;

namespace Scrabble.Domain
{
    public class Game
    {
        public Guid Id { get; set; }
        public ILexicon Lexicon { get; set; }
        public Board Board { get; set; }
        public TileBag TileBag { get; set; }
        public Players Players { get; set; } 

        public Move NextMove;

        public List<string> messages;

        public List<(Move move, int score, string playerName)> Moves { get; set; }

        private GameState _state;

        public static class GameFactory
        {
            public static Game CreateGame(Lexicon lexicon, List<Player> players)
            {
 
                return new Game(lexicon, players);
            }
        }

        private Game(Lexicon lexicon, List<Player> players) {

            this.Lexicon = lexicon;
            this.TileBag = TileBag.TileBagFactory.Create();
            this.Board = new Board(lexicon.IsWordValid);
            this.Players = new Players(players);

            this._state = new MoveStarting();
        }

        public void SetState(GameState state)
        {
            _state = state;
        }

        public void Handle()
        {
            _state.Handle(this);
        }

    }
}