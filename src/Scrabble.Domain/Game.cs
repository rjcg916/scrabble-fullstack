using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Scrabble.Domain
{
    public class Game
    {
        public ILexicon Lexicon { get; set; }
        public Board Board { get; set; }
        public TileBag TileBag { get; set; }
        public List<Player> Players { get; set; } = [];
        public int NumberOfPlayers => Players.Count;
        public byte TurnOfPlayer { get; set; } = 1;
        public bool GameDone { get; } = false;
        public List<(Move move,int score, string player)> Moves { get; set; }

        private Game(Lexicon lexicon, List<Player> players) {
            Lexicon = lexicon;
            TileBag = TileBag.TileBagFactory.Create();           
            Board = new Board(lexicon.IsWordValid);

            // create each player, add to game and draw tiles from bag
            players.ForEach(p =>
            {
                var player = new Player(p.Name);
                TileBag = DrawTiles(player);
                Players.Add(player);
            });
        }

        //public void TakeTurn(Func<List<Tile>, List<Coord>, List<TilePlacement>> GetMove)
        //{
        //    var availableSquares = Board.GetVacantSquares().Select(ls => ls.Coord).ToList();
        //    var availableTiles = Players[TurnOfPlayer].Rack.GetTiles();

        //    bool moveValid;

        //    do
        //    {
        //        var candidateMove = GetMove(availableTiles, availableSquares);
        //         (moveValid, var errorList) = this.Board.IsMoveValid(candidateMove);
        //    } while (!moveValid);
        //}

        public void RecordMove(Player player, Move move) {

            if (!Board.IsMoveValid(move.TilePlacements).valid)
                _ = new SystemException("Invalid Move");                

            var score = Board.ScoreMove(move);
            Board = Board.MakeMove(move);

            Moves.Add((move, score, player.Name));

        }

        internal TileBag DrawTiles(Player player)
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

        public bool IsGameDone()
        {
            var tileBagEmpty = TileBag.Count == 0;

            var playerWithEmptyRack = false;

            foreach (var player in Players) { 

                playerWithEmptyRack = playerWithEmptyRack || (player.Rack.TileCount == 0);
            }
                
            return tileBagEmpty && playerWithEmptyRack;
        }

        public static class GameFactory
        {
            public static Game CreateGame(Lexicon lexicon, List<Player> players)
            {
                if ((players.Count < 2) || (players.Count > 4))
                    throw new System.ArgumentException($"{players.Count} is not a valid players list size");

                return new Game(lexicon, players);
            }
        }
    }
}