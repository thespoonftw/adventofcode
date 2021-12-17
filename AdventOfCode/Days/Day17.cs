using System;
using System.Collections.Generic;
using System.Linq;

public class Day17 : DayBase {

    protected override long PartOne(List<string> data) {
        var probeShooter = new ProbeShooter(data[0]);
        return probeShooter.GetMaxYValue();
    }

    protected override long PartTwo(List<string> data) {
        var probeShooter = new ProbeShooter(data[0]);
        return probeShooter.GetNumberOfSolutions();
    }

    class ProbeShooter
    {
        private int targetMinX;
        private int targetMaxX;
        private int targetMinY;
        private int targetMaxY;
        private List<Trajectory> trajectories = new List<Trajectory>();

        private const int velocityMaxX = 500;
        private const int velocityMaxY = 1000;
        private const int velocityMinY = -1000;

        public ProbeShooter(string input) {
            var split = input.Split(new char[] { ' ', '=', '.', ',' }, StringSplitOptions.RemoveEmptyEntries);
            targetMinX = int.Parse(split[3]);
            targetMaxX = int.Parse(split[4]);
            targetMinY = int.Parse(split[6]);
            targetMaxY = int.Parse(split[7]);

            for (int x = 0; x <= velocityMaxX; x++) {
                for (int y = velocityMinY; y <= velocityMaxY; y++) {
                    trajectories.Add(GetTrajectory(x, y));
                }
            }
        }

        public int GetMaxYValue() {
            var best = trajectories.Where(t => t.hitsTarget).OrderByDescending(t => t.maxY).First();
            Console.WriteLine("x:" + best.dx + " y:" + best.dy);
            return best.maxY;
        }

        public int GetNumberOfSolutions() {
            var l = trajectories.Where(t => t.hitsTarget);
            Console.WriteLine("x:" + l.Min(t => t.dx) + ".." + l.Max(t => t.dx) + " y:" +
                l.Min(t => t.dy) + ".." + l.Max(t => t.dy));
            return l.Count();
        }

        private Trajectory GetTrajectory(int dx, int dy) {
            var returner = new Trajectory();
            returner.dx = dx;
            returner.dy = dy;
            var x = 0;
            var y = 0;
            while (x < targetMaxX && y > targetMinY) {
                x += dx;
                y += dy;
                if (y > returner.maxY) { returner.maxY = y; }
                if (dx > 0) { dx--; }
                if (dx < 0) { dx++; }
                dy--;
                if (x >= targetMinX && x <= targetMaxX && y >= targetMinY && y <= targetMaxY) { returner.hitsTarget = true; }
            }
            return returner;
        }

        class Trajectory {
            public bool hitsTarget;
            public int maxY;
            public int dx;
            public int dy;
        }
    }

}
 