using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;

namespace Rain_Risk
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputList = new List<string>();
            StreamReader sr = new StreamReader(@"C:\Users\eiedu\Source\Repos\AdventOfCode\Rain Risk\TextFile1.txt");
            //StreamReader sr = new StreamReader(@"C:\Users\eiedu\Source\Repos\AdventOfCode\Rain Risk\TextFile2.txt");
            string line = string.Empty;
            while ((line = sr.ReadLine()) != null)
            {
                inputList.Add(line);
            }

            float y = 0;
            float x = 0;
            Vector2 waypoint = new Vector2(10, -1);

            foreach (var command in inputList)
            {
                char c = command[0];
                int num = int.Parse(command.Substring(1));

                switch (c)
                {
                    case 'N':
                        waypoint.Y -= num;
                        break;
                    case 'S':
                        waypoint.Y += num;
                        break;
                    case 'E':
                        waypoint.X += num;
                        break;
                    case 'W':
                        waypoint.X -= num;
                        break;
                    case 'L':
                        waypoint = waypoint.RotateCCW(num);
                        break;
                    case 'R':
                        waypoint = waypoint.RotateCW(num);
                        break;
                    case 'F':
                            y += waypoint.Y * num;
                            x += waypoint.X * num;
                        break;
                    default:
                        Console.WriteLine($"{c} {num} +++++++++++++++++++++++++++++");
                        break;
                }

                Console.WriteLine($"{c} | {num,3} | {waypoint,10} | {x,4} | {y,4} ");
            }

            Console.WriteLine($"{x}, {y}, {waypoint}, {Math.Abs(y) + Math.Abs(x)}");
        }
    }

    public static class Extensions
    {
        public static Vector2 RotateCW(this Vector2 v, int angle)
        {
            switch (angle)
            {
                case 90:
                    //right 90 deg means 4,-3 becomes 3, 4
                    return new Vector2(-v.Y, v.X);
                case 180:
                    //right 180 deg means 4,-3 becomes -4, 3
                    return new Vector2(-v.X, -v.Y);
                case 270:
                    //right 270 deg means 4,-3 becomes -3, -4
                    return new Vector2(v.Y, -v.X);
                default:
                    return v;
            }
        }
        public static Vector2 RotateCCW(this Vector2 v, int angle)
        {
            switch (angle)
            {
                case 90:
                    //left 90 deg means 4,-3 becomes -3, -4
                    return new Vector2(v.Y, -v.X);
                case 180:
                    //left 180 deg means 4,-3 becomes -4, 3
                    return new Vector2(-v.X, -v.Y);
                case 270:
                    //left 270 deg means 4,-3 becomes 3, 4
                    return new Vector2(-v.Y, v.X);
                default:
                    return v;
            }
        }

        public static double ToRadians(this int n)
        {
            Console.WriteLine($"int: {n}, rad: {(Math.PI / 180) * n}");
            return (Math.PI / 180) * n;
        }
    }
}
