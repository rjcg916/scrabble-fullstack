using System;
using System.Collections.Generic;
using System.Text;

namespace Scrabble.Domain.Model
{
    public struct GameDetails
    {
        public TileBag tileBag;
 //       public List<LetterValue> letterValues;
        public List<string> rowLabels;
        public List<string> colLabels;
        public List<CoordSquare> squares;
    }

    public class Game
    {
        public byte MINPLAYERS = 2;
        public byte MAXPLAYERS = 4;
        string MINMAXPLAYERERROR = "Game must have 2, 3 or 4 players.";

        public Board board { get; set; }

        public Dictionary<byte, Player> players { get; set; } = new Dictionary<byte, Player>();
        public byte numberOfPlayers { get; set; }

        TileBag tileBag = new TileBag();

        public int remainingTileCount
        {
            get
            {
                return tileBag.count;
            }
        }


        public byte turnOfPlayer { get; set; } = 1;

        public bool gameDone { get; } = false;

        public Lexicon lexicon { get; set; }

        public Game(List<string> playerNames)
        {
            this.numberOfPlayers = (byte)playerNames.Count;

            bool validNumberOfPlayers = (numberOfPlayers >= MINPLAYERS) && (numberOfPlayers <= MAXPLAYERS); 

            if (!validNumberOfPlayers)
            {
                throw new Exception(MINMAXPLAYERERROR);
            }

            // initialize/choose lexicon
            this.lexicon = new Lexicon();

            // fill tile bag
            this.tileBag = new TileBag();

            // for each player, draw tiles from bag
       
            byte i = 1;

            playerNames.ForEach( name =>
            {
                var player = new Player(name);
                player.DrawTiles(this.tileBag);
                players.Add(i++, player);
            }
            );
        
            // create board
            this.board = new Board();

        }

        public  GameDetails GetDetails()
        {

            var details = new GameDetails();

            details.tileBag = this.tileBag;

       //     details.letterValues = TileBag.GetLetterValues();

            details.rowLabels = Board.GetRowLabels();

            details.colLabels = Board.GetColLabels();

            details.squares = this.board.GetCoordSquares();

            return details;

        }

    }
}
