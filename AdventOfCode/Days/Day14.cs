using System;
using System.Collections.Generic;
using System.Linq;

public class Day14 : DayBase {

    protected override long PartOne(List<string> data) {
        var polymer = new Polymer(data);
        for (int i = 0; i < 10; i++) { polymer.TakeStep(); }
        return polymer.Evaluate();
    }

    protected override long PartTwo(List<string> data) {
        var polymer = new Polymer(data);
        for (int i = 0; i < 40; i++) { polymer.TakeStep(); }
        return polymer.Evaluate();
    }

    class Polymer {

        private Dictionary<string, long> polymerPairs = new Dictionary<string, long>();
        private readonly Dictionary<string, char> polyRules = new Dictionary<string, char>();
        private readonly char finalCharacter;

        public Polymer(List<string> data) {
            var initialPolymer = data[0];
            finalCharacter = initialPolymer.Last();

            // initialise polymer pairs
            for (int i=0; i<initialPolymer.Length - 1; i++) {
                var pair = initialPolymer.Substring(i, 2);
                ModifyDict(polymerPairs, pair, 1);
            }

            // initialise polymer rules
            for (int i = 2; i<data.Count; i++) {
                var split = data[i].Split(" -> ");
                polyRules.Add(split[0], split[1][0]);            
            }
        }

        public void TakeStep() { 
            var newDict = new Dictionary<string, long>();
            foreach (var entry in polymerPairs) {
                var newCharacter = polyRules[entry.Key];
                var pair1 = entry.Key[0] + newCharacter.ToString();
                var pair2 = newCharacter.ToString() + entry.Key[1];
                ModifyDict(newDict, pair1, entry.Value);
                ModifyDict(newDict, pair2, entry.Value);
            }
            polymerPairs = newDict;
        }

        public long Evaluate() {
            var letters = new Dictionary<char, long>();
            foreach (var pair in polymerPairs) {
                ModifyDict(letters, pair.Key[0], pair.Value);
            }
            ModifyDict(letters, finalCharacter, 1);
            return letters.Values.Max() - letters.Values.Min();
        }

        public void ModifyDict<T>(Dictionary<T, long> dict, T key, long amount) { 
            if (!dict.ContainsKey(key)) {
                dict.Add(key, 0);
            }
            dict[key] += amount;
        }

    }


}
 