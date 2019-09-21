using System;
using System.Collections.Generic;
using System.Text;

namespace ScrabbleLib.Model
{
    public class Game
    {
        Board board { get; set; }

        Dictionary<byte, Player> players { get; set; }
        byte numberOfPlayers { get; set; }

        TileBagService tileBagService = new TileBagService();
        List<Tile> tileBag;

        byte turnOfPlayer { get; set; }
        bool gameDone { get; } = false;
        Lexicon lexicon;

        Game(byte numberOfPlayers = 2)
        {
            // initialize/choose lexicon
            this.lexicon = new Lexicon();

            // fill tile bag
            this.tileBag = this.tileBagService.GetTileBag();

            // for each player, draw tiles from bag
            this.numberOfPlayers = numberOfPlayers;
            for (byte i = 1; i <= numberOfPlayers; i++)
            {
                var player = new Player(i.ToString());
                this.tileBag = player.DrawTiles(this.tileBag);
                players.Add(i, player);
            }

            // create board
            this.board = new Board();

        }

    }
}
