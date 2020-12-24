using System;
using System.Collections.Generic;
using System.IO;

namespace Lobby_Layout
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputList = new List<string>();
            StreamReader sr = new StreamReader(@"C:\Users\eiedu\Source\Repos\AdventOfCode\Lobby Layout\TextFile1.txt");
            string line = string.Empty;
            while ((line = sr.ReadLine()) != null)
            {
                inputList.Add(line);
            }

            var blackTiles = new SortedSet<(int, int)>();

            var pathList = new List<string>();
            //nw = a, ne = b, se = c, sw = d
            foreach (var path in inputList)
            {
                pathList.Add(path.Replace("nw", "a").Replace("ne", "b").Replace("se", "c").Replace("sw", "d"));
            }

            foreach (var path in pathList)
            {
                int y = 0;
                int x = 0;
                foreach (var c in path)
                {
                    switch (c)
                    {
                        case 'a':
                            x--;
                            y++;
                            break;
                        case 'b':
                            y++;
                            break;
                        case 'c':
                            x++;
                            y--;
                            break;
                        case 'd':
                            y--;
                            break;
                        case 'w':
                            x--;
                            break;
                        case 'e':
                            x++;
                            break;
                        default:
                            break;
                    }
                }
                if (!blackTiles.Add((y,x)))
                {
                    blackTiles.Remove((y, x));
                }
            }

            Console.WriteLine(blackTiles.Count);

            //black tiles stay black if they have 1 or 2 adjacent black tiles
            //white tiles with 2 adjacent black tiles are flipped
            for (int i = 1; i <= 100; i++)
            {
                var flipDict = new Dictionary<(int, int), int>();
                foreach (var tile in blackTiles)
                {
                    //x+1
                    var e = (tile.Item1, tile.Item2 + 1);
                    AddOrUp(flipDict, e);
                    //x-1
                    var w = (tile.Item1, tile.Item2 - 1);
                    AddOrUp(flipDict, w);
                    //y+1
                    var ne = (tile.Item1 + 1, tile.Item2);
                    AddOrUp(flipDict, ne);
                    //y-1
                    var sw = (tile.Item1 - 1, tile.Item2);
                    AddOrUp(flipDict, sw);
                    //x+1;y-1
                    var se = (tile.Item1 - 1, tile.Item2 + 1);
                    AddOrUp(flipDict, se);
                    //x-1;y+1
                    var nw = (tile.Item1 + 1, tile.Item2 - 1);
                    AddOrUp(flipDict, nw);
                }

                //we know all tiles' adjacencies now
                var newBlackTiles = new SortedSet<(int, int)>();
                foreach (var tile in flipDict)
                {
                    if (blackTiles.Contains(tile.Key)) //tile was black
                    {
                        if (tile.Value == 1 || tile.Value == 2)
                        {
                            newBlackTiles.Add(tile.Key);
                        }
                    }
                    else
                    {
                        if (tile.Value == 2)
                        {
                            newBlackTiles.Add(tile.Key);
                        }
                    }
                }
                blackTiles = new SortedSet<(int, int)>(newBlackTiles);
                newBlackTiles.Clear();
                Console.WriteLine($"Day {i}: {blackTiles.Count}");
            }
        }

        private static void AddOrUp(Dictionary<(int, int), int> flipDict, (int, int) e)
        {
            if (flipDict.ContainsKey(e))
            {
                flipDict[e]++;
            }
            else
            {
                flipDict.Add(e, 1);
            }
        }
    }
}
