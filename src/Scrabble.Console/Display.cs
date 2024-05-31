using Scrabble.Domain;


namespace Scrabble.Console
{
    class BoardHelper(Board board)
    {
        Board Board { get; set; } = board;

        public void DisplayBoard()
        {
            for (int r = 0; r < Board.rowCount; r++)
            {
                for (int c = 0; c < Board.colCount; c++)
                {
                    var tile = Board.squares[r, c].Tile;
                    if (tile != null)
                    {
                        System.Console.Write(tile.Letter);
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
