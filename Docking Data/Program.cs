using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Docking_Data
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputList = new List<string>();
            StreamReader sr = new StreamReader(@"C:\Users\eiedu\Source\Repos\AdventOfCode\Docking Data\TextFile1.txt");
            //StreamReader sr = new StreamReader(@"C:\Users\eiedu\Source\Repos\AdventOfCode\Docking Data\TextFile2.txt");

            string line = string.Empty;
            while ((line = sr.ReadLine()) != null)
            {
                inputList.Add(line);
            }

            //PartOne(inputList);

            PartTwo(inputList);
        }

        private static void PartTwo(List<string> inputList)
        {
            string mask = string.Empty;
            var mem = new Dictionary<long, long>();
            foreach (var item in inputList)
            {
                //Console.WriteLine(item);
                string index = string.Empty;
                var splitLine = item.Split('=');
                if (splitLine[0].Trim() == "mask")
                {
                    mask = splitLine[1].Trim();
                }
                else
                {
                    index = $"{splitLine[0].Trim().Substring(4).Trim(']'),36}";
                    int val = int.Parse(splitLine[1].Trim());
                    index = $"{Convert.ToString(int.Parse(index),2),36}";
                    //or mask and index
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < 36; i++)
                    {
                        if (mask[i] == 'X')
                        {
                            sb.Append('X');
                        }
                        else if (mask[i] == '1' || index[i] == '1')
                        {
                            sb.Append('1');
                        }
                        else if (mask[i] == '0' && index[i] == '1')
                        {
                            sb.Append('1');
                        }
                        else
                        {
                            sb.Append('0');
                        }
                    }
                    //now write these values to memory
                    var m = sb.ToString();
                    var l = GetMemAdds(m, 0, 0);
                    //Console.WriteLine($"{m} gives {l.Count} addresses");
                    foreach (var address in l)
                    {
                        //Console.WriteLine(address);
                        if (mem.ContainsKey(address))
                        {
                            mem[address] = val;
                        }
                        else
                        {
                            mem.Add(address, val);
                        }
                    }
                }
            }

            //Console.WriteLine(  mem.Poop());
            
            long sum = 0;
            foreach (var item in mem.Values)
            {
                sum += item;
            }
            Console.WriteLine(sum);
        }

        static List<long> GetMemAdds(string mask, long res, int place)
        {
            //Console.WriteLine($"{mask,36} | {Convert.ToString(res,2),36} | {res} | {place}");
            var ret = new List<long>();
            if (place > 35)
            {
                return new List<long> { res };
            }
            //build a list recursively by building the strings and branching at every X
            res <<= 1;
            if (mask[place] == '1')
            {
                ret.AddRange(GetMemAdds(mask, res + 1, place + 1));
            }
            else if (mask[place] == '0')
            {
                ret.AddRange(GetMemAdds(mask, res, place + 1));
            }
            else
            {
                //branch 1
                var branchOne = GetMemAdds(mask, res + 1, place + 1);
                ret.AddRange(branchOne);
                //branch 0
                var branchTwo = GetMemAdds(mask, res, place + 1);
                ret.AddRange(branchTwo);
            }
            return ret;
        }

        private static void PartOne(List<string> inputList)
        {
            string mask = string.Empty;
            var mem = new Dictionary<long, long>();
            foreach (var item in inputList)
            {
                int index = 0;
                var splitLine = item.Split('=');
                if (splitLine[0].Trim() == "mask")
                {
                    mask = splitLine[1].Trim();
                }
                else
                {
                    index = int.Parse(splitLine[0].Trim().Substring(4).Trim(']'));
                    int val = int.Parse(splitLine[1].Trim());
                    string v = $"{Convert.ToString(val, 2),36}";
                    //Console.WriteLine(v);

                    long res = Mask(mask, v);

                    if (mem.ContainsKey(index))
                    {
                        mem[index] = res;
                    }
                    else
                    {
                        mem.Add(index, res);
                    }
                }
            }

            long sum = 0;
            foreach (var item in mem.Values)
            {
                sum += item;
            }
            Console.WriteLine(sum);
        }

        private static long Mask(string mask, string v)
        {
            //bitshift val over mask from left to right
            long res = 0;
            for (int i = 0; i < 36; i++)
            {
                res <<= 1;
                char c = mask[i];
                if (c == '1')
                {
                    res += 1;
                }
                else if (v[i] == '1' && c == 'X')
                {
                    res += 1;
                }
            }

            return res;
        }
    }

    public static class dictExtension
    {
        public static string Poop(this Dictionary<long, long> a)
        {
            StringBuilder sb = new StringBuilder("");
            foreach (var item in a)
            {
                sb.Append($"{{{Convert.ToString(item.Key,2),36}:{item.Value,5}}}\n");
            }
            return sb.ToString();
        }
    }
}
