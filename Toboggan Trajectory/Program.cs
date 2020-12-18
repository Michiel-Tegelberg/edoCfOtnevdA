using System;
using System.Collections.Generic;
using System.IO;

namespace Toboggan_Trajectory
{
    class Program
    {
        static void Main(string[] args)
        {
            var numbers = new List<string>();
            StreamReader sr = new StreamReader(@"C:\Users\eiedu\Source\Repos\AdventOfCode\Toboggan Trajectory\TextFile1.txt");
            string line = string.Empty;
            while ((line = sr.ReadLine()) != null)
            {
                numbers.Add(line);
            }

            int width = numbers[0].Length;
            int height = numbers.Count;
            //Console.WriteLine($"width: {width,3} | height: {height,3}");

            int total = 1;
            bool stop = false;
            for (int down = 1; down < 3; down++)
            {
                if (stop)
                {
                    break;
                }
                for (int right = 1; right < 9; right += 2)
                {
                    int count = 0;
                    int x = 0;
                    for (int y = 0; y < height; y += down)
                    {
                        if (numbers[y][x] == '#')
                        {
                            count++;
                        }

                        x = (x + right) % width;
                    }
                    total *= count;
                    Console.WriteLine($"{count,5} | right:{right,2} | down:{down}");
                    if (down == 2)
                    {
                        stop = true;
                    }
                    if (stop)
                    {
                        break;
                    }
                } 
            }
            Console.WriteLine(total);
        }
    }
}
