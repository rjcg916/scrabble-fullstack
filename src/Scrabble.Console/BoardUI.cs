using Scrabble.Domain;

namespace Scrabble.Console
{
    using System;

    class BoardUI(Board board)
    {
        Board Board { get; set; } = board;

        public void DisplayBoard(bool DisplayStatus = true)
        {
            Console.WriteLine("Board"); Console.WriteLine();

            for (int r = 0; r < Coord.RowCount; r++)
            {
                for (int c = 0; c < Coord.ColCount; c++)
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

  //          if (DisplayStatus) 
  //              DisplayBoardStatus();

            Console.WriteLine(); 
            Console.Write("Press RETURN"); 
            Console.ReadLine();
        }
   
        //public void DisplayBoardStatus()
        //{
        //    (bool IsBoardValid, List<PlacementError> InvalidMessage) = Board.//OnlyValidWords();

        //    Console.WriteLine(
        //       $"Is Board Valid? {IsBoardValid}\n{(IsBoardValid ? string.Empty : InvalidMessage.ToString())}");
        //    Console.WriteLine();
        //}
   
        public void PlaceTile(Player player)
        {
            var emptySquares = Board.GetVacantSquares().Select(ls => ls.Coord).ToList();

            var availableLetters = player.Rack.GetTiles().TilesToLetters();

        }
    }
}
