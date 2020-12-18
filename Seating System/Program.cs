using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using System.Threading;

namespace Seating_System
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputList = new List<string>();
            StreamReader sr = new StreamReader(@"C:\Users\eiedu\Source\Repos\AdventOfCode\Seating System\TextFile1.txt");
            //StreamReader sr = new StreamReader(@"C:\Users\eiedu\Source\Repos\AdventOfCode\Seating System\TextFile2.txt");
            string line = string.Empty;
            while ((line = sr.ReadLine()) != null)
            {
                inputList.Add(line);
            }

            int lenY = inputList.Count + 2;
            int lenX = inputList[1].Length + 2;
            int[,] floorplan = new int[lenY, lenX];

            //put the seats on the floorplan
            Parallel.For(0, lenY - 2, y =>
              {
                  for (int x = 0; x < lenX - 2; x++)
                  {
                      if (inputList[y][x] == 'L')
                      {
                          floorplan[y + 1, x + 1] = 1;
                      }
                  }
              });

            int[,] people = new int[lenY, lenX];

            int changes = 1;
            var startTime = DateTime.Now;
            while (changes > 0)
            {
                changes = FillLoS(ref floorplan, ref people, lenY, lenX);
                //Console.WriteLine(changes);
                //printSeats(ref floorplan, ref people, lenY, lenX);
                //Thread.Sleep(200);
            };
            var endTime = DateTime.Now;
            Console.WriteLine((endTime - startTime).TotalMilliseconds);
            //printSeats(ref floorplan, ref people, lenY, lenX);
            Console.WriteLine(CountPeople(ref people, lenY, lenX));
        }

        static int FillLoS(ref int[,] seats, ref int[,] people, int lenY, int lenX)
        {
            int changes = 0;
            int[,] occupancy = new int[lenY, lenX];
            occupancy = (int[,])people.Clone();

            for (int y = 1; y < lenY - 1; y++)
            {
                for (int x = 1; x < lenX - 1; x++)
                {
                    int adj = 0;
                    int relY;
                    int relX;

                    //consider all eight directions
                    //N = y--
                    relY = y - 1;
                    while (relY > 0)
                    {
                        if (seats[relY, x] == 1 && people[relY,x] == 0)
                        {
                            break;
                        }
                        else if (seats[relY, x] == 1 && people[relY, x] == 1)
                        {
                            adj++;
                            break;
                        }
                        relY--;
                    }

                    //S = y++
                    relY = y + 1;
                    while(relY < lenY)
                    {
                        if (seats[relY, x] == 1 && people[relY, x] == 0)
                        {
                            break;
                        }
                        else if (seats[relY, x] == 1 && people[relY, x] == 1)
                        {
                            adj++;
                            break;
                        }
                        relY++;
                    }

                    //W = x--
                    relX = x - 1;
                    while (relX > 0)
                    {
                        if (seats[y, relX] == 1 && people[y, relX] == 0)
                        {
                            break;
                        }
                        else if (seats[y, relX] == 1 && people[y, relX] == 1)
                        {
                            adj++;
                            break;
                        }
                        relX--;
                    }

                    //E = x++
                    relX = x + 1;
                    while (relX < lenX)
                    {
                        if (seats[y, relX] == 1 && people[y, relX] == 0)
                        {
                            break;
                        }
                        else if (seats[y, relX] == 1 && people[y, relX] == 1)
                        {
                            adj++;
                            break;
                        }
                        relX++;
                    }

                    //NE = y--, x++
                    relY = y - 1;
                    relX = x + 1;
                    while (relY > 0 && relX < lenX)
                    {
                        if (seats[relY, relX] == 1 && people[relY, relX] == 0)
                        {
                            break;
                        }
                        else if (seats[relY, relX] == 1 && people[relY, relX] == 1)
                        {
                            adj++;
                            break;
                        }
                        relY--;
                        relX++;
                    }

                    //SE = y++, x++
                    relY = y + 1;
                    relX = x + 1;
                    while (relY < lenY && relX < lenX)
                    {
                        if (seats[relY, relX] == 1 && people[relY, relX] == 0)
                        {
                            break;
                        }
                        else if (seats[relY, relX] == 1 && people[relY, relX] == 1)
                        {
                            adj++;
                            break;
                        }
                        relY++;
                        relX++;
                    }

                    //SW = y++, x--
                    relY = y + 1;
                    relX = x - 1;
                    while (relY < lenY && relX > 0)
                    {
                        if (seats[relY, relX] == 1 && people[relY, relX] == 0)
                        {
                            break;
                        }
                        else if (seats[relY, relX] == 1 && people[relY, relX] == 1)
                        {
                            adj++;
                            break;
                        }
                        relY++;
                        relX--;
                    }

                    //NW = y--, x--
                    relY = y-1;
                    relX = x-1;
                    while (relY > 0 && relX > 0)
                    {
                        if (seats[relY, relX] == 1 && people[relY, relX] == 0)
                        {
                            break;
                        }
                        else if (seats[relY, relX] == 1 && people[relY, relX] == 1)
                        {
                            adj++;
                            break;
                        }
                        relY--;
                        relX--;
                    }

                    //if all 8 directions are clear, the seat becomes occupied
                    if (adj == 0 && seats[y, x] == 1 && people[y, x] == 0)
                    {
                        changes++;
                        occupancy[y, x] = 1;
                    }
                    //if 5 or more directions have occupied seats, the seat becomes empty
                    else if (people[y, x] == 1 && adj > 4)
                    {
                        changes++;
                        occupancy[y, x] = 0;
                    }
                }
            }
            people = occupancy;
            return changes;
        }

        static int Fill(ref int[,] seats, ref int[,] people, int lenY, int lenX)
        {
            int changes = 0;
            int[,] occupancy = new int[lenY, lenX];
            occupancy = (int[,])people.Clone();
            for (int y = 1; y < lenY - 1; y++)
            {
                for (int x = 1; x < lenX - 1; x++)
                {
                    /* If a seat is empty (L) and there are no occupied seats adjacent to it, the seat becomes occupied.
                     * If a seat is occupied (#) and four or more seats adjacent to it are also occupied, the seat becomes empty.
                     * Otherwise, the seat's state does not change.
                     */

                    int adj = 0;
                    for (int relY = -1; relY <= 1; relY++)
                    {
                        for (int relX = -1; relX <= 1; relX++)
                        {
                            if (relY == 0 & relX == 0)
                            {
                                continue;
                            }
                            adj += people[y + relY, x + relX] * seats[y + relY, x + relX];
                        }
                    }
                    if (adj == 0 && seats[y, x] == 1 && people[y, x] == 0)
                    {
                        changes++;
                        occupancy[y, x] = 1;
                    }
                    else if (people[y, x] == 1 && adj > 3)
                    {
                        changes++;
                        occupancy[y, x] = 0;
                    }
                }
            }
            people = occupancy;
            return changes;
        }

        static int CountPeople(ref int[,] people, int lenY, int lenX)
        {
            int count = 0;
            for (int y = 0; y < lenY; y++)
            {
                for (int x = 0; x < lenX; x++)
                {
                    count += people[y, x];
                }
            }
            return count;
        }

        static void printSeats(ref int[,] seats, ref int[,] people, int lenY, int lenX)
        {
            for (int y = 0; y < lenY; y++)
            {
                StringBuilder sb = new StringBuilder();
                for (int x = 0; x < lenX; x++)
                {
                    if (people[y,x] == 1)
                    {
                        sb.Append($"X ");
                    }
                    else if (people[y,x] == 0 && seats[y,x] == 1)
                    {
                        sb.Append($". ");
                    }
                    else
                    {
                        sb.Append("  ");
                    }
                }
                Console.WriteLine(sb.ToString());
            }
        }
    }
}
