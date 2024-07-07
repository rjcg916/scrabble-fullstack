namespace Scrabble.WASM.Client.Models
{    
    public record ScrabbleMove (int StartRow, int StartColumn, string Letters, bool IsHorizontal);
}
