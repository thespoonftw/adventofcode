using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

public class Day03 : DayBase {

    protected override string PartOne() {
        var numbers = GetData();
        var gammaString = "";
        var epsilonString = "";
        var binaryLength = numbers[0].Length;
        for (int i=0; i<binaryLength; i++) {
            gammaString += GetMostCommonValue(i, numbers);
            epsilonString += GetLeastCommonValue(i, numbers);
        }
        var gamma = Convert.ToInt32(gammaString, 2);
        var epsilon = Convert.ToInt32(epsilonString, 2);
        return (gamma * epsilon).ToString();
    }

    protected override string PartTwo() {
        var numbers = GetData();
        var oxygenString = GetOxygen(0, numbers);
        var co2String = GetCO2(0, numbers);
        var oxygen = Convert.ToInt32(oxygenString, 2);
        var co2 = Convert.ToInt32(co2String, 2);
        return (oxygen * co2).ToString();
    }

    protected override string Test() {
        var test = new List<string>() {
            "00100",
            "11110",
            "10110",
            "10111",
            "10101",
            "01111",
            "00111",
            "11100",
            "10000",
            "11001",
            "00010",
            "01010",
        };
        var oxygen = GetOxygen(0, test);
        var co2 = GetCO2(0, test);
        return oxygen + " " + co2;
    }

    public string GetOxygen(int currentIndex, List<string> input) {
        var mostCommon = GetMostCommonValue(currentIndex, input);
        var reducedList = input.Where(n => n[currentIndex] == mostCommon).ToList();
        if (reducedList.Count == 1) { return reducedList[0]; }
        else { return GetOxygen(currentIndex + 1, reducedList); }
    }

    public string GetCO2(int currentIndex, List<string> input) {
        var leastCommon = GetLeastCommonValue(currentIndex, input);
        var reducedList = input.Where(n => n[currentIndex] == leastCommon).ToList();
        if (reducedList.Count == 1) { return reducedList[0]; }
        else { return GetCO2(currentIndex + 1, reducedList); }
    }

    public char GetMostCommonValue(int index, List<string> rows) {
        var counter = 0;
        foreach (var row in rows) {
            if (row[index] == '1') { counter++; }
        }
        if (counter == rows.Count / 2f) { return '1'; }
        else { return (counter > rows.Count / 2f) ? '1' : '0'; }
    }

    public char GetLeastCommonValue(int index, List<string> rows) {
        return GetMostCommonValue(index, rows) == '1' ? '1' : '0';
    }
}
 