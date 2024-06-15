using Scrabble.Domain;

namespace Scrabble.Console
{
    using System;

    class RackUI(Rack rack)
    {
        Rack Rack { get; set; } = rack;

        public void DisplayRack()
        {          
            Console.WriteLine(
                Rack.GetTiles().Select(  t => $"[{t.Letter}]").ToList().ToString()
            );            
        }
    }
}