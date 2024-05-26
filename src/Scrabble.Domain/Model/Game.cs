using System;
using System.Collections.Generic;

namespace Scrabble.Domain.Model
{
    public struct GameDetails
    {
        public TileBag TileBag;
        public List<string> rowLabels;
        public List<string> colLabels;
        public List<CoordSquare> squares;
    }

    public class Game
    {
        public byte MINPLAYERS = 2;
        public byte MAXPLAYERS = 4;
        readonly string MINMAXPLAYERERROR = "Game must have 2, 3 or 4 players.";

        public Board Board { get; set; }

        public Dictionary<byte, Player> Players { get; set; } = [];
        public byte NumberOfPlayers { get; set; }

        readonly TileBag TileBag = new();

        public int RemainingTileCount
        {
            get
            {
                return TileBag.Count;
            }
        }


        public byte TurnOfPlayer { get; set; } = 1;

        public bool GameDone { get; } = false;

        public Lexicon Lexicon { get; set; }

        public Game(List<string> playerNames)
        {
            this.NumberOfPlayers = (byte)playerNames.Count;

            bool validNumberOfPlayers = (NumberOfPlayers >= MINPLAYERS) && (NumberOfPlayers <= MAXPLAYERS);

            if (!validNumberOfPlayers)
            {
                throw new Exception(MINMAXPLAYERERROR);
            }

            // initialize/choose lexicon
            this.Lexicon = new Lexicon();

            // fill tile bag
            this.TileBag = new TileBag();

            // for each player, draw tiles from bag

            byte i = 1;

            playerNames.ForEach(name =>
            {
                var player = new Player(name);
                player.DrawTiles(this.TileBag);
                Players.Add(i++, player);
            }
            );

            // create board
            this.Board = new Board();

        }

        public GameDetails GetDetails()
        {

            var details = new GameDetails
            {
                TileBag = this.TileBag,

                rowLabels = Board.GetRowLabels(),

                colLabels = Board.GetColLabels(),

                squares = this.Board.GetCoordSquares()
            };

            return details;

        }

    }
}
