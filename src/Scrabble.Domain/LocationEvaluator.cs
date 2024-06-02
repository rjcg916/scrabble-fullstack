namespace Scrabble.Domain
{
    public class LocationEvaluator<T>(int row, int col, T evaluator = default)
    {
        public T Evaluator { get; set; } = evaluator;
        public int Row { get; set; } = row;
        public string RowName { get; set; } = ((R)row).ToString()[1..];
        public int Col { get; set; } = col;
        public string ColName { get; set; } = ((C)col).ToString()[0..];
    }
}
