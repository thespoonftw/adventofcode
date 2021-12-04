using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

public abstract class DayBase {

    public DayBase() {
        var name = GetType().Name;
        Console.WriteLine(name + " Test One: " + PartOne(GetTestData()));
        Console.WriteLine(name + " Part One: " + PartOne(GetData()));
        Console.WriteLine(name + " Test Two: " + PartTwo(GetTestData()));
        Console.WriteLine(name + " Part Two: " + PartTwo(GetData()));
        
    }

    private List<string> GetData() {
        return System.IO.File.ReadAllLines(@"C:\Users\mikew\Desktop\AdventOfCode\AdventOfCode\Inputs\" + GetType().Name + ".txt").ToList();
    }

    private List<string> GetTestData() {
        return System.IO.File.ReadAllLines(@"C:\Users\mikew\Desktop\AdventOfCode\AdventOfCode\TestData\" + GetType().Name + ".txt").ToList();
    }

    protected abstract string PartOne(List<string> data);
    protected abstract string PartTwo(List<string> data);
}
 