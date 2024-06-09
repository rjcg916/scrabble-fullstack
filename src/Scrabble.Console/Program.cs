using Scrabble.Domain;
using Scrabble.Console;

var lexicon = new Lexicon();

var gameManager = new GameManager();

// create first game
var game = Game.GameFactory.CreateGame(lexicon, 
    new PlayerList( [new("A"), new("B")]));
gameManager.AddGame(game);

// get current board for move
var moveBoard = new Board(game.Board);

// display board
var boardUI = new BoardUI(moveBoard);
boardUI.DisplayBoard(false);

// move list
var moves = new List<List<TilePlacement>>();

// make moves

// make first move
var tiles = new List<TilePlacement>
            {
                new (new Coord(R._8, C.G), new Tile('C')),
                new (new Coord(R._8, C.H), new Tile('A')),
                new (new Coord(R._8, C.I), new Tile('R'))
            };
moveBoard.PlaceTiles(tiles);
//moveBoard.ScoreMove(tiles); recording move, getting tiles, scoring ???
moves.Add(tiles);
boardUI.DisplayBoard();


// trial moves
//   initialize trial move list
// get move
// if cells occupied
//  reject move
// else
//  add move to move list
//  display board/status/score
// while (not done or board invalid) 
// 
// add trial move list to game move list
//
