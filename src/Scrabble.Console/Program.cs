using Scrabble.Domain;
using Scrabble.Console;

var lexicon = new Lexicon();

var board = new Board(lexicon.IsWordValid, new Coord(R._8, C.G), 
                        "cAr".LettersToTiles(), 
                        Placement.Horizontal);

var (isBoardValid, invalidWord)  = board.IsBoardValid();


var helper = new BoardUI(board);

Console.WriteLine(
    $"Is Board Valid? {isBoardValid}\n{(isBoardValid ? string.Empty : invalidWord)}"
);


helper.DisplayBoard();

Console.WriteLine();
