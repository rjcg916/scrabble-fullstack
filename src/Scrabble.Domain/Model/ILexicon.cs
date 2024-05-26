namespace Scrabble.Domain.Model
{
    interface ILexicon
    {
        bool IsWordValid(string word);
    }
}
