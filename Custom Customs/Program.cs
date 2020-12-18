using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Custom_Customs
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputList = new List<string>();
            StreamReader sr = new StreamReader(@"C:\Users\eiedu\Source\Repos\AdventOfCode\Custom Customs\TextFile1.txt");
            string line = string.Empty;

            int count = 0;
            var set = new SortedSet<char>(); 
            line = sr.ReadLine();
            foreach (var item in line)
            {
                set.Add(item);
            }
            while ((line = sr.ReadLine()) != null)
            {
                if (line == string.Empty)
                {
                    count += set.Count;
                    set = new SortedSet<char>();
                    line = sr.ReadLine();
                    foreach (var item in line)
                    {
                        set.Add(item);
                    }
                }
                else
                {
                    var newSet = new SortedSet<char>();
                    foreach (var item in line)
                    {
                        newSet.Add(item);
                    }
                    Console.WriteLine(set.setToString());
                    Console.WriteLine(newSet.setToString());
                    set.IntersectWith(newSet);
                    Console.WriteLine(set.setToString());
                    Console.WriteLine("===================================");
                }
            }
            count += set.Count;
            Console.WriteLine(count);
        }


    }

    public static class Extensions
    {
        public static string setToString(this SortedSet<char> s)
        {
            StringBuilder sb = new StringBuilder("{");
            foreach (var item in s)
            {
                sb.Append($"{item},");
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append("}");
            return sb.ToString();
        }
    }
}
