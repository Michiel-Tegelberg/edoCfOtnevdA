using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Ticket_Translation
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputList = new List<List<string>>();
            StreamReader sr = new StreamReader(@"C:\Users\eiedu\Source\Repos\AdventOfCode\Ticket Translation\TextFile1.txt");
            string line = string.Empty;
            var intermediateList = new List<string>();
            while ((line = sr.ReadLine()) != null)
            {
                if (line == string.Empty)
                {
                    inputList.Add(intermediateList);
                    intermediateList = new List<string>();
                }
                else
                {
                    intermediateList.Add(line);
                }
            }
            inputList.Add(intermediateList);

            //Define types
            var TicketTypes = new List<TicketType>();
            int num = 0;
            foreach (var item in inputList[0])
            {
                var colonSplit = item.Trim().Split(':');
                string name = colonSplit[0].Trim();
                var orSplit = colonSplit[1].Trim().Split(" or ");
                int ll, hl, lh, hh;
                var lowSplit = orSplit[0].Split('-');
                var highSplit = orSplit[1].Split('-');
                ll = int.Parse(lowSplit[0].Trim());
                hl = int.Parse(lowSplit[1].Trim());
                lh = int.Parse(highSplit[0].Trim());
                hh = int.Parse(highSplit[1].Trim());
                var tt = new TicketType(name, ll, hl, lh, hh, num);
                num++;
                //Console.WriteLine(tt.ToString());
                TicketTypes.Add(tt);
            }

            inputList[2].RemoveAt(0); //remove header
            int y = inputList[2].Count;
            int x = inputList[2][0].Split(',').Length;
            //Console.WriteLine($"{y} by {x}");
            var ticketTuples = new int[y, x];
            var removeList = new List<string>();
            int sum = 0;
            for (int i = 0; i < y; i++)
            {
                var splitLine = inputList[2][i].Split(',');
                for (int j = 0; j < x; j++)
                {
                    int n = int.Parse(splitLine[j]);
                    int isGood = 0;
                    foreach (TicketType type in TicketTypes)
                    {
                        if (type.FitsType(n))
                        {
                            isGood++;
                        }
                    }
                    if (isGood < 1)
                    {
                        removeList.Add(inputList[2][i]);
                        sum += n;
                    }
                }
            }
            Console.WriteLine(sum);

            foreach (var item in removeList)
            {
                inputList[2].Remove(item);
            }


            y = inputList[2].Count;
            for (int i = 0; i < y; i++)
            {
                var splitLine = inputList[2][i].Split(',');
                for (int j = 0; j < x; j++)
                {
                    ticketTuples[i, j] = int.Parse(splitLine[j]);
                }
            }

            //for each column find the possible types
            var typeArray = new List<TicketType>[x];
            for (int i = 0; i < x; i++)
            {
                typeArray[i] = new List<TicketType>(TicketTypes);
            }

            //per column
            for (int i = 0; i < x; i++)
            {
                //for each row at x
                for (int j = 0; j < y; j++)
                {
                    //if the number doesn't fit in a certain ticketType, strike the ticketType off the list
                    var toRemove = new List<TicketType>();
                    for (int k = 0; k < typeArray[i].Count; k++)
                    {
                        var ticketType = typeArray[i][k];
                        if (!ticketType.FitsType(ticketTuples[j, i]))
                        {
                            toRemove.Add(ticketType);
                        }
                    }
                    foreach (var item in toRemove)
                    {
                        typeArray[i].Remove(item);
                    }
                }

                //after striking the impossibilities output all possible types per column
                Console.WriteLine($"\nColumn: {i}");
                foreach (var item in typeArray[i])
                {
                    Console.WriteLine(item);
                }
            }

            //mr_bean_majick.wav https://i.imgur.com/dVQoPHR.png

            var arr = inputList[1][1].Split(',');
            long[] ia = new long[x];
            for (int i = 0; i < x; i++)
            {
                ia[i] = int.Parse(arr[i]);
            }
            Console.WriteLine(ia[1]*ia[2]*ia[5]*ia[13]*ia[15]*ia[19]);

        }
    }



    public class TicketType
    {
        public int Num { get; set; }
        public string Name { get; private set; }
        public int LL { get; private set; }
        public int HL { get; private set; }
        public int LH { get; private set; }
        public int HH { get; private set; }

        public TicketType(string name, int ll, int hl, int lh, int hh, int num)
        {
            Name = name;
            LL = ll;
            HL = hl;
            LH = lh;
            HH = hh;
            Num = num;
        }

        public bool FitsType(int n)
        {
            return (LL <= n && HL >= n) || (LH <= n && HH >= n);
        }

        public override string ToString()
        {
            return $"{Num,2} | {Name,20} | {LL,3}-{HL,3} or {LH,3}-{HH,3}";
        }
    }
}
