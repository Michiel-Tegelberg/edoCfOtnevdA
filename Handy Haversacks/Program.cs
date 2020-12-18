using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Handy_Haversacks
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputList = new List<string>();
            StreamReader sr = new StreamReader(@"C:\Users\eiedu\Source\Repos\AdventOfCode\Handy Haversacks\TextFile1.txt");
            string line = string.Empty;
            var BagList = new List<Bag>();
            while((line = sr.ReadLine()) != null)
            {
                inputList.Add(line);
                var splitLine = line.Split("bags contain");
                var newBag = new Bag(splitLine[0].Trim());
                BagList.Add(newBag);
            }

            foreach (var item in inputList)
            {
                var splitLine = item.Split("bags contain");
                var bag = BagList.Find(x => x.Name == splitLine[0].Trim());
                var addBags = splitLine[1].Split(',');
                foreach (var addBag in addBags)
                {
                    var trimmedBag = addBag.Trim();
                    if (trimmedBag == "no other bags.")
                    {
                        continue;
                    }
                    var num = trimmedBag.Substring(0, 1);
                    trimmedBag = trimmedBag.Substring(1);
                    var bagName = trimmedBag.Split("bag")[0].Trim();
                    bag.Bags.Add(bagName, int.Parse(num));
                    if (bagName == "shiny gold")
                    {
                        bag.canHaveGoldBag = true;
                    }
                }
            }

            //do list swappy stuff until no changes to find all bags that can have golds
            var LastList = new List<Bag>(BagList);
            bool changes = true;
            while (changes)
            {
                changes = false;
                foreach (Bag b in BagList)
                {
                    foreach (var bname in b.Bags.Keys)
                    {
                        if (BagList.Find(x => x.Name == bname).canHaveGoldBag && !b.canHaveGoldBag)
                        {
                            b.canHaveGoldBag = true;
                            changes = true;
                            break;
                        }
                    }
                }
            }

            int count = 0;
            foreach (var item in BagList)
            {
                if (item.canHaveGoldBag)
                {
                    count++;
                }
                Console.WriteLine(item.ToString());
            }
            Console.WriteLine(count);

            var goldBag = BagList.Find(x => x.Name == "shiny gold");
            Console.WriteLine($"Bags inside 1 gold bag: {goldBag.CountBags(BagList)}");
        }
    }

    public class Bag
    {
        public string Name { get; set; }

        public Dictionary<string, int> Bags { get; set; }

        public bool canHaveGoldBag { get; set; }

        public Bag(string name)
        {
            Name = name;
            Bags = new Dictionary<string, int>();
            canHaveGoldBag = false;
        }

        public int CountBags(List<Bag> bagList)
        {
            int count = 1;
            foreach (var bag in Bags)
            {
                Bag b = bagList.Find(x => x.Name == bag.Key);
                count += bag.Value * b.CountBags(bagList);
            }
            return count;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder($"{canHaveGoldBag,5}, {Name} holds:");
            foreach (var item in Bags)
            {
                sb.Append($" {item.Value} {item.Key},");
            }
            sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }
    }
}
