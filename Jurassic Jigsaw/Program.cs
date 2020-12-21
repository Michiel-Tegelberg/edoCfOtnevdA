using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Jurassic_Jigsaw
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputList = new List<List<string>>();
            //StreamReader sr = new StreamReader(@"C:\Users\eiedu\Source\Repos\AdventOfCode\Jurassic Jigsaw\TextFile2.txt");
            StreamReader sr = new StreamReader(@"C:\Users\eiedu\Source\Repos\AdventOfCode\Jurassic Jigsaw\TextFile1.txt");
            string line = string.Empty;
            var tempList = new List<string>();
            while ((line = sr.ReadLine()) != null)
            {
                if (line == string.Empty)
                {
                    inputList.Add(tempList);
                    tempList = new List<string>();
                }
                else
                {
                    tempList.Add(line);
                }
            }
            inputList.Add(tempList);

            List<Tile> tiles = ResetTiles(inputList);

            //now try to start with each tile in each orientation and snake your way through all the tiles
            int gridSize = Convert.ToInt32(Math.Sqrt(tiles.Count));
            Tile[,] grid = new Tile[gridSize, gridSize];
            Console.WriteLine(Solve(grid, tiles, 0, 0));
        }

        public static List<Tile> ResetTiles(List<List<string>> inputList)
        {
            var tiles = new List<Tile>();
            foreach (var tile in inputList)
            {
                tiles.Add(new Tile(tile));
            }

            return tiles;
        }

        public static bool Solve(Tile[,] grid, List<Tile> tiles, int y, int x)
        {
            int maxX = grid.GetUpperBound(1);
            int maxY = grid.GetUpperBound(0);
            if (y == maxY && x == maxX)
            {
                bool done = CheckTile(tiles[0], grid, y, x);
                if (done)
                {
                    Console.WriteLine(grid[0, 0].ID * grid[0, maxX].ID * grid[maxY, 0].ID * grid[maxY, maxX].ID);
                }
                else
                {
                    return false;
                }
            }

            foreach (var tile in tiles)
            {
                var gridCopy = (Tile[,])grid.Clone();
                //Console.WriteLine("++++++++++++++++++++++++++++++++++++");
                //DrawGrid(grid);
                //try to place each tile
                for (int i = 0; i < 8; i++)
                {
                    if (CheckTile(tile, gridCopy, y, x))
                    {
                        //the tile has been placed so do recursion
                        var tileCopy = new List<Tile>(tiles);
                        tileCopy.Remove(tile);
                        int newX = x + 1;
                        int newY = y;
                        if (newX > maxX)
                        {
                            newY++;
                            newX = 0;
                        }
                        bool ret = Solve(gridCopy, tileCopy, newY, newX);
                        if (ret)
                        {
                            return ret;
                        }
                    }
                    //Console.WriteLine($"not: {tile.ID}.{tile.Orientation}");
                    tile.NextOrientation();
                }
            }
            return false;
        }

        public static void DrawGrid(Tile[,] grid)
        {
            int maxy = grid.GetUpperBound(0);
            int maxx = grid.GetUpperBound(1);

            for (int y = 0; y <= maxy; y++)
            {
                StringBuilder sb = new StringBuilder("\n");
                for (int x = 0; x <= maxx; x++)
                {
                    long id = grid[y, x] == null ? 0 : grid[y, x].ID;
                    sb.Append($"{id,4} ");
                }
                Console.WriteLine(sb.ToString());
            }
        }

        /// <summary>
        /// Checks whether Tile t can be placed on the grid on place y,x in any orientation and places it if possible.
        /// </summary>
        /// <param name="t">Tile t</param>
        /// <param name="grid">the grid</param>
        /// <param name="y">the position Tile t will be placed on</param>
        /// <param name="x">the position Tile t will be placed on</param>
        /// <param name="maxY">the maximum Y index</param>
        /// <param name="maxX">the maximum X index</param>
        /// <returns></returns>
        static bool CheckTile(Tile t, Tile[,] grid, int y, int x)
        {
            int maxX = grid.GetUpperBound(1);
            int maxY = grid.GetUpperBound(0);
            for (int i = 0; i < 8; i++)
            {
                //Console.WriteLine($"trying to fit {t.ID}.{t.Orientation}");
                bool fits = true;
                //left
                if (x > 0)
                {
                    Tile neighbour = grid[y, x - 1];
                    if (neighbour != null)
                    {
                        if (neighbour.Right != t.Left)
                        {
                            fits = false;
                        }
                    }
                }
                //right
                if (x < maxX)
                {
                    Tile neighbour = grid[y, x + 1];
                    if (neighbour != null)
                    {
                        if (neighbour.Left != t.Right)
                        {
                            fits = false;
                        }
                    }
                }
                //top
                if (y > 0)
                {
                    Tile neighbour = grid[y - 1, x];
                    if (neighbour != null)
                    {
                        if (neighbour.Bottom != t.Top)
                        {
                            fits = false;
                        }
                    }
                }
                //bottom
                if (y < maxY)
                {
                    Tile neighbour = grid[y + 1, x];
                    if (neighbour != null)
                    {
                        if (neighbour.Top != t.Bottom)
                        {
                            fits = false;
                        }
                    }
                }
                if (!fits)
                {
                    t.NextOrientation();
                }
                else
                {
                    grid[y, x] = t;
                    return true;
                }
            }
            return false;
        }


    }

    public enum FlipAxis
    {
        HORIZONTAL,
        COUNTERCLOCKWISE
    }

    public class Tile
    {
        public int Left { get; set; }
        public int Top { get; set; }
        public int Right { get; set; }
        public int Bottom { get; set; }
        public long ID { get; set; }
        private int Size { get; set; }
        public int Orientation { get; set; }

        public Tile(List<string> l)
        {
            Orientation = 0;
            //get id
            ID = int.Parse(l[0].Split(' ')[1].Trim(':'));
            l.RemoveAt(0);

            //create all side strings from left to right and top to bottom
            int y = l.Count; //highest y index exclusive
            int x = l[1].Length; //highest x index exclusive
            Size = y;
            Top = TileSideToInt(l[0]);
            Bottom = TileSideToInt(l[y - 1]);

            StringBuilder leftString = new StringBuilder();
            for (int i = 0; i < y; i++)
            {
                leftString.Append(l[i][0]);
            }
            Left = TileSideToInt(leftString.ToString());

            StringBuilder rightString = new StringBuilder();
            for (int i = 0; i < y; i++)
            {
                rightString.Append(l[i][x - 1]);
            }
            Right = TileSideToInt(rightString.ToString());
        }

        //All orientations can be achieved by 0, CCW, CCW, CCW, HZ, CCW, CCW, CCW
        public void NextOrientation()
        {
            switch (Orientation)
            {
                case 0:
                    Orientation++;
                    Flip(FlipAxis.COUNTERCLOCKWISE);
                    break;
                case 1:
                    Orientation++;
                    Flip(FlipAxis.COUNTERCLOCKWISE);
                    break;
                case 2:
                    Orientation++;
                    Flip(FlipAxis.COUNTERCLOCKWISE);
                    break;
                case 3:
                    Orientation++;
                    Flip(FlipAxis.HORIZONTAL);
                    break;
                case 4:
                    Orientation++;
                    Flip(FlipAxis.COUNTERCLOCKWISE);
                    break;
                case 5:
                    Orientation++;
                    Flip(FlipAxis.COUNTERCLOCKWISE);
                    break;
                case 6:
                    Orientation++;
                    Flip(FlipAxis.COUNTERCLOCKWISE);
                    break;
                case 7:
                    Orientation = 0;
                    Flip(FlipAxis.HORIZONTAL);
                    break;
                default:
                    throw new Exception("unexpected flip");
            }
        }

        private void Flip(FlipAxis flipAxis)
        {
            switch (flipAxis)
            {
                //case FlipAxis.VERTICAL:
                //    int temp = Left;
                //    Left = Right;
                //    Right = temp;
                //    Top = InvertInt(Top);
                //    Bottom = InvertInt(Bottom);
                //    break;

                case FlipAxis.HORIZONTAL:
                    int temp = Top;
                    Top = Bottom;
                    Bottom = temp;
                    Left = InvertInt(Left);
                    Right = InvertInt(Right);
                    break;

                //case FlipAxis.CLOCKWISE:
                //    temp = Right;
                //    Right = Top;
                //    Top = InvertInt(Left);
                //    Left = Bottom;
                //    Bottom = InvertInt(temp);
                //    break;

                case FlipAxis.COUNTERCLOCKWISE:
                    temp = Left;
                    Left = InvertInt(Top);
                    Top = Right;
                    Right = InvertInt(Bottom);
                    Bottom = temp;
                    break;

                default:
                    break;
            }
        }

        private int TileSideToInt(string side)
        {
            int ret = 0;
            for (int i = 0; i < side.Length; i++)
            {
                ret <<= 1;
                if (side[i] == '#')
                {
                    ret++;
                }
            }
            return ret;
        }

        private int InvertInt(int n)
        {
            int ret = 0;
            for (int i = Size; i > 0; i--)
            {
                ret <<= 1;
                if ((n & 1) == 1)
                {
                    ret++;
                }
                n >>= 1;
            }
            return ret;
        }

        public override string ToString()
        {
            return $"ID: {ID} | {Top,4} {Right,4} {Bottom,4} {Left,4} | {Orientation}";
        }
    }
}
