using System;
using System.Collections.Generic;
using System.Linq;

public class Day10 : DayBase {

    protected override long PartOne(List<string> data) {
        var lines = data.Select(d => new Line(d));
        return lines.Sum(d => d.Evaluate(true));
    }

    protected override long PartTwo(List<string> data) {
        var lines = data.Select(d => new Line(d)).ToList();
        var allScores = lines.Select(l => l.Evaluate(false)).ToList();
        var validScores = allScores.RemoveAll(x => x == 0);
        allScores.Sort();
        return allScores[allScores.Count / 2];
    }

    private class Line {

        string dataString;

        public Line(string input) {
            dataString = input;
        }

        public long Evaluate(bool isPartOne) {
            var openBrackets = new List<char>();
            for (int i = 0; i < dataString.Length; i++) {
                var c = dataString[i];

                // open bracket
                if (c == '(' || c == '[' || c == '{' || c == '<') {
                    openBrackets.Add(c);

                // closing bracket
                } else if (DoBracketsMatch(openBrackets.Last(), c)) {
                    openBrackets.RemoveAt(openBrackets.Count - 1);

                // corruption
                } else if (isPartOne) {
                    return GetCorruptionScore(c);
                } else {
                    return 0;
                }
            }

            // incompletion
            if (isPartOne) {
                return 0;
            } else {
                long scorer = 0;
                for (int i=openBrackets.Count - 1; i>=0; i--) {
                    scorer *= 5;
                    scorer += GetIncompletionScore(openBrackets[i]);
                }
                return scorer;
            }
            
        }

        private bool DoBracketsMatch(char openBracket, char closeBracket) {
            if (openBracket == '(' && closeBracket == ')') { return true; }
            else if (openBracket == '<' && closeBracket == '>') { return true; }
            else if (openBracket == '{' && closeBracket == '}') { return true; }
            else if (openBracket == '[' && closeBracket == ']') { return true; }
            return false;
        }

        private int GetCorruptionScore(char closeBracket) { 
            switch (closeBracket) {
                case ')': return 3;
                case ']': return 57;
                case '}': return 1197;
                case '>': return 25137;
                default: return 0;
            }
        }

        public int GetIncompletionScore(char openBracket) { 
            switch (openBracket) {
                case '(': return 1;
                case '[': return 2;
                case '{': return 3;
                case '<': return 4;
                default: return 0;
            }        
        }

    }




}
 