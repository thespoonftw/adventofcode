using System;
using System.Collections.Generic;
using System.Linq;

public class Day07 : DayBase {

    protected override long PartOne(List<string> data) {
        var crabs = new Crabs(data[0], false);
        return crabs.FindBestFuelCost();
    }

    protected override long PartTwo(List<string> data) {
        var crabs = new Crabs(data[0], true);
        return crabs.FindBestFuelCost();
    }

    class Crabs {

        private readonly List<int> crabPositions;
        private readonly List<int> triangleNumbers = new List<int>();
        private readonly int maxValue;
        private readonly bool isPartTwo;

        public Crabs(string line, bool isPartTwo) {
            crabPositions = line.Split(',').ToList().Select(n => int.Parse(n)).ToList();
            maxValue = crabPositions.Max();
            this.isPartTwo = isPartTwo;
            if (isPartTwo) { SetTriangleNumbers(); }            
        }

        private void SetTriangleNumbers() {
            triangleNumbers.Add(0);
            for (int i = 1; i < maxValue + 1; i++) {
                var prev = triangleNumbers[i - 1];
                triangleNumbers.Add(i + prev);
            }
        }

        public int FindBestFuelCost() {
            var bestFuelCost = EvaluateFuelCost(0);
            for (int i=1; i<maxValue; i++) {
                var newFuelCost = EvaluateFuelCost(i);
                if (newFuelCost < bestFuelCost) {
                    bestFuelCost = newFuelCost;
                }
            }
            return bestFuelCost;
        }

        private int EvaluateFuelCost(int value) {
            var counter = 0;
            foreach (var c in crabPositions) {
                var distance = Math.Abs(c - value);
                var fuelCost = isPartTwo ? triangleNumbers[distance] : distance;
                counter += fuelCost;
            }
            return counter;
        }

    }

}
 