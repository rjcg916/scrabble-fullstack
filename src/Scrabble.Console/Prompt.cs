using Scrabble.Domain;

namespace Scrabble.Console
{
    using System;

    public static class Prompt
    {

        //var x = GetValidLetter(new List<char>() { 'A', 'C' });
        //Console.WriteLine(x);

        public static char GetValidLetter(List<char> currentLetters)
        {
            char validLetter;
            bool isLetterValid;
            do
            {
                Console.Write("Letter: ");
                string input = Console.ReadLine().ToUpper();
                if (char.TryParse(input, out validLetter))
                {
                    isLetterValid = currentLetters.Any(l => l.Equals(validLetter));
                    if (!isLetterValid)
                    {
                        Console.WriteLine("Invalid letter. Please try again.");
                    }
                }
                else
                {
                    isLetterValid = false;
                    Console.WriteLine("Invalid input. Please enter a single letter.");
                }
            } while (!isLetterValid);

            return validLetter;
        }

        //var x = GetValidLocation(new List<Coord>() { new Coord(R._1, C.A), new Coord(R._2, C.B) });
        //Console.WriteLine(x);
        public static Coord GetValidLocation(List<Coord> emptySquares)
        {
            Coord validLocation = emptySquares.First();

            bool isLocationValid = false;
            do
            {
                Console.Write("Row: ");
                string rowStr = Console.ReadLine();
                Console.Write("Col: ");
                string colStr = Console.ReadLine();

                if (int.TryParse(rowStr, out int row) && int.TryParse(colStr, out int col))
                {
                    validLocation = new Coord((R)row, (C)col);
                    var es = emptySquares.Select(s => s.ToString());
                    isLocationValid = es.Contains(validLocation.ToString());
                    if (!isLocationValid)
                    {
                        Console.WriteLine("Invalid location. Please try again.");
                    }
                }
                else
                {
                    isLocationValid = false;
                    Console.WriteLine("Invalid input. Please enter numeric values for row and column.");
                }
            } while (!isLocationValid);

            return validLocation;
        }

        //var x = AskToMakeMove();
        //Console.WriteLine(x);
        public static bool AskToMakeMove()
        {
            bool validInput;
            do
            {
                Console.Write("Make Move (yes/no)? ");
                string response = Console.ReadLine().Trim().ToLower();
                if (response == "yes")
                {
                    return true;
                }
                else if (response == "no")
                {
                    return false;
                }
                else
                {
                    validInput = false;
                    Console.WriteLine("Invalid input. Please enter 'yes' or 'no'.");
                }
            } while (!validInput);

            return false;
        }
    }
}