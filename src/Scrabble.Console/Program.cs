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

//// place tiles until valid and done

//var makeMove = false;
//bool moveValid = false;
//var theMove = new List<TilePlacement>();

//do // get letters/locations until done and resulting move valid
//{
//    char theLetter = GetValidLetter(currentLetters);
//    Coord validCoord = GetValidLocation(emptySquares);

//    theMove.Add(new TilePlacement(validCoord, new Tile(theLetter)));

//    (moveValid, var errorList) = moveBoard.IsMoveValid(theMove);
//    if (!moveValid)
//    {
//        // display error
//    }
//    else
//    {
//        // determine if done
//        makeMove = AskToMakeMove();
//    }

//} while (!makeMove && !moveValid);


// make move on original board and score


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

// generate a random move using all available tiles in vacant spaces
//static List<TilePlacement> GetMoveRandom(List<Tile> availableTiles, List<Coord> availableSpaces)
//{
//    var tilePlacements = new List<TilePlacement>();

//    var coord = 0;            
//    foreach (var tile in availableTiles)
//    {
//        tilePlacements.Add(new TilePlacement(availableSpaces[coord], tile));                    
//        coord++;
//    }

//    return tilePlacements;
//}