using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

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

        public void SaveGameState(string filePath)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                IncludeFields = true
            };

            string jsonString = JsonSerializer.Serialize(this, options);
            File.WriteAllText(filePath, jsonString);
        }

        // Method to load the game state from a file
        public static Game LoadGameState(string filePath, ILexicon lexicon)
        {
            var options = new JsonSerializerOptions
            {
                IncludeFields = true
            };

            string jsonString = File.ReadAllText(filePath);
            var game = JsonSerializer.Deserialize<Game>(jsonString, options);

            if (game != null)
            {
                game.Lexicon = lexicon;
                game.Board.IsWordValid = lexicon.IsWordValid;
            }

            return game;
        }
    }
}