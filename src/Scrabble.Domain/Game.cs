using System;
using System.Collections.Generic;

namespace Scrabble.Domain
{
    public class Game
    {
        public ILexicon Lexicon { get; set; }
        public Board Board { get; set; }
        public TileBag TileBag { get; set; }
        public Dictionary<byte, Player> Players { get; set; } = [];

        public int NumberOfPlayers => Players.Count;
        public byte TurnOfPlayer { get; set; } = 1;
        public bool GameDone { get; } = false;

        private Game() { }

        public class GameFactory(ILexicon lexicon)
        {
            private const byte MINPLAYERS = 2;
            private const byte MAXPLAYERS = 4;
            readonly string MINMAXPLAYERERROR = "Game must have 2, 3 or 4 players.";

            public Game CreateGame(List<string> playerNames)
            {
                var numberOfPlayers = playerNames.Count;

                bool validNumberOfPlayers = numberOfPlayers >= MINPLAYERS && numberOfPlayers <= MAXPLAYERS;

                if (!validNumberOfPlayers)
                {
                    throw new Exception(MINMAXPLAYERERROR);
                }

                var game = new Game
                {
                    Lexicon = lexicon,
                    TileBag = new TileBag(),
                    Board = new Board()
                };

                // create each player, add to game and draw tiles from bag
                byte i = 1;
                playerNames.ForEach(name =>
                {
                    var player = new Player(name, game.TileBag);
                    game.Players.Add(i++, player);
                });

                return game;
            }
        }
    }
}
