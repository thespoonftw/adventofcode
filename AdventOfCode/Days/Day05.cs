using System;
using System.Collections.Generic;
using System.Linq;

public class Day05 : DayBase {

    protected override string PartOne(List<string> data) {
        var vents = data.Select(line => new HydrothermalVent(line)).ToList();
        var straightVents = vents.Where(v => !v.isDiagonal).ToList();
        var ventMap = new VentMap(straightVents);
        return ventMap.EvaluateMap().ToString();
    }

    protected override string PartTwo(List<string> data) {
        var vents = data.Select(line => new HydrothermalVent(line)).ToList();
        var ventMap = new VentMap(vents);
        return ventMap.EvaluateMap().ToString();
    }

    class HydrothermalVent {

        public readonly int x1;
        public readonly int x2;
        public readonly int y1;
        public readonly int y2;
        public readonly bool isDiagonal;
        public readonly int length;

        public HydrothermalVent(string line) {
            var split = line.Split(" -> ");
            var coord1 = split[0].Split(',');
            var coord2 = split[1].Split(',');
            x1 = int.Parse(coord1[0]);
            y1 = int.Parse(coord1[1]);
            x2 = int.Parse(coord2[0]);
            y2 = int.Parse(coord2[1]);
            length = Math.Max(Math.Abs(x2 - x1), Math.Abs(y2 - y1));
            isDiagonal = (x1 != x2) && (y1 != y2);            
        }
    }

    class VentMap {

        private readonly int[,] array;
        private readonly int gridSize;

        public VentMap(List<HydrothermalVent> vents) {
            gridSize = 1000;
            array = new int[gridSize, gridSize];
            vents.ForEach(v => ApplyVent(v));            
        }

        private void ApplyVent(HydrothermalVent vent) {
            var xStep = vent.x2.CompareTo(vent.x1);
            var yStep = vent.y2.CompareTo(vent.y1);
            for (int i = 0; i <= vent.length; i++) {
                array[vent.x1 + (i*xStep), vent.y1 + (i*yStep)]++;
            }
        }

        public int EvaluateMap() {
            var counter = 0;
            for (int x = 0; x < gridSize; x++) {
                for (int y = 0; y < gridSize; y++) {
                    if (array[x, y] > 1) { counter++; }
                }
            }
            return counter;
        }
        
    }
}
 