using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Allergen_Assessment
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputList = new List<string>();
            StreamReader sr = new StreamReader(@"C:\Users\eiedu\Source\Repos\AdventOfCode\Allergen Assessment\TextFile1.txt");
            string line = string.Empty;
            while ((line = sr.ReadLine()) != null)
            {
                inputList.Add(line);
            }

            var allForeignIngredients = new List<string>();
            var AllergenDict = new Dictionary<string, List<string>>();
            foreach (var item in inputList)
            {
                var ingredients = item.Split(" (contains ")[0].Trim().Split(' ').ToList();
                foreach (var ingredient in ingredients)
                {
                    allForeignIngredients.Add(ingredient);
                }
                var allergens = item.Split(" (contains ")[1].Trim(')').Split(", ");
                foreach (var allergen in allergens)
                {
                    //if the allergen is already known
                    var removeFromDict = new List<string>();
                    if (AllergenDict.ContainsKey(allergen))
                    {
                        foreach (var ingredient in AllergenDict[allergen])
                        {
                            if (!ingredients.Contains(ingredient))
                            {
                                removeFromDict.Add(ingredient);
                            }
                        }
                        foreach (var ingredient in removeFromDict)
                        {
                            AllergenDict[allergen].Remove(ingredient);
                        }
                    }
                    else
                    {
                        //if the allergen is new
                        AllergenDict.Add(allergen, new List<string>(ingredients)); 
                    }
                }
            }

            var confirmedForeignAllergens = new SortedSet<string>();
            foreach (var allergen in AllergenDict)
            {
                foreach (var ingredient in allergen.Value)
                {
                    confirmedForeignAllergens.Add(ingredient);
                }
            }
            Console.WriteLine(allForeignIngredients.Count);
            allForeignIngredients.RemoveAll(x => confirmedForeignAllergens.Contains(x));
            Console.WriteLine(allForeignIngredients.Count);

            //foreach (var item in AllergenDict)
            //{
            //    StringBuilder sb = new StringBuilder($"{item.Key}:");
            //    foreach (var ingredient in item.Value)
            //    {
            //        sb.Append($" {ingredient},");
            //    }
            //    sb.Remove(sb.Length - 1, 1);
            //    Console.WriteLine(sb.ToString());
            //}
        }
    }
}
