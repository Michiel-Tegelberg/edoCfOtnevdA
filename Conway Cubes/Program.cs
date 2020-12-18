using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Conway_Cubes
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputList = new List<string>();
            //StreamReader sr = new StreamReader(@"C:\Users\eiedu\Source\Repos\AdventOfCode\Conway Cubes\TextFile1.txt");
            StreamReader sr = new StreamReader(@"C:\Users\eiedu\Source\Repos\AdventOfCode\Conway Cubes\TextFile2.txt");
            string line = string.Empty;
            while ((line = sr.ReadLine()) != null)
            {
                inputList.Add(line);
            }

            int x = inputList[0].Length;
            int y = inputList.Count;
            //setup initial state
            var Field = new List<Cube>();
            for (int i = 0; i < y; i++)
            {
                for (int j = 0; j < x; j++)
                {
                    if (inputList[i][j] == '#')
                    {
                        Cube c = new Cube(j - 1, i - 1, 0, 0);
                        //Console.WriteLine(c);
                        c.Active = true;
                        Field.Add(c);
                    }
                }
            }

            //SolveSequentiallyAsGraph(Field);

            SolveParallelAsArray(Field);

            //WriteField(Field);
        }

        private static void SolveParallelAsArray(List<Cube> field)
        {
            //init boundaries
            int lowW, highW, lowZ, highZ, lowY, highY, lowX, highX;
            lowW =  lowZ =  lowY =  lowX =  -1;
            highW = highZ = highY = highX = 1;
            foreach (var item in field)
            {
                if (item.W < lowW)
                {
                    lowW = item.W;
                }
                if (item.W > highW)
                {
                    highW = item.W;
                }
                if (item.Z < lowZ)
                {
                    lowZ = item.Z;
                }
                if (item.Z > highZ)
                {
                    highZ = item.Z;
                }
                if (item.Y < lowY)
                {
                    lowY = item.Y;
                }
                if (item.Y > highY)
                {
                    highY = item.Y;
                }
                if (item.X < lowX)
                {
                    lowX = item.X;
                }
                if (item.X > highX)
                {
                    highX = item.X;
                }
            }

            var start = DateTime.Now;
            Mutex[,,,] gridLock = new Mutex[(highX - lowX) + 1, (highY - lowY) + 1, (highZ - lowZ) + 1, (highW - lowW) + 1];
            gridLock.Initialize();
            for (int i = 0; i < 6; i++)
            {
                //make grid
                int[,,,] grid = new int[(highX - lowX)+1, (highY - lowY) + 1, (highZ - lowZ) + 1, (highW - lowW) + 1];
                bool[,,,] active = new bool[(highX - lowX) + 1, (highY - lowY) + 1, (highZ - lowZ) + 1, (highW - lowW) + 1];

                var res = Parallel.ForEach(field, c =>
                {
                    int relX = c.X - lowX;
                    int relY = c.Y - lowY;
                    int relZ = c.Z - lowZ;
                    int relW = c.W - lowW;

                    active[relX, relY, relZ, relW] = true;
                    var neighbours = c.GetNeighbours();
                    foreach (var neighbour in neighbours)
                    {
                        gridLock[relX, relY, relZ, relW].WaitOne();
                        grid[relX, relY, relZ, relW]++;
                        gridLock[relX, relY, relZ, relW].ReleaseMutex();
                    }
                });

                //do logic to make the whole field
                field.Clear();
                for (int w = lowW; w < highW + 1; w++)
                {
                    for (int z = lowZ; z < highZ + 1; z++)
                    {
                        for (int y = lowY; y < highZ + 1; y++)
                        {
                            for (int x = lowX; x < highX + 1; x++)
                            {
                                if (active[x, y, z, w] && (grid[x, y, z, w] == 2 || grid[x, y, z, w] == 3))
                                {
                                    //add to list for next round
                                    var c = new Cube(x, y, z, w);
                                    c.Active = true;
                                    field.Add(c);
                                }
                                else if (!active[x, y, z, w] && grid[x, y, z, w] == 3)
                                {
                                    //and add to list for next round
                                    var c = new Cube(x, y, z, w);
                                    c.Active = true;
                                    field.Add(c);
                                }
                            }
                        }
                    }
                }

                //update size
                lowW--;
                lowZ--;
                lowY--;
                lowX--;
                highW++;
                highZ++;
                highY++;
                highX++;
            }
            var end = DateTime.Now;
            Console.WriteLine($"SolveParallelAsArray completed in: {(end - start).TotalMilliseconds}ms with answer {field.Count}");
        }

        private static void SolveSequentiallyAsGraph(List<Cube> Field)
        {
            var start = DateTime.Now;
            //loop 6 times
            var newField = new List<Cube>();
            //Console.WriteLine(Field.Count);
            for (int i = 0; i < 6; i++)
            {
                foreach (var cube in Field)
                {
                    cube.ActiveNeighbours = 0;
                }
                newField = new List<Cube>(Field);//copy current active cubes into newfield for later application of change rules

                //go down each active cube to make a list of how many active neighbours each cube has
                foreach (Cube activeCube in Field)
                {
                    var neighbours = activeCube.GetNeighbours();
                    //Console.WriteLine($"looking for {neighbours.Count} neighbours of {activeCube}");
                    int count = 0;
                    foreach (var neighbour in neighbours)
                    {
                        var c = newField.Find(c => c == neighbour);
                        //if neighbour is on new field, increase the amount of active neighbours for that cube
                        if (c != null)
                        {
                            c.ActiveNeighbours++;
                            //Console.WriteLine(c);
                        }
                        //if neighbour isn't in the new list yet, make a new one, 
                        else
                        {
                            c = new Cube(neighbour);
                            c.ActiveNeighbours++;
                            newField.Add(c);
                            count++;
                        }
                    }
                    //Console.WriteLine($"found {count} not on the list yet");
                }

                //for each cube to be considered, go down the rules
                foreach (var cube in newField)
                {
                    if (cube.Active && (cube.ActiveNeighbours == 2 || cube.ActiveNeighbours == 3))
                    {
                        //stay active
                    }
                    else if (cube.Active)
                    {
                        //Console.WriteLine($"Inactivated {cube}");
                        cube.Active = false;
                    }
                    else if (!cube.Active && cube.ActiveNeighbours == 3)
                    {
                        //Console.WriteLine($"Activated   {cube}");
                        cube.Active = true;
                    }
                }
                //Console.WriteLine($"newfield has {newField.Count} cubes");
                Field.Clear();
                foreach (var cube in newField)
                {
                    if (cube.Active)
                    {
                        Field.Add(cube);
                    }
                }
            }
            var end = DateTime.Now;
            Console.WriteLine($"SolveSequentiallyAsGraph completed in: {(end - start).TotalMilliseconds}ms with answer {Field.Count}");
        }

        private static void WriteField(List<Cube> Field)
        {
            int lowW, highW, lowZ, highZ, lowY, highY, lowX, highX;
            lowW = highW = lowZ = highZ = lowY = highY = lowX = highX = 0;
            foreach (var item in Field)
            {
                if (item.W < lowW)
                {
                    lowW = item.W;
                }
                if (item.W > highW)
                {
                    highW = item.W;
                }
                if (item.Z < lowZ)
                {
                    lowZ = item.Z;
                }
                if (item.Z > highZ)
                {
                    highZ = item.Z;
                }
                if (item.Y < lowY)
                {
                    lowY = item.Y;
                }
                if (item.Y > highY)
                {
                    highY = item.Y;
                }
                if (item.X < lowX)
                {
                    lowX = item.X;
                }
                if (item.X > highX)
                {
                    highX = item.X;
                }
            }

            for (int W = lowW; W <= highW; W++)
            {
                for (int Z = lowZ; Z <= highZ; Z++)
                {
                    for (int Y = lowY; Y <= highY; Y++)
                    {
                        StringBuilder sb = new StringBuilder();
                        for (int X = lowX; X <= highX; X++)
                        {
                            Cube c = Field.Find(c => c == (X, Y, Z, W));
                            if (c != null)
                            {
                                if (c.Active)
                                {
                                    sb.Append("#");
                                    continue;
                                }
                            }
                            sb.Append(".");
                        }
                        Console.WriteLine(sb.ToString());
                    }
                    Console.WriteLine($"Z={Z}, W={W}");
                }
            }
        }
    }

    public class Cube
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public int Z { get; private set; }
        public int W { get; private set; }
        public bool Active { get; set; } = false;
        public int ActiveNeighbours { get; set; } = 0;

        public Cube(int x, int y, int z, int w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        public Cube((int, int, int, int) t)
        {
            X = t.Item1;
            Y = t.Item2;
            Z = t.Item3;
            W = t.Item4;
        }

        public List<(int, int, int, int)> GetNeighbours()
        {
            var ret = new List<(int, int, int, int)>();
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    for (int z = -1; z <= 1; z++)
                    {
                        for (int w = -1; w <= 1; w++)
                        {
                            if (!(x == 0 && y == 0 && z == 0 && w == 0))
                            {
                                ret.Add((x + X, y + Y, z + Z, w + W));
                            }
                        }
                    }
                }
            }
            return ret;
        }

        public override string ToString()
        {
            return $"({X,2}, {Y,2}, {Z,2}, {W,2}, {Active,5}, {ActiveNeighbours,2})";
        }

        public static bool operator ==(Cube c, (int, int, int, int) t) => c.X == t.Item1 && c.Y == t.Item2 && c.Z == t.Item3 && c.W == t.Item4;
        public static bool operator !=(Cube c, (int, int, int, int) t) => !(c == t);
    }
}
