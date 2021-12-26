using System;
using System.Collections.Generic;
using System.Linq;

public class Day24 : DayBase {

    protected override long PartOne(List<string> data) {
        var alu = new ALU(data);
        alu.Solve();
        return 0;
    }

    protected override long PartTwo(List<string> data) {
        return 0;
    }

    public class ALU {

        public List<(int divZ, int addX, int addY)> instructions = new List<(int divZ, int addX, int addY)>();

        private List<Solution> solutions = new List<Solution>();

        public ALU(List<string> input) {

            for (int i = 0; i < input.Count; i += 18) {
                var divZ = int.Parse(input[i + 4].Split(' ')[2]);
                var addX = int.Parse(input[i + 5].Split(' ')[2]);
                var addY = int.Parse(input[i + 15].Split(' ')[2]);
                instructions.Add((divZ, addX, addY));
            }
        }

        public long Solve() {

            for (int i=1; i<=9; i++) {
                solutions.Add(new Solution(i));
            }

            for (int i=12; i >= 0; i--) {

                var ins = instructions[i];
                var newSolutions = new List<Solution>();

                foreach (var s in solutions) {
                    for (int w = 1; w <= 9; w++) {
                        for (int z = -30; z <= 30; z++) {
                            var endZ = RunInstruction(w, z, ins);
                            if (endZ == s.currentZ) {
                                newSolutions.Add(new Solution(w, s));
                            }
                        }
                    }
                }

                solutions = newSolutions;
            }


            return 0;
        }

        // returns value of z
        private int RunInstruction(int inputW, int inputZ, (int divZ, int addX, int addY) t) {
            var z = inputZ;
            var x = (z % 26) + t.addX;
            z /= t.divZ;
            x = (inputW == x) ? 0 : 1;
            var y = (25 * x) + 1;
            z *= y;
            y = (inputW + t.addY) * x;
            z += y;
            return z;
        }

        public class Solution {

            public List<int> wValues = new List<int>();
            public int currentZ;

            public Solution(int w) {
                wValues = new List<int>() { w };
                currentZ = wValues.Count;
            }

            public Solution(int w, Solution s) {
                wValues = new List<int>(s.wValues);
                wValues.Add(w);
                currentZ = wValues.Count;
            }

        }
    }

}
 