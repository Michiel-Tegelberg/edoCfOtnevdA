using System;
using System.Collections.Generic;
using System.IO;

namespace Shuttle_Search
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputList = new List<string>();
            StreamReader sr = new StreamReader(@"C:\Users\eiedu\Source\Repos\AdventOfCode\Shuttle Search\TextFile1.txt");
            //StreamReader sr = new StreamReader(@"C:\Users\eiedu\Source\Repos\AdventOfCode\Shuttle Search\TextFile2.txt");
            string line = string.Empty;
            while ((line = sr.ReadLine()) != null)
            {
                inputList.Add(line);
            }

            int start = int.Parse(inputList[0]);
            var splitline = inputList[1].Split(',');
            var Busses = new List<int>();
            for (int i = 0; i < splitline.Length; i++)
            {
                var item = splitline[i];
                if (item != "x")
                {
                    int busId = int.Parse(item);
                    Console.WriteLine($"{i,3}|{busId}");
                    Busses.Add(busId);
                }
            }


            bool found = false;
            long m = 143758579;
            long n = 12164659;
            long k = 100000000000000 / m;
            while (!found)
            {
                long t = (k + 1) * m;

                if (t % n == (n -31))
                {
                    found = true;
                }
                k++;

            }
            Console.WriteLine(k * m - 17);

            //https://i.imgur.com/2IB9dPI.png revelation

            //foreach (var Bus in Busses)
            //{
            //    int ttw = start % Bus;
            //    int trips = start / Bus;
            //    Console.WriteLine($"{Bus,3} leaves in {(Bus - ttw),3} after {start} at {(Bus - ttw) + start,7}, having done {trips} trips in {trips * Bus}, {Bus * (Bus - ttw)}");
            //}
        }
    }
}
