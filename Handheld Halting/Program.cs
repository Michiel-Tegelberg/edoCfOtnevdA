using System;
using System.Collections.Generic;
using System.IO;

namespace Handheld_Halting
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputList = new List<string>();
            StreamReader sr = new StreamReader(@"C:\Users\eiedu\Source\Repos\AdventOfCode\Handheld Halting\TextFile1.txt");
            string line = string.Empty;
            while((line = sr.ReadLine()) != null)
            {
                inputList.Add(line);
            }

            int targetPos = inputList.Count;
            var targets = new List<int>();
            Console.WriteLine(targetPos);
            
            var stateSet = new List<int>();
            bool repeat = false;
            int pos = 0;
            int acc = 0;

            while (!repeat)
            {
                line = inputList[pos];
                var splitLine = line.Split(' ');
                string command = splitLine[0];
                int value = int.Parse(splitLine[1].TrimStart('+'));

                switch (command)
                {
                    case "acc":
                        acc += value;
                        pos++;
                        break;
                    case "jmp":
                        stateSet.Add(pos);
                        pos += value;
                        break;
                    case "nop":
                        stateSet.Add(pos);
                        pos++;
                        break;
                    default:
                        break;
                }

                if (stateSet.Contains(pos))
                {
                    targets.AddRange(stateSet);
                    repeat = true;
                }
            }

            foreach (var item in targets)
            {
                //go over the inputlist
                //do nop where jmp or jmp where nop when pos == item
                //if pos == targetPos then cw item & acc\

                stateSet = new List<int>();
                repeat = false;
                pos = 0;
                acc = 0;

                while (!repeat)
                {
                    line = inputList[pos];
                    var splitLine = line.Split(' ');
                    string command = splitLine[0];
                    int value = int.Parse(splitLine[1].TrimStart('+'));

                    if (pos == item)
                    {
                        if (command == "jmp")
                        {
                            command = "nop";
                        }
                        else if (command == "nop")
                        {
                            command = "jmp";
                        }
                    }

                    switch (command)
                    {
                        case "acc":
                            acc += value;
                            pos++;
                            break;
                        case "jmp":
                            stateSet.Add(pos);
                            pos += value;
                            break;
                        case "nop":
                            stateSet.Add(pos);
                            pos++;
                            break;
                        default:
                            break;
                    }

                    if (pos == targetPos)
                    {
                        Console.WriteLine($"{targetPos} reached with {acc} by changing {item}");
                        repeat = true;
                    }
                    if (stateSet.Contains(pos))
                    {
                        repeat = true;
                    }
                }
            }
        }
    }

    public class State
    {
        public int Acc { get; private set; }
        public int Pos { get; private set; }

        public State(int pos, int acc)
        {
            Pos = pos;
            Acc = acc;
        }

        public override int GetHashCode()
        {
            return Acc * 1000 + Pos;
        }
    }
}
