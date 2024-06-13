using Scrabble.Domain;
using Scrabble.Console;
using System.Diagnostics.Metrics;

var lexicon = new Lexicon();

var gameManager = new GameManager();

// create first game
var game = Game.GameFactory.CreateGame(lexicon, 
    new PlayerList( [new("A"), new("B")]));
gameManager.AddGame(game);


// get current board for move
var moveBoard = new Board(game.Board);


// get rack for current player
var currentRack = game.Players[game.TurnOfPlayer].Rack;
var currentLetters = currentRack.GetTiles().Select( t => t.Letter);
var rackUI = new RackUI(currentRack);

// display board
var boardUI = new BoardUI(moveBoard);
boardUI.DisplayBoard(false);

// display rack
rackUI.DisplayRack();

var emptySquares = game.Board.GetLocationSquares().Select( ls => ls.Coord);

// place tiles until valid and done
var makeMove = false;
var theMove = new List<TilePlacement>();

do
{
    bool validLetter;
    char theLetter;

    do
    {
        Console.Write("Tile:");
        theLetter = char.Parse( Console.ReadLine());

        validLetter = currentLetters.Any(l => l.Equals(theLetter));
    } while (!validLetter);

    bool validLocation;
    Coord validCoord;
    do
    {
        Console.Write("Row:");
        var rowStr = Console.ReadLine();
        var row = int.Parse(rowStr);

        Console.Write("Col:");
        var colStr = Console.ReadLine();
        var col = int.Parse(colStr);

        validCoord = new Coord((R) row, (C) col);

        validLocation = emptySquares.Any();
    } while (!validLocation);

    theMove.Add(new TilePlacement(validCoord, new Tile(theLetter)));

    Console.Write("Make Move (yes/no)?");
    var makeMoveStr = Console.ReadLine();
    makeMove = bool.Parse(makeMoveStr);

} while (!makeMove);

// display tiles


//// move list
//var moves = new List<List<TilePlacement>>();

//// make moves

//// make first move
//var tiles = new List<TilePlacement>
//    {
//        new (new Coord(R._8, C.G), new Tile('C')),
//        new (new Coord(R._8, C.H), new Tile('A')),
//        new (new Coord(R._8, C.I), new Tile('R'))
//    };


//moveBoard.PlaceTiles(tiles);
////moveBoard.ScoreMove(tiles); recording move, getting tiles, scoring ???
//moves.Add(tiles);

//boardUI.DisplayBoard();
