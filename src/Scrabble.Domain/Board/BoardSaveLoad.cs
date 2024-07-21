using System;
using System.IO;
using System.Text.Json;

namespace Scrabble.Domain
{
    public partial class Board
    {
        // Method to save the board state to a file
        public void SaveBoardState(string filePath)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                IncludeFields = true
            };

            string jsonString = JsonSerializer.Serialize(this, options);
            File.WriteAllText(filePath, jsonString);
        }

        // Method to load the board state from a file
        public static Board LoadBoardState(string filePath, Func<string, bool> isWordValid)
        {
            var options = new JsonSerializerOptions
            {
                IncludeFields = true
            };

            string jsonString = File.ReadAllText(filePath);
            var board = JsonSerializer.Deserialize<Board>(jsonString, options);

            if (board != null)
            {
                board.IsWordValid = isWordValid;
            }

            return board;
        }
    }
}
