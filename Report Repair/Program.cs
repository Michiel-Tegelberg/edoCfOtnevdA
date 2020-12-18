using System;
using System.Collections.Generic;
using System.IO;

namespace Report_Repair
{
    class Program
    {
        static void Main(string[] args)
        {
            List<int> numbers = new List<int>();
            StreamReader sr = new StreamReader(@"C:\Users\eiedu\Source\Repos\AdventOfCode\Report Repair\TextFile1.txt");
            string line = string.Empty;
            while((line = sr.ReadLine()) != null)
            {
                numbers.Add(int.Parse(line));
            }

            for (int i = 0; i < numbers.Count - 1; i++)
            {
                for (int j = i + 1; j < numbers.Count; j++)
                {
                    if (numbers[i] + numbers[j] == 2020)
                    {
                        Console.WriteLine(numbers[i]);
                        Console.WriteLine(numbers[j]);
                        Console.WriteLine(numbers[i] * numbers[j]);
                    }
                }
            }


            for (int i = 0; i < numbers.Count - 2; i++)
            {
                for (int j = i + 1; j < numbers.Count - 1; j++)
                {
                    for (int k = j + 1; k < numbers.Count; k++)
                    {
                        if (numbers[i] + numbers[j] + numbers[k] == 2020)
                        {
                            Console.WriteLine(numbers[i]);
                            Console.WriteLine(numbers[j]);
                            Console.WriteLine(numbers[k]);
                            Console.WriteLine(numbers[i] * numbers[j] * numbers[k]);
                        } 
                    }
                }
            }
        }
    }
}
