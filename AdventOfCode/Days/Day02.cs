using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

public class Day02 : DayBase {

    protected override string PartOne() {
        var instructions = GetData().Select(s => new Instruction(s)).ToList();
        var horizontal = 0;
        var vertical = 0;
        foreach (var i in instructions) {
            switch (i.type) {
                case InstructionType.down: vertical -= i.value; break;
                case InstructionType.up: vertical += i.value; break;
                case InstructionType.forward: horizontal += i.value; break;
            }
        }
        return (Math.Abs(horizontal * vertical)).ToString();
    }

    protected override string PartTwo() {
        var instructions = GetData().Select(s => new Instruction(s)).ToList();
        var horizontal = 0;
        var depth = 0;
        var aim = 0;
        foreach (var i in instructions) {
            switch (i.type) {
                case InstructionType.down: aim += i.value; break;
                case InstructionType.up: aim -= i.value; break;
                case InstructionType.forward: horizontal += i.value; depth += aim * i.value; break;
            }
        }
        return (horizontal * depth).ToString();
    }

    enum InstructionType {
        forward,
        down,
        up,
    }

    class Instruction {
        public InstructionType type;
        public int value;

        public Instruction(string input) {
            var split = input.Split(' ');
            type = Enum.Parse<InstructionType>(split[0]);
            value = int.Parse(split[1]);
        }
    }

}
