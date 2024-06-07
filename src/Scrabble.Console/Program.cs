using Scrabble.Domain;
using Scrabble.Console;

var lexicon = new Lexicon();

var board = new Board(lexicon.IsWordValid);

var boardUI = new BoardUI(board);

boardUI.DisplayBoard();

Console.WriteLine(); Console.Write("Press RETURN"); Console.ReadLine();

var tiles = new List<(Coord, Tile)>
            {
                (new Coord(R._8, C.G), new Tile('C')),
                (new Coord(R._8, C.H), new Tile('A')),
                (new Coord(R._8, C.I), new Tile('R'))
            };
board.PlaceTiles(tiles);


boardUI.DisplayBoard();
boardUI.DisplayBoardStatus();

Console.WriteLine(); Console.Write("Press RETURN"); Console.ReadLine();