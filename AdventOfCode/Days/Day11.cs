using System;
using System.Collections.Generic;
using System.Linq;

public class Day11 : DayBase {

    protected override long PartOne(List<string> data) {
        var octopuses = new Octopuses(data);
        var counter = 0;
        for (int i = 0; i<100; i++) {
            counter += octopuses.TakeStep();
        }
        return counter;
    }

    protected override long PartTwo(List<string> data) {
        var octopuses = new Octopuses(data);
        int counter = 0;
        int flashes = 0;
        while (flashes < 100) {
            counter++;
            flashes = octopuses.TakeStep();
        }
        return counter;
    }

    class Octopuses {

        private const int gridSize = 10;
        private const int flashLevel = 10;
        Octopus[,] array = new Octopus[gridSize, gridSize];
        List<Octopus> list = new List<Octopus>();
        
        public int NumberOfFlashes { get; private set; }

        public Octopuses(List<string> data) {
            for (int x = 0; x < gridSize; x++) {
                for (int y = 0; y < gridSize; y++) {
                    var oct = new Octopus(x, y, data[x][y] - '0');
                    array[x, y] = oct;
                    list.Add(oct);
                }
            }
        }

        public int TakeStep() {

            list.ForEach(o => o.hasFlashed = false);
            list.ForEach(o => o.value++);

            var octopusToCheck = list.Where(o => o.value >= flashLevel).ToList();
            var flashCount = 0;

            while (octopusToCheck.Count > 0) {
                var newOctopus = new List<Octopus>();

                foreach (var o in octopusToCheck) {
                    if (o.value >= flashLevel && !o.hasFlashed) {
                        flashCount++;
                        o.hasFlashed = true;

                        var neighbours = GetNeighbours(o);
                        neighbours.RemoveAll(n => n.hasFlashed);
                        neighbours.ForEach(n => n.value++);
                        newOctopus.AddRange(neighbours);
                    }
                }
                octopusToCheck = newOctopus.Distinct().ToList();
            }

            list.Where(o => o.value >= flashLevel).ToList().ForEach(o => o.value = 0);

            return flashCount;
        }

        private List<Octopus> GetNeighbours(Octopus o) {
            var returner = new List<Octopus>();
            TryAddNeighbour(returner, o.x - 1, o.y);
            TryAddNeighbour(returner, o.x + 1, o.y);
            TryAddNeighbour(returner, o.x, o.y - 1);
            TryAddNeighbour(returner, o.x, o.y + 1);
            TryAddNeighbour(returner, o.x - 1, o.y - 1);
            TryAddNeighbour(returner, o.x + 1, o.y - 1);
            TryAddNeighbour(returner, o.x - 1, o.y + 1);
            TryAddNeighbour(returner, o.x + 1, o.y + 1);
            return returner;
        }

        private void TryAddNeighbour(List<Octopus> list, int x, int y) {
            if (x < 0 || y < 0 || x > gridSize - 1 || y > gridSize - 1) { return; }
            list.Add(array[x, y]);
        }


    }

    class Octopus {

        public int value;
        public int x;
        public int y;
        public bool hasFlashed = false;

        public Octopus(int x, int y, int value) {
            this.x = x;
            this.y = y;
            this.value = value;
        }





    }

}
 