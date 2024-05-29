namespace Scrabble.Domain
{
    public class Tile(char letter, ushort Value = 1)
    {
        public char Letter { get; } = char.ToUpper(letter);
        public ushort Value { get; } = Value;
    }
}