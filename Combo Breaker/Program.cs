using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;

namespace Combo_Breaker
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputList = new List<string>();
            StreamReader sr = new StreamReader(@"C:\Users\eiedu\Source\Repos\AdventOfCode\Combo Breaker\TextFile1.txt");
            string line = string.Empty;
            while ((line = sr.ReadLine()) != null)
            {
                inputList.Add(line);
            }

            ////(loop size* sjn) mod 20201227
            //var doorPub = Transform(7, 11);
            //var cardPub = Transform(7, 8);
            //Console.WriteLine(doorPub);
            //Console.WriteLine(cardPub);
            //Console.WriteLine(Transform(doorPub, 8));
            //Console.WriteLine(Transform(cardPub, 11));
            //Console.WriteLine(FindLoopSize(cardPub, 7));
            //Console.WriteLine(FindLoopSize(doorPub, 7));
            long cardPub = long.Parse(inputList[0]);
            long doorPub = long.Parse(inputList[1]);
            long LoopSizeCard = FindLoopSize(cardPub, 7);
            long LoopSizeDoor = FindLoopSize(doorPub, 7);
            Console.WriteLine(LoopSizeCard);
            Console.WriteLine(LoopSizeDoor);

            var eKey = Transform(cardPub, (int)LoopSizeDoor);
            var eKey2 = Transform(doorPub, (int)LoopSizeCard);
            Console.WriteLine(eKey);
            Console.WriteLine(eKey2);
        }

        static long Transform(long sjn, long loop)
        {
            long val = 1;
            for (int i = 0; i < loop; i++)
            {
                val *= sjn;
                val %= 20201227;
            }
            return val;
        }

        static long FindLoopSize(long pubKey, long sjn)
        {
            long loopSize = 0;
            long val = 1;
            do
            {
                loopSize++;

                val *= sjn;
                val %= 20201227;
            }
            while (val != pubKey);
            return loopSize;
        }
    }
}
