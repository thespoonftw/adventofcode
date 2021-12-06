using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

public abstract class DayBase {

    private string ProjectDirectory => Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;

    public DayBase() {
        var name = GetType().Name;
        Console.WriteLine(name + " Test One: " + PartOne(GetTestData()));
        Console.WriteLine(name + " Part One: " + PartOne(GetData()));
        Console.WriteLine(name + " Test Two: " + PartTwo(GetTestData()));
        Console.WriteLine(name + " Part Two: " + PartTwo(GetData()));
        
    }

    private List<string> GetData() {
        return System.IO.File.ReadAllLines(ProjectDirectory + @"\Inputs\" + GetType().Name + ".txt").ToList();
    }

    private List<string> GetTestData() {
        return System.IO.File.ReadAllLines(ProjectDirectory + @"\TestData\" + GetType().Name + ".txt").ToList();
    }

    protected abstract long PartOne(List<string> data);
    protected abstract long PartTwo(List<string> data);
}
 