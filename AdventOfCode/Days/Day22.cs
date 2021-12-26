using System;
using System.Collections.Generic;
using System.Linq;

public class Day22 : DayBase {

    protected override long PartOne(List<string> data) {
        var reactor = new Reactor();
        var instructions = LoadInstructions(data, true);
        instructions.ForEach(i => reactor.AddInstruction(i));
        return reactor.CountLights();
    }

    protected override long PartTwo(List<string> data) {
        var reactor = new Reactor();
        var instructions = LoadInstructions(data, false);
        instructions.ForEach(i => reactor.AddInstruction(i));
        return reactor.CountLights();
    }

    private List<Instruction> LoadInstructions(List<string> data, bool smallOnly) {
        var returner = new List<Instruction>();
        foreach (var i in data) {
            var split = i.Split(new char[] { ' ', '=', '.', ',' }, StringSplitOptions.RemoveEmptyEntries);
            returner.Add(new Instruction(split[0] == "on", int.Parse(split[2]), int.Parse(split[3]), int.Parse(split[5]), int.Parse(split[6]), int.Parse(split[8]), int.Parse(split[9])));
        }
        if (smallOnly) {
            var limit = 51;
            returner.RemoveAll(i =>
                i.xStart < -limit ||
                i.xEnd > limit ||
                i.yStart < -limit ||
                i.yEnd > limit ||
                i.zStart < -limit ||
                i.zEnd > limit
            );
        }
        return returner;
    }

    private class ReactorTest {

        private bool[,,] array = new bool[101, 101, 101];

        public void AddInstruction(Instruction i) {
            for (long x = i.xStart; x < i.xEnd; x++) {
                for (long y = i.yStart; y < i.yEnd; y++) {
                    for (long z = i.zStart; z < i.zEnd; z++) {
                        array[x + 50, y + 50, z + 50] = i.isOn;
                    }
                }
            }
        }

        public long CountLights() {
            long counter = 0;
            for (long x = 0; x < 101; x++) {
                for (long y = 0; y < 101; y++) {
                    for (long z = 0; z < 101; z++) {
                        if (array[x,y,z]) { counter++; }
                    }
                }
            }
            return counter;
        }
    }

    private class Reactor {

        private List<OnCube> onCubes = new List<OnCube>();

        public void AddInstruction(Instruction i) {
            var cubesToAdd = new List<OnCube>();
            var cubesToRem = new List<OnCube>();
            foreach (var c in onCubes) {
                if (InstructionContainsCube(c, i)) {
                    cubesToRem.Add(c);
                } else if (IsCollision(c, i)) {
                    cubesToRem.Add(c);
                    cubesToAdd.AddRange(SliceCube(c, i));
                }
            }
            if (i.isOn) { cubesToAdd.Add(new OnCube(i.xStart, i.xEnd, i.yStart, i.yEnd, i.zStart, i.zEnd)); }
            onCubes.AddRange(cubesToAdd);
            onCubes = onCubes.Except(cubesToRem).ToList();
        }

        public long CountLights() {
            long counter = 0;
            foreach (var c in onCubes) {
                counter += c.Size;
            }
            return counter;
        }

        private bool InstructionContainsCube(OnCube cube, Instruction ins) {
            return ins.xStart <= cube.xStart && ins.xEnd >= cube.xEnd &&
                ins.yStart <= cube.yStart && ins.yEnd >= cube.yEnd &&
                ins.zStart <= cube.zStart && ins.zEnd >= cube.zEnd;
        }

        private bool IsCollision(OnCube cube, Instruction ins) {
            bool isX = false;
            bool isY = false;
            bool isZ = false;
            // check to see if either xEnd is contained inside the other
            if (ins.xEnd > cube.xStart && ins.xEnd <= cube.xEnd) { isX = true; } 
            if (cube.xEnd > ins.xStart && cube.xEnd <= ins.xEnd) { isX = true; }
            if (ins.yEnd > cube.yStart && ins.yEnd <= cube.yEnd) { isY = true; }
            if (cube.yEnd > ins.yStart && cube.yEnd <= ins.yEnd) { isY = true; }
            if (ins.zEnd > cube.zStart && ins.zEnd <= cube.zEnd) { isZ = true; }
            if (cube.zEnd > ins.zStart && cube.zEnd <= ins.zEnd) { isZ = true; }
            return (isX && isY && isZ);
        }

        private List<OnCube> SliceCube(OnCube cube, Instruction ins) {
            var slicer = new CubeSlicer(cube, ins);

            // up to 27 new cubes could be created, centre cube left empty
            for (int i = 0; i < 3; i++) {
                for (int j = 0; j < 3; j++) {
                    for (int k = 0; k < 3; k++) {
                        if (i == 1 && j == 1 && k == 1) { continue; }
                        slicer.TryAdd((Slice)i, (Slice)j, (Slice)k);
                    }
                }
            }
            return slicer.cubes;
        }
    }

    public class OnCube {
        public long xStart;
        public long xEnd;
        public long yStart;
        public long yEnd;
        public long zStart;
        public long zEnd;
        public long Size => (xEnd - xStart) * (yEnd - yStart) * (zEnd - zStart);

        public OnCube(long xStart, long xEnd, long yStart, long yEnd, long zStart, long zEnd) {
            this.xStart = xStart;
            this.xEnd = xEnd;
            this.yStart = yStart;
            this.yEnd = yEnd;
            this.zStart = zStart;
            this.zEnd = zEnd;
        }
    }

    public class Instruction {

        public bool isOn;
        public long xStart;
        public long xEnd;
        public long yStart;
        public long yEnd;
        public long zStart;
        public long zEnd;

        public Instruction(bool isOn, long xStart, long xEnd, long yStart, long yEnd, long zStart, long zEnd) {
            this.isOn = isOn;
            this.xStart = xStart;
            this.xEnd = xEnd + 1;
            this.yStart = yStart;
            this.yEnd = yEnd + 1;
            this.zStart = zStart;
            this.zEnd = zEnd + 1;
        }
    }

    public class CubeSlicer {

        private readonly OnCube cube;
        private readonly Instruction ins;

        public List<OnCube> cubes = new List<OnCube>();

        public CubeSlicer(OnCube cube, Instruction ins) {
            this.cube = cube;
            this.ins = ins;
        }

        public void TryAdd(Slice x, Slice y, Slice z) {
            if (x == Slice.below && ins.xStart <= cube.xStart) { return; }
            if (x == Slice.above && ins.xEnd >= cube.xEnd) { return; }
            if (y == Slice.below && ins.yStart <= cube.yStart) { return; }
            if (y == Slice.above && ins.yEnd >= cube.yEnd) { return; }
            if (z == Slice.below && ins.zStart <= cube.zStart) { return; }
            if (z == Slice.above && ins.zEnd >= cube.zEnd) { return; }
            var lowerX = Math.Max(cube.xStart, ins.xStart);
            var higherX = Math.Min(cube.xEnd, ins.xEnd);
            var lowerY = Math.Max(cube.yStart, ins.yStart);
            var higherY = Math.Min(cube.yEnd, ins.yEnd);
            var lowerZ = Math.Max(cube.zStart, ins.zStart);
            var higherZ = Math.Min(cube.zEnd, ins.zEnd);
            cubes.Add(new OnCube(
                x == Slice.below ? cube.xStart : x == Slice.middle ? lowerX : higherX,
                x == Slice.below ? lowerX : x == Slice.middle ? higherX : cube.xEnd,
                y == Slice.below ? cube.yStart : y == Slice.middle ? lowerY : higherY,
                y == Slice.below ? lowerY : y == Slice.middle ? higherY : cube.yEnd,
                z == Slice.below ? cube.zStart : z == Slice.middle ? lowerZ : higherZ,
                z == Slice.below ? lowerZ : z == Slice.middle ? higherZ : cube.zEnd
            ));
        }
    }

    public enum Slice {
        below,
        middle,
        above,
    }

}
 