using Scrabble.Domain;

namespace Scrabble.WASM.Client.Helpers
{
    public static class DropZoneId
    {

        // Determines if the identifier corresponds to a rack slot
        public static bool IsOnRack(string id) =>
            id.Length == 1;

        // Determines if the identifier corresponds to a board position
        public static bool IsOnBoard(string id) =>
            id.Length == 4;

        // Parses a board identifier to get the row and column coordinates
        public static Coord GetCoord(string id) =>
            new Coord(int.Parse(id.Substring(0, 2)), int.Parse(id.Substring(2, 2)));

        // Generates a rack slot identifier from a slot index
        public static string ToId(int slot) =>
            slot.ToString("D1");

        // Generates a board position identifier from row and column values
        public static string ToId(int row, int col)
        {
            const string displayFormat = "D2";
            return $"{row.ToString(displayFormat)}{col.ToString(displayFormat)}";
        }
    }
}
