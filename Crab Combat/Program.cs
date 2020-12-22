using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Crab_Combat
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputList = new List<string>();
            //StreamReader sr = new StreamReader(@"C:\Users\eiedu\Source\Repos\AdventOfCode\Crab Combat\TextFile1.txt");
            StreamReader sr = new StreamReader(@"C:\Users\eiedu\Source\Repos\AdventOfCode\Crab Combat\TextFile2.txt");
            //StreamReader sr = new StreamReader(@"C:\Users\eiedu\Source\Repos\AdventOfCode\Crab Combat\TextFile3.txt");
            string line = string.Empty;
            while ((line = sr.ReadLine()) != null)
            {
                inputList.Add(line);
            }

            var p1 = new Queue<int>();
            var p2 = new Queue<int>();
            var target = p1;
            foreach (var item in inputList)
            {
                if (item == string.Empty)
                {
                    target = p2;
                }
                else
                {
                    bool parsed = int.TryParse(item, out int card);
                    if (parsed)
                    {
                        target.Enqueue(card);
                    }
                }
            }
            Console.WriteLine(p1.Count);
            Console.WriteLine(p2.Count);

            //PlayNormalGame(p1, p2);
            var start = DateTime.Now;
            PlayRecursiveGame(p1, p2, 1);
            var end = DateTime.Now;
            Console.WriteLine($"took: {(end-start).TotalMilliseconds}");


            var Winner = p1.Count > p2.Count ? p1 : p2;
            int multiplier = Winner.Count;
            int sum = 0;
            while (Winner.Count > 0)
            {
                int card = Winner.Dequeue();
                sum += card * multiplier;
                multiplier--;
            }
            Console.WriteLine(sum);
        }

        /// <summary>
        /// Play a recursive game of Crab Combat
        /// </summary>
        /// <param name="p1">Player 1's deck</param>
        /// <param name="p2">Player 2's deck</param>
        /// <returns>True if player 1 wins, false if player 2 wins</returns>
        private static bool PlayRecursiveGame(Queue<int> p1, Queue<int> p2, int game)
        {
            int thisGame = game;
            //Console.WriteLine($"\n=== Game {thisGame} ===\n");
            var history = new SortedSet<string>();
            int round = 0;
            while (p1.Count > 0 && p2.Count > 0)
            {
                round++;
                //Console.WriteLine($"\n-- Round {round} (Game {thisGame}) --");
                //Console.WriteLine($"Player 1's deck: {p1.S()}");
                //Console.WriteLine($"Player 2's deck: {p2.S()}");
                if (!AddToHistory(history, p1, p2))
                {
                    //Console.WriteLine($"P1 wins game {thisGame} due to infinite loops");
                    return true;
                }

                int card1 = p1.Dequeue();
                int card2 = p2.Dequeue();
                //Console.WriteLine($"Player 1 plays: {card1}");
                //Console.WriteLine($"Player 2 plays: {card2}");

                bool winner;
                if (p1.Count >= card1 && p2.Count >= card2)
                {
                    //Console.WriteLine($"Playing a sub-game to determine the winner...");
                    winner = PlayRecursiveGame(MakeSubDeck(p1,card1), MakeSubDeck(p2,card2), ++game);
                    //Console.WriteLine($"\n=== Game {thisGame} ===\n");
                }
                else
                {
                    winner = card1 > card2;
                }

                if (winner)
                {
                    //Console.WriteLine($"Player 1 wins round {round} of game {thisGame}!");
                    p1.Enqueue(card1);
                    p1.Enqueue(card2);
                }
                else
                {
                    //Console.WriteLine($"Player 2 wins round {round} of game {thisGame}!");
                    p2.Enqueue(card2);
                    p2.Enqueue(card1);
                }
            }

            if (p1.Count == 0)
            {
                //Console.WriteLine($"The winner of game {thisGame} is player 2!");
                return false;
            }
            else
            {
                //Console.WriteLine($"The winner of game {thisGame} is player 1!");
                return true;
            }
        }

        private static Queue<int> MakeSubDeck(Queue<int> q, int card)
        {
            return new Queue<int>(q.ToArray()[0..card]);
        }

        private static bool AddToHistory(SortedSet<string> history, Queue<int> p1, Queue<int> p2)
        {
            var p1Copy = new Queue<int>(p1);
            var p2Copy = new Queue<int>(p2);
            string s = p1Copy.S() + "|" + p2Copy.S();
            return history.Add(s);
        }

        private static void PlayNormalGame(Queue<int> p1, Queue<int> p2)
        {
            while (p1.Count > 0 && p2.Count > 0)
            {
                //battle
                int card1 = p1.Dequeue();
                int card2 = p2.Dequeue();
                if (card1 > card2)
                {
                    p1.Enqueue(card1);
                    p1.Enqueue(card2);
                }
                else if (card2 > card1)
                {
                    p2.Enqueue(card2);
                    p2.Enqueue(card1);
                }
            }

            //we have a winner
            var Winner = p1.Count > p2.Count ? p1 : p2;
            int multiplier = Winner.Count;
            int sum = 0;
            while (Winner.Count > 0)
            {
                int card = Winner.Dequeue();
                sum += card * multiplier;
                multiplier--;
            }
            Console.WriteLine(sum);
        }
    }

    public static class Extensions
    {
        public static string S(this Queue<int> self) 
        {
            StringBuilder sb = new StringBuilder();
            var selfCopy = new Queue<int>(self);
            int l = self.Count;
            for (int i = 0; i < l; i++)
            {
                sb.Append($"{selfCopy.Dequeue()},");
            }
            if (sb.Length>0)
            {
                sb.Remove(sb.Length - 1, 1); 
            }
            return sb.ToString();
        }
    }
}
