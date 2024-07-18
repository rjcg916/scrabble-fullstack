using System;
using System.Collections.Generic;

namespace Scrabble.Domain
{
    public class Game
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public ILexicon Lexicon { get; set; }
        public Board Board { get; set; }
        public TileBag TileBag { get; set; }
        public Players Players { get; set; } 

        public Move NextMove { get; set; }

        public List<string> messages = new();

        public List<(Move move, int score, string playerName)> Moves { get; set; } = [];

        private GameState _state;

        public static class GameFactory
        {
            public static Game CreateGame(ILexicon lexicon, List<Player> players)
            {
                return new Game(lexicon, players);
            }
        }

        private Game(ILexicon lexicon, List<Player> players) {

            this.Lexicon = lexicon;
            this.TileBag = TileBag.TileBagFactory.Create();
            this.Board = new Board(lexicon.IsWordValid);
            this.Players = new Players(players);

            this._state = new GameStarting();
        }

        public void SetState(GameState state)
        {
            _state = state;
        }

        public GameState GetState()
        {
            return _state;
        }

        public void Handle()
        {
            _state.Handle(this);
        }

    }
}