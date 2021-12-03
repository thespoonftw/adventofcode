using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

public class Day01 : DayBase {

    protected override string PartOne() {
        var numbers = GetData().Select(n => float.Parse(n)).ToList();
        var count = 0;
        for (int i = 0; i < (numbers.Count - 1); i++) {
            if (numbers[i] < numbers[i + 1]) { count++; }
        }
        return count.ToString();
    }

    protected override string PartTwo() {
        var numbers = GetData().Select(n => float.Parse(n)).ToList();
        var count = 0;
        for (int i = 0; i < (numbers.Count - 3); i++) {
            if (numbers[i] < numbers[i + 3]) { count++; }
        }
        return count.ToString();
    }

}
