using Scrabble.Console;
using Scrabble.Domain;

var lexicon = new Lexicon();

var gameManager = new GameManager();

//// create first game
var game = Game.GameFactory.CreateGame(lexicon, 
    new List<Player>( [new("A"), new("B")]));
gameManager.AddGame(game);

//// get current board for move
var moveBoard = new Board(game.Board);

//// get rack for current player
var currentRack = game.Players.CurrentPlayer.Rack;
var currentLetters = currentRack.GetTiles().Select( t => t.Letter).ToList();
var rackUI = new RackUI(currentRack);

//// display board
var boardUI = new BoardUI(moveBoard);
boardUI.DisplayBoard(false);

//// display rack
rackUI.DisplayRack();

var emptySquares = game.Board.GetVacantSquares().Select( ls => ls.Coord).ToList();

