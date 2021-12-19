using System;
using System.Collections.Generic;
using System.Linq;

public class Day18 : DayBase {

    public class SnailNumber {

        public Pair topPair;
        
        public SnailNumber(string input) {
            int i = 0;
            topPair = (Pair)BuildTreeElement(input, ref i, null);
        }

        public SnailNumber(SnailNumber left, SnailNumber right) {
            topPair = new Pair(null);
            topPair.leftElement = new SnailNumber(left.ToString()).topPair;
            topPair.rightElement = new SnailNumber(right.ToString()).topPair;
            ((Pair)topPair.rightElement).parent = topPair;
            ((Pair)topPair.leftElement).parent = topPair;
            while (true) {
                if (ReduceByExplosions(topPair)) { continue; }
                if (ReduceBySplitting(topPair)) { continue; }
                break;
            }
        }

        public override string ToString() {
            var returner = "";
            AddToString(ref returner, topPair);
            return returner;
        }

        private void AddToString(ref string s, Pair pair) {
            s += "[";
            if (pair.leftElement is Value) { s += ((Value)pair.leftElement).value; }
            else { AddToString(ref s, (Pair)pair.leftElement); }
            s += ",";
            if (pair.rightElement is Value) { s += ((Value)pair.rightElement).value; } 
            else { AddToString(ref s, (Pair)pair.rightElement); }
            s += "]";
        }

        private object BuildTreeElement(string input, ref int indexer, Pair parent) {
            object returner;
            var nextChar = input[indexer];

            if (nextChar == '[') {
                indexer++;
                var pair = new Pair(parent);
                pair.leftElement = BuildTreeElement(input, ref indexer, pair);
                indexer++;
                indexer++;
                pair.rightElement = BuildTreeElement(input, ref indexer, pair);
                returner = pair;
                indexer++;

            } else {
                var endIndex = indexer;
                while (true) {
                    endIndex++;
                    if (input[endIndex] == ']' || input[endIndex] == ',') { break; }
                }
                var sub = input.Substring(indexer, endIndex - indexer);
                var newValue = new Value(int.Parse(sub), parent);
                returner = newValue;
                indexer = endIndex - 1;
            }
            return returner;
        }

        public bool ReduceByExplosions(Pair pair) {
            if (pair.leftElement is Pair) {
                if (TryExplode((Pair)pair.leftElement)) {
                    return true;
                } else if (ReduceByExplosions((Pair)pair.leftElement)) {
                    return true;
                }
            }
            if (pair.rightElement is Pair) {
                if (TryExplode((Pair)pair.rightElement)) {
                    return true;
                } else if (ReduceByExplosions((Pair)pair.rightElement)) {
                    return true;
                }
            }
            return false;
        }

        public bool ReduceBySplitting(Pair pair) {
            if (pair.leftElement is Value) {
                if (TrySplit((Value)pair.leftElement)) {
                    return true;
                }
            } else if (ReduceBySplitting((Pair)pair.leftElement)) {
                return true;
            }
            if (pair.rightElement is Value) {
                if (TrySplit((Value)pair.rightElement)) {
                    return true;
                }
            } else if (ReduceBySplitting((Pair)pair.rightElement)) {
                return true;
            }
            return false;
        }

        private bool TryExplode(Pair pair) {
            if (pair.depth < 4) { return false; }
            Value leftElement = (Value)pair.leftElement;
            Value rightElement = (Value)pair.rightElement;
            var parent = pair.parent;
            var newValue = new Value(0, parent);
            if (pair.isLeft) {
                parent.leftElement = newValue;
            } else {
                parent.rightElement = newValue;
            }
            var leftAdjacent = GetAdjacentValue(newValue, true);
            var rightAdjacent = GetAdjacentValue(newValue, false);
            if (leftAdjacent != null) { leftAdjacent.value += leftElement.value; }
            if (rightAdjacent != null) { rightAdjacent.value += rightElement.value; }
            return true;
        }

        private Value GetAdjacentValue(Value value, bool isSearchingLeft) {
            var listOfValues = new List<Value>();
            AddToListOfValues(topPair, listOfValues);
            var index = listOfValues.IndexOf(value);
            if (index == 0 && isSearchingLeft) { return null; }
            if (index == listOfValues.Count - 1 && !isSearchingLeft) { return null; }
            if (isSearchingLeft) {
                return listOfValues[index - 1];
            } else {
                return listOfValues[index + 1];
            }
        }

        private void AddToListOfValues(Pair pair, List<Value> values) {
            if (pair.leftElement is Pair) {
                AddToListOfValues((Pair)pair.leftElement, values);
            } else {
                values.Add((Value)pair.leftElement);
            }
            if (pair.rightElement is Pair) {
                AddToListOfValues((Pair)pair.rightElement, values);
            } else {
                values.Add((Value)pair.rightElement);
            }
        }

        private bool TrySplit(Value toSplit) {
            if (toSplit.value < 10) { return false; }
            int roundDown = (int)Math.Floor(toSplit.value / 2f);
            int roundUp = (int)Math.Ceiling(toSplit.value / 2f);
            var pair = new Pair(toSplit.parent);
            if (toSplit.isLeft) {
                toSplit.parent.leftElement = pair;
            } else {
                toSplit.parent.rightElement = pair;
            }
            pair.leftElement = new Value(roundDown, pair);
            pair.rightElement = new Value(roundUp, pair);
            return true;
        }

        public int GetMagnitude() => GetMagnitude(topPair);

        private int GetMagnitude(Pair pair) {
            int a = 0;
            int b = 0;
            if (pair.leftElement is Value) {
                a = ((Value)pair.leftElement).value * 3;
            } else {
                a = GetMagnitude((Pair)pair.leftElement) * 3;
            }
            if (pair.rightElement is Value) {
                b = ((Value)pair.rightElement).value * 2;
            } else {
                b = GetMagnitude((Pair)pair.rightElement) * 2;
            }
            return a + b;
        }

        public class Pair {
            public object leftElement;
            public object rightElement;
            public Pair parent;
            public bool isLeft => parent != null && parent.leftElement == this;
            public int depth => parent == null ? 0 : parent.depth + 1;
            public Pair(Pair parent) {
                this.parent = parent;
            }
        }

        public class Value {
            public int value;
            public Pair parent;
            public bool isLeft => parent.leftElement == this;
            public int depth => parent.depth + 1;
            public Value(int value, Pair parent) {
                this.value = value;
                this.parent = parent;
            }
        }
    }    

    protected override long PartOne(List<string> data) {
        var numbers = data.Select(d => new SnailNumber(d)).ToList();
        var current = numbers[0];
        for (int i=1; i<numbers.Count; i++) {
            current = new SnailNumber(current, numbers[i]);
        }
        return current.GetMagnitude();
    }

    protected override long PartTwo(List<string> data) {
        var numbers = data.Select(d => new SnailNumber(d)).ToList();
        var returner = 0;
        foreach (var n in numbers) {
            foreach (var m in numbers) {
                if (n == m) { continue; }
                var a = new SnailNumber(n, m).GetMagnitude();
                var b = new SnailNumber(m, n).GetMagnitude();
                if (a > returner) { returner = a; }
                if (b > returner) { returner = b; }
            }
        }
        return returner;
    }

}
 