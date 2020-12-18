using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Encoding_Error
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputList = new List<long>();
            StreamReader sr = new StreamReader(@"C:\Users\eiedu\Source\Repos\AdventOfCode\Encoding Error\TextFile1.txt");
            string line = string.Empty;
            while ((line = sr.ReadLine()) != null)
            {
                inputList.Add(long.Parse(line));
            }

            long target = 0;
            var Q = new Queue<long>();
            for (int i = 0; i < 25; i++)
            {
                Q.Enqueue(inputList[i]);
            }

            for (int a = 25; a < inputList.Count; a++)
            {
                var num = inputList[a];
                bool isItem = false;
                var Qa = Q.ToArray();
                for (int i = 0; i < Qa.Length - 1; i++)
                {
                    if (isItem)
                    {
                        break;
                    }
                    for (int j = i; j < Qa.Length; j++)
                    {
                        if (Qa[i] + Qa[j] == num)
                        {
                            isItem = true;
                            break;
                        }
                    }
                }

                if (!isItem)
                {
                    target = num;
                    Console.WriteLine(num);
                    break;
                }

                if (Q.Count >= 25)
                {
                    Q.Dequeue();
                }

                Q.Enqueue(num);

            }

            //found error
            //make contiguous set
            bool end = false;
            for (int i = 0; i < inputList.Count - 1; i++)
            {
                var contiguousList = new List<long>();
                contiguousList.Add(inputList[i]);
                long contiguousSum = inputList[i];
                for (int j = i + 1; j < inputList.Count; j++)
                {
                    contiguousList.Add(inputList[j]);
                    contiguousSum += inputList[j];
                    if (contiguousSum == target)
                    {
                        var min = contiguousList.Min();
                        var max = contiguousList.Max();

                        Console.WriteLine($"sum:{contiguousSum} | {min} + {max} = {min + max}");
                        end = true;
                        break;
                    }
                    else if (contiguousSum > target)
                    {
                        break;
                    }
                }
                if (end)
                {
                    break;
                }
            }



        }
    }
}
