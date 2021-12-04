using System;
using System.Collections.Generic;
using System.Linq;

public class Day01 : DayBase {

    protected override string PartOne(List<string> data) {
        var numbers = data.Select(n => float.Parse(n)).ToList();
        var count = 0;
        for (int i = 0; i < (numbers.Count - 1); i++) {
            if (numbers[i] < numbers[i + 1]) { count++; }
        }
        return count.ToString();
    }

    protected override string PartTwo(List<string> data) {
        var numbers = data.Select(n => float.Parse(n)).ToList();
        var count = 0;
        for (int i = 0; i < (numbers.Count - 3); i++) {
            if (numbers[i] < numbers[i + 3]) { count++; }
        }
        return count.ToString();
    }

}
