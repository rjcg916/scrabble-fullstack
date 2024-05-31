using Scrabble.Console;
using Scrabble.Domain;

var board = new Board();

var updateBoard = board.PlaceTilesInARow(new Coord(R._2, C.B), "HiBob");

var helper = new BoardHelper(updateBoard);

helper.DisplayBoard();

System.Console.WriteLine();
