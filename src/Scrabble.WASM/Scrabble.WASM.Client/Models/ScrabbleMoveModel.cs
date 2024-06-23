namespace Scrabble.WASM.Client.Models
{
    public class ScrabbleMoveModel
    {
        public int StartRow { get; set; }
        public int StartColumn { get; set; }
        public string Letters { get; set; }
        public bool IsHorizontal { get; set; }
    }
}
