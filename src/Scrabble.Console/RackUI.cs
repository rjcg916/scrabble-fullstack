﻿using Scrabble.Domain;

namespace Scrabble.Console
{
    using System;

    class RackUI(Rack rack)
    {
        Rack Rack { get; set; } = rack;

        public void DisplayRack()
        {
            Console.WriteLine("Tiles"); Console.WriteLine();
            Console.WriteLine(
             Rack.GetTiles().TilesToLetters()
            );            
        }
    }
}