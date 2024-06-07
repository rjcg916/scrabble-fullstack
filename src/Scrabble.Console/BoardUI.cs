using Scrabble.Domain;


namespace Scrabble.Console
{
    class BoardUI(Board board)
    {
        Board Board { get; set; } = board;

        public void DisplayBoard()
        {
            for (int r = 0; r < Board.rowCount; r++)
            {
                for (int c = 0; c < Board.colCount; c++)
                {
                    var square = Board.squares[r, c];               
                    if (square.IsOccupied)
                    {
                        System.Console.Write(square.Tile.Letter);
                    }
                    else
                    {
                        System.Console.Write("."); // Use a dot to represent an empty square
                    }
                }
                System.Console.WriteLine();
            }
        }
    }
}
