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


        private Game(Lexicon lexicon, PlayerList players) {
            Lexicon = lexicon;
            TileBag = new TileBag();
            Board = new Board(lexicon.IsWordValid);

            // create each player, add to game and draw tiles from bag
            byte i = 1;
            players.List.ForEach(p =>
            {
                var player = new Player(p.Name);
                this.Players.Add(i++, player);
            });
        }

        public TileBag DrawTiles(Player player)
        {
            var tilesAvailable = TileBag.Count;

            if (tilesAvailable == 0)
                throw new Exception("No tiles available to draw.");

            var tilesNeeded = Rack.Capacity - player.Rack.TileCount;

            var drawCount = tilesAvailable > tilesNeeded ? tilesNeeded : tilesAvailable;

            var (tilesToAddToRack, tileBagAfterRemoval) = TileBag.DrawTiles(new TileDrawCount(drawCount));

            player.Rack = player.Rack.AddTiles(tilesToAddToRack);

            return tileBagAfterRemoval;
        }

        public static class GameFactory
        {
            public static Game CreateGame(Lexicon lexicon, PlayerList players)
            {
                return new Game(lexicon, players);
            }
        }
    }
}