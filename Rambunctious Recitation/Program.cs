using System;
using System.Collections.Generic;
using System.IO;

namespace Rambunctious_Recitation
{
    class Program
    {
        static void Main(string[] args)
        {

            #region Prep
            var inputList = new List<string>();
            StreamReader sr = new StreamReader(@"C:\Users\eiedu\Source\Repos\AdventOfCode\Rambunctious Recitation\TextFile1.txt");
            string line = string.Empty;
            while ((line = sr.ReadLine()) != null)
            {
                inputList.Add(line);
            }

            var dict = new Dictionary<int, int>();
            int last = 0;
            var splitLine = inputList[0].Split(',');
            for (int i = 0; i < splitLine.Length; i++)
            {
                last = int.Parse(splitLine[i]);
                dict.Add(last, i + 1);
                //Console.WriteLine($"{last}:{i + 1}");
            }
            #endregion


            var start = DateTime.Now;
            int p = splitLine.Length + 1;
            for (int turn = p; turn <= 30000000; turn++)
            {
                //consider last and then add it to the dict

                if (dict.ContainsKey(last))
                {
                    int occurrence = dict[last];
                    int diff = turn - 1 - occurrence;
                    dict[last] = turn - 1;
                    last = diff;
                }
                else
                {
                    dict.Add(last, turn - 1);
                    last = 0;
                }

            }
            var end = DateTime.Now;
            
            Console.WriteLine($"{last}");
            Console.WriteLine($"took {(end - start).TotalMilliseconds}ms");
        }
    }
}
