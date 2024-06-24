namespace Scrabble.WASM.Client.Models
{
    public record ScrabbleMoveModel (int StartRow, int StartColumn, string Letters, bool IsHorizontal);

}
