using System;
using System.Collections.Generic;
using System.IO;

namespace Adapter_Array
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputList = new List<int>();
            StreamReader sr = new StreamReader(@"C:\Users\eiedu\Source\Repos\AdventOfCode\Adapter Array\TextFile1.txt");
            string line = string.Empty;
            while((line = sr.ReadLine()) != null)
            {
                inputList.Add(int.Parse(line));
            }

            inputList.Sort();

            var looseLists = new List<List<int>>();
            var newList = new List<int>();

            int last = 0;
            int ones = 0;
            int threes = 0;
            foreach (var item in inputList)
            {
                newList.Add(item);
                int diff = item - last;
                if (diff == 3)
                {
                    threes++;
                    Console.WriteLine($"  |");
                }
                if (diff == 1)
                {
                    ones++;
                }
                Console.WriteLine($"{item,3}");
                if (diff == 3)
                {

                    looseLists.Add(newList);
                    newList = new List<int>();
                    //Console.WriteLine("*snip*");
                }
                last = item;
            }
            looseLists.Add(newList);


            int longest = 0;
            long sum = 1;
            foreach (var L in looseLists)
            {
                longest = L.Count > longest ? L.Count : longest;
                switch (L.Count)
                {
                    case 3:
                        sum *= 2;
                        break;
                    case 4:
                        sum *= 4;
                        break;
                    case 5:
                        sum *= 7;
                        break;
                    case 6:
                        sum *= 7;
                        break;
                    default:
                        break;
                }
            }

            Console.WriteLine($"Threes: {threes}");
            Console.WriteLine($"Ones: {ones}");
            Console.WriteLine($"{threes * ones}");
            Console.WriteLine($"{looseLists.Count} in looselists");
            Console.WriteLine($"Longest: {longest}");
            Console.WriteLine(sum);
        }
    }
}
