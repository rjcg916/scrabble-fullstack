namespace Scrabble.Domain
{

    public class Player(string name)
    {
        public Rack Rack { get; set; } = new Rack();
        public string Name { get; set; } = name;
        public int Score { get; set; } = 0;
    }

}
