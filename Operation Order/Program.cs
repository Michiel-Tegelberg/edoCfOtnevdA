using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Operation_Order
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputList = new List<string>();
            //StreamReader sr = new StreamReader(@"C:\Users\eiedu\Source\Repos\AdventOfCode\Operation Order\TextFile2.txt");
            StreamReader sr = new StreamReader(@"C:\Users\eiedu\Source\Repos\AdventOfCode\Operation Order\TextFile1.txt");
            string line = string.Empty;
            while ((line = sr.ReadLine()) != null)
            {
                inputList.Add(line);
            }

            long total = 0;
            foreach (var sum in inputList)
            {
                total += Solve(Tokenize(sum));
            }
            Console.WriteLine(total);
        }

        public static List<string> Tokenize(string s)
        {
            //cut it up
            var tokens = new List<string>();
            string token = string.Empty;
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == '(')
                {
                    tokens.Add("(");
                }
                else if (s[i] == ')')
                {
                    if (token != string.Empty)
                    {
                        tokens.Add(token);
                        token = string.Empty;
                    }
                    tokens.Add(")");
                }
                else if (s[i] == ' ')
                {
                    if (token != string.Empty)
                    {
                        tokens.Add(token);
                        token = string.Empty;
                    }
                }
                else
                {
                    token += s[i];
                }
            }
            if (token != string.Empty)
            {
                tokens.Add(token);
            }
            return tokens;
        }

        public static long Solve(List<string> tokens)
        {
            bool changes = true;
            while (changes)
            {
                changes = false;
                for (int i = 0; i < tokens.Count; i++)
                {
                    if (tokens[i] == "(")
                    {
                        long opened = 1;
                        for (int j = i + 1; j < tokens.Count; j++)
                        {
                            if (tokens[j] == "(")
                            {
                                opened++;
                            }
                            else if (tokens[j] == ")")
                            {
                                opened--;
                            }
                            if (opened == 0)
                            {
                                changes = true;
                                int count = j - i;
                                long val = Solve(tokens.GetRange(i + 1, count - 1));
                                tokens[i] = val.ToString();
                                tokens.RemoveRange(i + 1, count);
                                break;
                            }
                        }
                    }
                    if (changes)
                    {
                        break;
                    }
                }
            }

            //the formula is now just x +/* y (*/+ z)? so I can resolve all + first
            changes = true;
            while (changes)
            {
                changes = false;
                for (int i = 0; i < tokens.Count; i++)
                {
                    if (tokens[i] == "+")
                    {
                        changes = true;
                        long val = long.Parse(tokens[i - 1]) + long.Parse(tokens[i + 1]);
                        tokens[i - 1] = val.ToString();
                        tokens.RemoveRange(i, 2);
                        break;
                    }
                }
            }

            for (int i = 0; i < tokens.Count - 2; i += 2)
            {
                long LOp = long.Parse(tokens[i]);
                string Op = tokens[i + 1];
                long ROp = long.Parse(tokens[i + 2]);

                if (Op == "+")
                {
                    tokens[i + 2] = (LOp + ROp).ToString();
                }
                else
                {
                    tokens[i + 2] = (LOp * ROp).ToString();
                }
            }
            return long.Parse(tokens[tokens.Count - 1]);
        }
    }

    public static class Extensions
    {
        public static string StringList(this List<string> l)
        {
            StringBuilder sb = new StringBuilder("{");
            foreach (var item in l)
            {
                sb.Append($"{item},");
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append("}");
            return sb.ToString();
        }
    }
}