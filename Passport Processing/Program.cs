using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Passport_Processing
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputList = new List<string>();
            StreamReader sr = new StreamReader(@"C:\Users\eiedu\Source\Repos\AdventOfCode\Passport Processing\TextFile1.txt");
            string line = string.Empty;
            while((line = sr.ReadLine()) != null)
            {
                inputList.Add(line);
            }

            var listofPassports = new List<string>();
            string passportString = string.Empty;
            foreach (var item in inputList)
            {
                if (item != string.Empty)
                {
                    passportString += " " + item.Trim();
                }
                else
                {
                    listofPassports.Add(passportString);
                    passportString = string.Empty;
                }
            }
            listofPassports.Add(passportString);

            var Batch = new List<Dictionary<string, string>>();
            foreach (var item in listofPassports)
            {
                var Passport = new Dictionary<string, string>();
                var splitLine = item.Trim().Split(' ');
                foreach (var pp in splitLine)
                {
                    var kvp = pp.Split(':');
                    Passport.Add(kvp[0], kvp[1]);
                }
                Batch.Add(Passport);
            }

            var manditoryKeys = new List<string> { "byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid"};
            int count = 0;
            foreach (var item in Batch)
            {
                int flag = item.IsValid();
                if (flag == 0)
                {
                    count++;
                }
                else
                {
                    Console.WriteLine($"{flag,5} | {item.Poop()}");
                }
            }
            Console.WriteLine(count);
        }
    }

    public static class dictExtension
    {
        public static string Poop(this Dictionary<string,string> a)
        {
            StringBuilder sb = new StringBuilder("");
            foreach (var item in a)
            {
                sb.Append($"{{{item.Key}:{item.Value}}} ");
            }
            return sb.ToString();
        }

        public static int IsValid(this Dictionary<string,string> a)
        {
            var manditoryKeys = new List<string> { "byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid" };
            int flag = 0;
            foreach (var key in manditoryKeys)
            {
                if (!a.ContainsKey(key))
                {
                    flag |= 128;
                    return flag;
                }
            }

            //byr flag 1
            int byr = int.Parse(a["byr"]);
            if (byr < 1920 || byr > 2002)
            {
                flag |= 1;
            }

            //iyr flag 2
            int iyr = int.Parse(a["iyr"]);
            if (iyr > 2020 || iyr < 2010)
            {
                flag |= 2;
            }

            //eyr flag 4
            int eyr = int.Parse(a["eyr"]);
            if (eyr > 2030 || eyr < 2020)
            {
                flag |= 4;
            }

            //hgt flag 8
            string hgt = a["hgt"];
            if (hgt.Contains("in"))
            {
                int ht = int.Parse(hgt.Trim('n').Trim('i'));
                if (ht < 59 || ht > 76)
                {
                    flag |= 8;
                }
            }
            else if (hgt.Contains("cm"))
            {
                int ht = int.Parse(hgt.Trim('m').Trim('c'));
                if (ht < 150 || ht > 193)
                {
                    flag |= 8;
                }
            }
            else
            {
                flag |= 8;
            }

            //hcl flag 16
            Regex hexColor = new Regex("^#([a-fA-F0-9]{6})$");
            if (!hexColor.IsMatch(a["hcl"]))
            {
                flag |= 16;
            }

            //ecl flag 32
            var eyeColors = new SortedSet<string> { "amb", "blu", "brn", "gry", "grn", "hzl", "oth" };
            if (!eyeColors.Contains(a["ecl"]))
            {
                flag |= 32;
            }


            //pid flag 64
            Regex passportID = new Regex("^([0-9]{9})$");
            if (!passportID.IsMatch(a["pid"]))
            {
                flag |= 64;
            }

            return flag;
        }
    }
}
