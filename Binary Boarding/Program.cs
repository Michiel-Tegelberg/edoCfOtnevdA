using System;
using System.Collections.Generic;
using System.IO;

namespace Binary_Boarding
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputList = new List<string>();
            StreamReader sr = new StreamReader(@"C:\Users\eiedu\Source\Repos\AdventOfCode\Binary Boarding\TextFile1.txt");
            string line = string.Empty;
            while((line = sr.ReadLine()) != null)
            {
                inputList.Add(line);
            }

            var plan = new bool[1000];

            int hid = 0;
            foreach (var item in inputList)
	        {
                int id = Row(item) * 8 + Column(item);
                plan[id] = true;
                if (id > hid)
                {
                    hid = id;
                }
                Console.WriteLine($"{item} | row: {Row(item),4} | col: {Column(item),2}");
	        }
            Console.WriteLine(hid);

            for (int i = 0; i < plan.Length; i++)
            {
                if (!plan[i])
                {
                    Console.WriteLine(i);
                }
            }
        }


        static int Row(string pass)
        {
            int row = 0;
            for (int i = 0; i < 7; i++)
            {
                row <<= 1;
                if (pass[i] == 'B')
                {
                    row = row | 1;
                }
            }
            return row;
        }

        static int Column(string pass)
        {
            int col = 0;
            for (int i = 7; i < 10; i++)
            {
                col <<= 1;
                if (pass[i] == 'R')
                {
                    col = col | 1;
                }
            }
            return col;
        }
    }
}
