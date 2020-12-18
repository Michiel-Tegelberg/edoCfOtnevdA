using System;
using System.Collections.Generic;
using System.IO;

namespace Password_Philosophy
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Password> numbers = new List<Password>();
            StreamReader sr = new StreamReader(@"C:\Users\eiedu\Source\Repos\AdventOfCode\Password Philosophy\TextFile1.txt");
            string line = string.Empty;
            while ((line = sr.ReadLine()) != null)
            {
                var splitline = line.Split(' ');
                string range = splitline[0];
                int low = int.Parse(range.Split('-')[0]);
                int high = int.Parse(range.Split('-')[1]);
                char letter = splitline[1][0];
                string pass = splitline[2];
                numbers.Add(new Password(low, high, letter, pass));
            }

            int count = 0;
            foreach (var item in numbers)
            {
                if (item.TobogganCorporateValid())
                {
                    count++;
                }
            }

            Console.WriteLine(count);
        }
    }
}
