using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Crab_Cups
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputList = new List<string>();
            StreamReader sr = new StreamReader(@"C:\Users\eiedu\Source\Repos\AdventOfCode\Crab Cups\TextFile1.txt");
            //StreamReader sr = new StreamReader(@"C:\Users\eiedu\Source\Repos\AdventOfCode\Crab Cups\TextFile2.txt");
            string line = string.Empty;
            while ((line = sr.ReadLine()) != null)
            {
                inputList.Add(line);
            }

            var charArray = inputList[0].ToCharArray();
            var cups = new Queue<int>();
            int max = 0;
            foreach (var c in charArray)
            {
                int newInt = int.Parse(c.ToString());
                if (newInt > max)
                {
                    max = newInt;
                }
                cups.Enqueue(newInt);
            }

            var pickUp = new Queue<int>();

            //PartOne(cups, max, pickUp);

            //use circularly linked list for P2, plus a value-indexed array with all items
            var startTime = DateTime.Now;
            linkedListItem<int>[] LUT = new linkedListItem<int>[1000001];

            max = 1000000;
            linkedListItem<int> first = new linkedListItem<int>(int.Parse(charArray[0].ToString()));
            linkedListItem<int> last = first;
            for (int i = 1; i < charArray.Length; i++)
            {
                int newInt = int.Parse(charArray[i].ToString());
                var currentLLI = new linkedListItem<int>(newInt);
                last.Next = currentLLI;
                last = currentLLI;
            }

            for (int i = 10; i <= 1000000; i++)
            {
                var currentLLI = new linkedListItem<int>(i);
                last.Next = currentLLI;
                last = currentLLI;
            }

            last.Next = first;
            var start = first;
            do
            {
                LUT[start.Value] = start;
                start = start.Next;
            }
            while (start != first);

            //currentcup
            var currentCup = first;

            for (int i = 0; i < 10000000; i++)
            {
                //after snip snip 3 elements right of currentcup
                var pickupFirst = currentCup.Next;
                var pickupLast = pickupFirst.Next.Next;
                currentCup.Next = pickupLast.Next;

                //destinationCup = currentcup-- until not in pickup linkedlist
                int destinationCupValue = currentCup.Value;
                do
                {
                    destinationCupValue--;
                    if (destinationCupValue == 0)
                    {
                        destinationCupValue = max;
                    }
                }
                while (destinationCupValue == pickupFirst.Value ||      //item 1
                        destinationCupValue == pickupFirst.Next.Value ||//item 2
                        destinationCupValue == pickupLast.Value);       //item 3

                currentCup = currentCup.Next;

                //find destinationcup; 
                var destinationCup = LUT[destinationCupValue];

                //when found, stitch in the picked up cups
                pickupLast.Next = destinationCup.Next; 
                destinationCup.Next = pickupFirst;
            }

            var endTime = DateTime.Now;

            //find 1 and then output answer
            while (currentCup.Value != 1)
            {
                currentCup = currentCup.Next;
            }
            Console.WriteLine((long)currentCup.Next.Value * (long)currentCup.Next.Next.Value);
            Console.WriteLine($"Took: {(endTime-startTime).TotalMilliseconds}ms");
        }

        private static void PartOne(Queue<int> cups, int max, Queue<int> pickUp)
        {

            //loop time
            for (int move = 1; move <= 100; move++)
            {
                /* it will designate the first cup in your list as the current cup.
                 */
                int currentCup = cups.Dequeue();
                cups.Enqueue(currentCup);
                /* The crab picks up the three cups that are immediately clockwise of the current cup. 
                 * They are removed from the circle; cup spacing is adjusted as necessary to maintain 
                 * the circle.
                 */
                for (int pick = 0; pick < 3; pick++)
                {
                    pickUp.Enqueue(cups.Dequeue());
                }

                /* The crab selects a destination cup: the cup with a label equal to the current cup's 
                 * label minus one. If this would select one of the cups that was just picked up, the 
                 * crab will keep subtracting one until it finds a cup that wasn't just picked up. If 
                 * at any point in this process the value goes below the lowest value on any cup's 
                 * label, it wraps around to the highest value on any cup's label instead.
                 */
                int destinationCup = currentCup;
                do
                {
                    destinationCup--;
                    if (destinationCup < 1)
                    {
                        destinationCup = max;
                    }
                }
                while (pickUp.ToArray().ToList().Contains(destinationCup));


                /* The crab places the cups it just picked up so that they are immediately clockwise 
                 * of the destination cup. They keep the same order as when they were picked up
                 */
                int cup;
                do
                {
                    cup = cups.Dequeue();
                    cups.Enqueue(cup);
                }
                while (cup != destinationCup);
                for (int pick = 0; pick < 3; pick++)
                {
                    cups.Enqueue(pickUp.Dequeue());
                }

                /* The crab selects a new current cup: the cup which is immediately clockwise of the 
                 * current cup.
                 */
                do
                {
                    cup = cups.Dequeue();
                    cups.Enqueue(cup);
                }
                while (cup != currentCup);
            }

            var cupCopy = cups.ToArray().ToList();
            var lastItem = cupCopy[cupCopy.Count - 1];
            cupCopy.Remove(lastItem);
            cupCopy.Insert(0, lastItem);
            foreach (var item in cupCopy)
            {
                Console.WriteLine(item);
            }
        }
    }

    public class linkedListItem<T>
    {
        public T Value { get; set; }
        public linkedListItem<T> Next { get; set; }

        public linkedListItem(T value)
        {
            Value = value;
        }
    }
}
