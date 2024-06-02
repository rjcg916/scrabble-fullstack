using Scrabble.Console;
using Scrabble.Domain;

var board = new Board();

var updateBoard = board.PlaceTilesInARow(new Coord(R._2, C.B), "HiBob".LettersToTiles());

var helper = new BoardUI(updateBoard);

helper.DisplayBoard();

System.Console.WriteLine();
