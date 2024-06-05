using Scrabble.Domain;
using Scrabble.Console;

var lexicon = new Lexicon();

var board = new Board(lexicon.IsWordValid, new Coord(R._7, C.G), 
                        "AB".LettersToTiles(), 
                        Placement.Horizontal  );

var helper = new BoardUI(board);

helper.DisplayBoard();

Console.WriteLine();
