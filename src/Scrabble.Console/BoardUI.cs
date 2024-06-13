using Scrabble.Domain;

namespace Scrabble.Console
{
    using System;

    class BoardUI(Board board)
    {
        Board Board { get; set; } = board;

        public void DisplayBoard(bool DisplayStatus = true)
        {            
            for (int r = 0; r < Board.rowCount; r++)
            {
                for (int c = 0; c < Board.colCount; c++)
                {
                    var square = Board.squares[r, c];               
                    if (square.IsOccupied)
                    {
                        Console.Write(square.Tile.Letter);
                    }
                    else
                    {
                        Console.Write("."); // Use a dot to represent an empty square
                    }
                }
                Console.WriteLine();
            }

            if (DisplayStatus) 
                DisplayBoardStatus();

            Console.WriteLine(); 
            Console.Write("Press RETURN"); 
            Console.ReadLine();
        }
   
        public void DisplayBoardStatus()
        {
            (bool IsBoardValid, List<PlacementError> InvalidMessage) = Board.IsBoardValid();

            Console.WriteLine(
               $"Is Board Valid? {IsBoardValid}\n{(IsBoardValid ? string.Empty : InvalidMessage.ToString())}");
            Console.WriteLine();
        }
   
        public void PlaceTile(Player player)
        {
            var emptySquares = Board.GetLocationSquares().Select(ls => ls.Coord).ToList();

            var availableLetters = player.Rack.GetTiles().TilesToLetters();

        }
    }
}
