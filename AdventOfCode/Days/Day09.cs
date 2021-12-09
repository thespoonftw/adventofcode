using System;
using System.Collections.Generic;
using System.Linq;

public class Day09 : DayBase {

    protected override long PartOne(List<string> data) {
        var heightmap = new Heightmap(data);
        return heightmap.EvaluateSimple();
    }

    protected override long PartTwo(List<string> data) {
        var heightmap = new Heightmap(data);
        return heightmap.EvaluateBasins();
    }

    private class Heightmap {

        public int width;
        public int height;
        public Coord[,] array;

        public Heightmap(List<string> data) {
            width = data[0].Length;
            height = data.Count;
            array = new Coord[width, height];

            for (int x = 0; x < width; x++) {
                for (int y = 0; y < height; y++) {
                    var value = data[y][x] - '0';
                    array[x, y] = new Coord(x, y, value, this);
                }
            }
        }

        public int EvaluateSimple() {
            return GetLowPoints().Sum(c => c.value + 1);
        }

        public int EvaluateBasins() {
            var topBasins = GetLowPoints().Select(c => FindBasinSize(c)).OrderByDescending(c => c).Take(3).ToList();
            return topBasins[0] * topBasins[1] * topBasins[2];
        }

        public List<Coord> GetLowPoints() {
            var returner = new List<Coord>();
            for (int x = 0; x < width; x++) {
                for (int y = 0; y < height; y++) {
                    var c = array[x, y];
                    if (!c.IsHigher(Direction.up)) { continue; }
                    if (!c.IsHigher(Direction.right)) { continue; }
                    if (!c.IsHigher(Direction.down)) { continue; }
                    if (!c.IsHigher(Direction.left)) { continue; }
                    returner.Add(c);
                }
            }
            return returner;
        }

        public int FindBasinSize(Coord lowPoint) {
            var basinPoints = new List<Coord>() { lowPoint };
            IterativelyCheck(basinPoints, lowPoint);
            return basinPoints.Count;
        }

        public void IterativelyCheck(List<Coord> basinSoFar, Coord c) {
            TryAddToBasin(c, Direction.up, basinSoFar);
            TryAddToBasin(c, Direction.right, basinSoFar);
            TryAddToBasin(c, Direction.down, basinSoFar);
            TryAddToBasin(c, Direction.left, basinSoFar);
        }

        public void TryAddToBasin(Coord c, Direction dir, List<Coord> basinSoFar) {
            var n = c.Neighbour(dir);
            if (n == null) { return; }
            if (basinSoFar.Contains(n)) { return; }
            if (n.value != 9) { 
                basinSoFar.Add(n);
                IterativelyCheck(basinSoFar, n);
            }
        }
    }

    private class Coord
    {
        public int x;
        public int y;
        public int value;
        private Heightmap map;

        public Coord(int x, int y, int value, Heightmap map) {
            this.x = x;
            this.y = y;
            this.map = map;
            this.value = value;
        }

        public Coord Neighbour(Direction dir) {
            switch (dir) {
                case Direction.left:
                    if (x <= 0) { return null; }
                    else { return map.array[x - 1, y]; }
                case Direction.up:
                    if (y <= 0) { return null; }
                    else { return map.array[x, y - 1]; }
                case Direction.right:
                    if (x >= map.width - 1) { return null; }
                    else { return map.array[x + 1, y]; }
                default:
                    if (y >= map.height - 1) { return null; }
                    else { return map.array[x, y + 1]; }
            }
        }

        public bool IsHigher(Direction dir) {
            var n = Neighbour(dir);
            return n == null || n.value > value;
        }
    }

    public enum Direction {
        up,
        down,
        left, 
        right
    }


}
 