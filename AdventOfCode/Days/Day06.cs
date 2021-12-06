using System;
using System.Collections.Generic;
using System.Linq;

public class Day06 : DayBase {

    private const int REPRODUCTION_DAYS = 7;
    private const int MATURITY_DAYS = 2;

    protected override long PartOne(List<string> data) {
        return Simulate(data[0], 80);
    }

    protected override long PartTwo(List<string> data) {
        return Simulate(data[0], 256);
    }

    private long Simulate(string data, int numberOfDays) {
        var sim = new LanternFishSimulation(data);        
        for (int i = 0; i <= numberOfDays; i++) {
            sim.AgeFish();
        }
        return sim.GetNumberOfFish();
    }
    
    class LanternFishSimulation {

        private Dictionary<int, long> fishDict = new Dictionary<int, long>();
        private int dayCounter = 0;

        public LanternFishSimulation(string data) {
            for (int i = 0; i < REPRODUCTION_DAYS + MATURITY_DAYS; i++) {
                fishDict[i] = 0;
            }
            data.Split(',').ToList().ForEach(s => AddFishFromData(s));
        }

        private void AddFishFromData(string s) {
            fishDict[int.Parse(s)]++;
        }

        public void AgeFish() {
            dayCounter++;
            var newFish = fishDict[0];
            for (int i = 1; i < REPRODUCTION_DAYS + MATURITY_DAYS; i++) {
                fishDict[i - 1] = fishDict[i];
            }
            fishDict[REPRODUCTION_DAYS - 1] += newFish;
            fishDict[REPRODUCTION_DAYS + MATURITY_DAYS - 1] = newFish;
        }

        public long GetNumberOfFish() {
            long counter = 0;
            for (int i = 0; i < REPRODUCTION_DAYS + MATURITY_DAYS - 1; i++)
            {
                counter += fishDict[i];
            }
            return counter;
        }
    }

}
 