using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

public abstract class DayBase {

    public DayBase() {
        var name = GetType().Name;
        Console.WriteLine(name + " Part One: " + PartOne());
        Console.WriteLine(name + " Part Two: " + PartTwo());
        Console.WriteLine(name + " Test: " + Test());
    }

    protected List<string> GetData() {
        return System.IO.File.ReadAllLines(@"C:\Users\mikew\Desktop\AdventOfCode\AdventOfCode\AdventOfCode\Inputs\" + GetType().Name + ".txt").ToList();
    }

    protected abstract string PartOne();
    protected abstract string PartTwo();
    protected virtual string Test() { return ""; }
}
 