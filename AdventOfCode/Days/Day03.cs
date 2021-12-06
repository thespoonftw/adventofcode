using System;
using System.Collections.Generic;
using System.Linq;

public class Day03 : DayBase {

    protected override long PartOne(List<string> data) {
        var gammaString = "";
        var epsilonString = "";
        var binaryLength = data[0].Length;
        for (int i=0; i<binaryLength; i++) {
            gammaString += GetMostCommonValue(i, data);
            epsilonString += GetLeastCommonValue(i, data);
        }
        var gamma = Convert.ToInt32(gammaString, 2);
        var epsilon = Convert.ToInt32(epsilonString, 2);
        return (gamma * epsilon);
    }

    protected override long PartTwo(List<string> data) {
        var oxygenString = GetOxygen(0, data);
        var co2String = GetCO2(0, data);
        var oxygen = Convert.ToInt32(oxygenString, 2);
        var co2 = Convert.ToInt32(co2String, 2);
        return (oxygen * co2);
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
        return GetMostCommonValue(index, rows) == '1' ? '0' : '1';
    }
}
 