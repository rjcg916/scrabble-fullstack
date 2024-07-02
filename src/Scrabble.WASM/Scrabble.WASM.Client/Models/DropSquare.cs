using MudBlazor;

namespace Scrabble.WASM.Client.Models
{

    public class DropSquare
    {
        public char Name { get; init; }
        public string Identifier { get; set; }
        public int Value { get; init; }
        public bool IsLocked { get; set; }
    }
}
