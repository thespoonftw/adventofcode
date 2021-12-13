using System;
using System.Collections.Generic;
using System.Linq;

public class Day13 : DayBase {

    protected override long PartOne(List<string> data) {
        var sheet = new Sheet(data);
        return sheet.FoldAndCount();
    }

    protected override long PartTwo(List<string> data) {
        var sheet = new Sheet(data);
        sheet.FoldAndPrint();
        return 0;
    }

    class Sheet {

        private const int MAX = 1500;
        private bool[,] array = new bool[MAX, MAX];
        private List<Instruction> instructions = new List<Instruction>();
        

        public Sheet(List<string> data) {

            var seperatorIndex = data.IndexOf("");
            for (int i=0; i<seperatorIndex; i++) {
                var split = data[i].Split(',');
                array[int.Parse(split[0]), int.Parse(split[1])] = true;
            }
            for (int i=seperatorIndex +1; i<data.Count; i++) {
                instructions.Add(new Instruction(data[i]));
            }

        }

        public int FoldAndCount() {
            Fold(instructions[0]);            
            var counter = 0;
            for (int x = 0; x < MAX; x++) {
                for (int y = 0; y < MAX; y++) { 
                    if (array[x, y]) { counter++; }
                }
            }
            return counter;
        }

        public void FoldAndPrint() { 
            foreach (var i in instructions) { Fold(i); }
            for (int y = 0; y < 7; y++) {
                var s = "";
                for (int x = 0; x < 40; x++) {
                    s += array[x, y] ? '#' : '_';
                }
                Console.WriteLine(s);
            }
        }

        private void Fold(Instruction ins) { 
            for (int i = 0; i < ins.foldLine; i++) { 
                for (int j = 0; j < MAX; j++) {
                    var foldI = 2 * ins.foldLine - i;
                    if (ins.isXfold) {
                        if (array[foldI, j]) { array[i, j] = true; }
                        array[foldI, j] = false;
                    } else {
                        if (array[j, foldI]) { array[j, i] = true; }
                        array[j, foldI] = false;
                    }
                }
            }
        }

    }

    class Instruction {

        public readonly bool isXfold;
        public readonly int foldLine;

        public Instruction(string input) {
            var split = input.Split('=');
            isXfold = split[0].Last() == 'x';
            foldLine = int.Parse(split[1]);
        }

    }
}
 