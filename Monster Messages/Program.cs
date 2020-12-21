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
            StreamReader sr = new StreamReader(@"C:\Users\eiedu\Source\Repos\AdventOfCode\Monster Messages\TextFile6.txt");
            //StreamReader sr = new StreamReader(@"C:\Users\eiedu\Source\Repos\AdventOfCode\Monster Messages\TextFile5.txt");
            //StreamReader sr = new StreamReader(@"C:\Users\eiedu\Source\Repos\AdventOfCode\Monster Messages\TextFile4.txt");
            //StreamReader sr = new StreamReader(@"C:\Users\eiedu\Source\Repos\AdventOfCode\Monster Messages\TextFile3.txt");
            //StreamReader sr = new StreamReader(@"C:\Users\eiedu\Source\Repos\AdventOfCode\Monster Messages\TextFile2.txt");
            //StreamReader sr = new StreamReader(@"C:\Users\eiedu\Source\Repos\AdventOfCode\Monster Messages\TextFile1.txt");
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
            


            SolveWithRules(inputList);
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
            int depth = 30 - sIn.Length;
            string dent = "";
            for (int i = 0; i < depth; i++)
            {
                dent += " ";
            }
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
                //Console.WriteLine($" in: {Num,2}.{Letter} => {sIn,30}");
                if (sA.Length > 0)
                {
                    match = Letter == sA[0].ToString();
                    if (match)
                    {
                        sOut = sA[1..];
                    }
                }
                //Console.WriteLine($"Matched {sA[0]} with {Letter} which is {match}, returned {sOut}");
                //Console.WriteLine($" ret 0 {match}");
            }
            if (SubrulesA.Count > 0 && !match)
            {
                //Console.WriteLine($" in: {Num,2}.A => {sIn,30}");
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
                    //Console.WriteLine($" ret 1 {match}");
                }
            }
            if (SubrulesB.Count > 0 && !match)
            {
                //Console.WriteLine($" in: {Num,2}.B => {sIn,30}");
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
                    //Console.WriteLine($" ret 2 {match}");
                }
            }

            //if (Letter != null)
            //{
            //    Console.WriteLine($"out: {Letter,2} => {sOut,30} => {match}");
            //}
            //else
            //{
            //    Console.WriteLine($"out: {Num,2} => {sOut,30} => {match}");
            //}
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
}
