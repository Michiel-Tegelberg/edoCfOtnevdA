using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Monster_Messages
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputList = new List<List<string>>();
            //StreamReader sr = new StreamReader(@"C:\Users\eiedu\Source\Repos\AdventOfCode\Monster Messages\TextFile6.txt");
            //StreamReader sr = new StreamReader(@"C:\Users\eiedu\Source\Repos\AdventOfCode\Monster Messages\TextFile5.txt");
            //StreamReader sr = new StreamReader(@"C:\Users\eiedu\Source\Repos\AdventOfCode\Monster Messages\TextFile4.txt");
            //StreamReader sr = new StreamReader(@"C:\Users\eiedu\Source\Repos\AdventOfCode\Monster Messages\TextFile3.txt");
            //StreamReader sr = new StreamReader(@"C:\Users\eiedu\Source\Repos\AdventOfCode\Monster Messages\TextFile2.txt");
            StreamReader sr = new StreamReader(@"C:\Users\eiedu\Source\Repos\AdventOfCode\Monster Messages\TextFile1.txt");
            string line = string.Empty;
            var subList = new List<string>();
            while ((line = sr.ReadLine()) != null)
            {
                if (line == string.Empty)
                {
                    inputList.Add(new List<string>(subList));
                    subList.Clear();
                }
                else
                {
                    subList.Add(line);
                }
            }
            inputList.Add(subList);
            
            //make mega regex v0v


            //SolveWithReplacement(inputList);

            SolveWithRules(inputList);
        }

        private static void SolveWithReplacement(List<List<string>> inputList)
        {


            //make rules
            var rules = new List<TextRule>();
            foreach (var item in inputList[0])
            {
                var colonSplit = item.Split(':');
                int num = int.Parse(colonSplit[0]);
                rules.Add(new TextRule(num));
            }

            foreach (var item in inputList[0])
            {
                var colonSplit = item.Split(':');
                int num = int.Parse(colonSplit[0].Trim());
                TextRule r = rules.Find(n => n.Num == num);
                var subrules = colonSplit[1].Trim().Split('|');
                r.ReplacesA = $",{subrules[0].Trim(' ').Trim('"').Replace(' ', ',')},";

                if (subrules.Length > 1)
                {
                    r.ReplacesB = $",{subrules[1].Trim(' ').Replace(' ', ',')},";
                }

                Console.WriteLine(r);
            }

            //string replace = "0,1,2,3,4,5,6,7,8,9";
            //string replacer = "A";
            //int lengthA = "3,4".Length;
            //Console.WriteLine(replace);
            ////Console.WriteLine($"replace[0..7] = {replace[0..7]}");
            ////Console.WriteLine($"replace[8..] = {replace[8..]}");
            ////Console.WriteLine($"replace[0..7] + \"a\" + replace[8..] = {replace[0..7] + replacer + replace[(7 + replacer.Length)..]}");

            //for (int i = 0; i < replace.Length - lengthA; i++)
            //{
            //    if (replace.Substring(i, lengthA) == "3,4")
            //    {
            //        Console.WriteLine(replace[0..i] + replacer + replace[(i + lengthA)..]);
            //    }
            //}

            StringBuilder sb = new StringBuilder(",");
            foreach (var c in inputList[1][0])
            {
                if (c == 'a')
                {
                    sb.Append($"1,");
                }
                else
                {
                    sb.Append($"14,");
                }
            }
            var possibilities = new HashSet<string> { sb.ToString() };



            bool changes = true;
            while (changes)
            {
                changes = false;
                var newList = new HashSet<string>();
                foreach (var rule in rules)
                {
                    foreach (var target in possibilities)
                    {
                        string replacer = $",{rule.Num},";

                        int lengthA = rule.ReplacesA.Length;
                        for (int i = 0; i < target.Length - lengthA; i++)
                        {
                            if (target.Substring(i, lengthA) == rule.ReplacesA)
                            {
                                newList.Add(target[0..i] + replacer + target[(i + lengthA)..]);
                            }
                        }

                        if (rule.ReplacesB != string.Empty)
                        {
                            int lengthB = rule.ReplacesB.Length;
                            for (int i = 0; i < target.Length - lengthB; i++)
                            {
                                if (target.Substring(i, lengthB) == rule.ReplacesB)
                                {
                                    newList.Add(target[0..i] + replacer + target[(i + lengthB)..]);
                                }
                            }
                        }
                    }
                }

                //check if there's a ,0, in newList, if true. break;
                bool end = false;
                foreach (var item in newList)
                {
                    if (item == ",0,")
                    {
                        end = true;
                        break;
                    }
                }
                if (end)
                {
                    //return true
                    Console.WriteLine("found ,0,");
                    break;
                }
                possibilities = new HashSet<string>(newList);
                newList.Clear();
                changes = true;
            }

            foreach (var item in possibilities)
            {
                Console.WriteLine(item);
            }
        }

        private static void SolveWithRules(List<List<string>> inputList)
        {

            //make rules
            var rules = new List<Rule>();
            foreach (var item in inputList[0])
            {
                var colonSplit = item.Split(':');
                int num = int.Parse(colonSplit[0]);
                rules.Add(new Rule(num));
            }
            foreach (var item in inputList[0])
            {
                var colonSplit = item.Split(':');
                int num = int.Parse(colonSplit[0].Trim());
                Rule r = rules.Find(n => n.Num == num);
                var subrules = colonSplit[1].Trim().Split(' ');
                var targetList = r.SubrulesA;
                foreach (var subrule in subrules)
                {
                    if (subrule == "|")
                    {
                        targetList = r.SubrulesB;
                    }
                    else if (subrule == "\"a\"")
                    {
                        r.Letter = "a";
                    }
                    else if (subrule == "\"b\"")
                    {
                        r.Letter = "b";
                    }
                    else
                    {
                        int n = int.Parse(subrule);
                        Rule addRule = rules.Find(x => x.Num == n);
                        targetList.Add(addRule);
                    }
                }
            }
            //foreach (var item in rules)
            //{
            //    Console.WriteLine(item);
            //}

            int count = 0;
            foreach (var item in inputList[1])
            {
                Console.WriteLine();
                bool res = MatchToRuleZero(rules, item);
                Console.WriteLine($"{item,41}, {res}");
                if (res)
                {
                    count++;
                }
                if (inputList.Count == 3)
                {
                    if (inputList[2].Contains(item))
                    {
                        Console.WriteLine($"should be true");
                    }
                }
            }
            Console.WriteLine(count);
        }

        public static bool MatchToRuleZero(List<Rule> rules, string s)
        {
            Rule ruleZero = rules.Find(x => x.Num == 0);
            bool truth = ruleZero.Match(s, out string output);
            if (truth && output.Length == 0)
            {
                return true;
            }
            else if (truth && output.Length != 0)
            {
                Console.WriteLine($"{output} expected empty string but was {truth}");
            }
            return false;
        }
    }

    public class Rule
    {
        public int Num { get; private set; }
        public string Letter { get; set; }
        public List<Rule> SubrulesA { get; set; }
        public List<Rule> SubrulesB { get; set; }

        public Rule(int num)
        {
            Num = num;
            SubrulesA = new List<Rule>();
            SubrulesB = new List<Rule>();
        }

        public bool Match(string sIn, out string sOut)
        {
            string sA = (string)sIn.Clone();
            string sB = (string)sIn.Clone();
            sOut = sIn;
            bool match = false;
            if (sIn.Length == 0)
            {
                return match;
            }

            if (Letter != null)
            {
                if (sA.Length > 0)
                {
                    match = Letter == sA[0].ToString();
                    if (match)
                    {
                        sOut = sA[1..];
                    }
                }
            }
            if (SubrulesA.Count > 0 && !match)
            {
                foreach (var subrule in SubrulesA)
                {
                    match = subrule.Match(sA, out string sAOut);
                    if (!match)
                    {
                        break;
                    }
                    sA = sAOut;
                }
                if (match)
                {
                    sOut = sA;
                }
            }
            if (SubrulesB.Count > 0 && !match)
            {
                foreach (var subrule in SubrulesB)
                {
                    match = subrule.Match(sB, out string sBOut);
                    if (!match)
                    {
                        break;
                    }
                    sB = sBOut;
                }
                if (match)
                {
                    sOut = sB;
                }
            }

            return match;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder($"{Num,3}");
            sb.Append($" {Letter,1} ");
            if (SubrulesA != null)
            {
                foreach (var item in SubrulesA)
                {
                    sb.Append($"{item.Num},");
                }
                sb.Remove(sb.Length - 1, 1);
            }
            if (SubrulesB != null)
            {
                sb.Append("|");
                foreach (var item in SubrulesB)
                {
                    sb.Append($"{item.Num},");
                }
                sb.Remove(sb.Length - 1, 1);
            }
            return sb.ToString();
        }
    }

    public class TextRule
    {
        public int Num { get; set; }
        public string ReplacesA { get; set; } = string.Empty;
        public string ReplacesB { get; set; } = string.Empty;

        public TextRule(int num)
        {
            Num = num;
        }

        public override string ToString()
        {
            return $"{Num} replaces {ReplacesA} {(ReplacesB != string.Empty? $"| {ReplacesB}" : "")}";
        }
    }
}

