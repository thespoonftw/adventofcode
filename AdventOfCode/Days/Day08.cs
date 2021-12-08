using System;
using System.Collections.Generic;
using System.Linq;

public class Day08 : DayBase {

    protected override long PartOne(List<string> data) {
        var entries = data.Select(d => new Entry(d)).ToList();
        return entries.Sum(e => e.NumberOfUniqueSegments());
    }

    protected override long PartTwo(List<string> data) {
        var entries = data.Select(d => new Entry(d)).ToList();
        return entries.Sum(e => e.Solve());
    }

    public class Entry {

        public List<string> testStrings;
        public List<string> outputStrings;

        public Entry(string data) {
            var unorderedTest = data.Split('|')[0].Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var unorderedOutput = data.Split('|')[1].Split(' ', StringSplitOptions.RemoveEmptyEntries);
            testStrings = unorderedTest.Select(s => String.Concat(s.OrderBy(c => c))).ToList();
            outputStrings = unorderedOutput.Select(s => String.Concat(s.OrderBy(c => c))).ToList();
        }

        public int NumberOfUniqueSegments() {
            return outputStrings.Where(s => IsUniqueSegment(s)).Count();
        }

        private bool IsUniqueSegment(string s) {
            return s.Length == 2 || s.Length == 3 || s.Length == 4 || s.Length == 7;
        }

        public int Solve() {

            var solvedStrings = new string[10];

            solvedStrings[1] = testStrings.Where(s => s.Length == 2).First();           
            solvedStrings[4] = testStrings.Where(s => s.Length == 4).First();
            solvedStrings[7] = testStrings.Where(s => s.Length == 3).First();
            solvedStrings[8] = testStrings.Where(s => s.Length == 7).First();

            var fiveChars = testStrings.Where(s => s.Length == 5).ToList();
            var sixChars = testStrings.Where(s => s.Length == 6).ToList();
            var oneChars = solvedStrings[1];
            var fourUniques = solvedStrings[4].Replace(oneChars[0].ToString(), string.Empty).Replace(oneChars[1].ToString(), string.Empty);

            solvedStrings[6] = sixChars.Where(s => !s.Contains(oneChars[0]) || !s.Contains(oneChars[1])).First();
            solvedStrings[0] = sixChars.Where(s => !s.Contains(fourUniques[0]) || !s.Contains(fourUniques[1])).First();
            solvedStrings[9] = sixChars.Where(s => s != solvedStrings[0] && s != solvedStrings[6]).First();

            solvedStrings[5] = fiveChars.Where(s => s.Contains(fourUniques[0]) && s.Contains(fourUniques[1])).First();
            solvedStrings[3] = fiveChars.Where(s => s.Contains(oneChars[0]) && s.Contains(oneChars[1])).First();
            solvedStrings[2] = fiveChars.Where(s => s != solvedStrings[5] && s != solvedStrings[3]).First();

            var solvedList = solvedStrings.ToList();
            var num1 = solvedList.IndexOf(outputStrings[0]);
            var num2 = solvedList.IndexOf(outputStrings[1]);
            var num3 = solvedList.IndexOf(outputStrings[2]);
            var num4 = solvedList.IndexOf(outputStrings[3]);
            return (num1 * 1000) + (num2 * 100) + (num3 * 10) + num4;
        }
    }
}
 