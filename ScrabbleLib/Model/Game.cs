using System;
using System.Collections.Generic;
using System.Text;

namespace ScrabbleLib.Model
{
    public class Game
    {
        public Board board { get; set; }

        public Dictionary<byte, Player> players { get; set; } = new Dictionary<byte, Player>();
        public byte numberOfPlayers { get; set; }

        TileBag tileBag = new TileBag();
       
        public byte turnOfPlayer { get; set; }

        public bool gameDone { get; } = false;

        public Lexicon lexicon { get; set; }

        public Game(byte numberOfPlayers = 2)
        {
            // initialize/choose lexicon
            this.lexicon = new Lexicon();

            // fill tile bag
            this.tileBag = new TileBag();

            // for each player, draw tiles from bag
            this.numberOfPlayers = numberOfPlayers;
            for (byte i = 1; i <= numberOfPlayers; i++)
            {
                var player = new Player();
                player.DrawTiles(this.tileBag);
                players.Add(i, player);
            }

            // create board
            this.board = new Board();

        }

    }
}
