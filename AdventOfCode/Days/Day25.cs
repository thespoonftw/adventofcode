using System;
using System.Collections.Generic;
using System.Linq;

public class Day25 : DayBase {

    protected override long PartOne(List<string> data) {
        var seafloor = new Seafloor(data);
        int counter = 1;
        while (seafloor.TakeStep()) {
            counter++;
        }
        return counter;
    }

    protected override long PartTwo(List<string> data) {
        return 0;
    }

    public class Seafloor {

        public Cucumber[,] cucumbers;
        public readonly int width;
        public readonly int height;
        private readonly List<Cucumber> rightFacingCucumbers = new List<Cucumber>();
        private readonly List<Cucumber> downFacingCucumbers = new List<Cucumber>();

        public Seafloor(List<string> data) {
            height = data.Count;
            width = data[0].Length;
            cucumbers = new Cucumber[width, height];

            for (int x=0; x<width; x++) {
                for (int y=0; y<height; y++) {
                    if (data[y][x] == '>') {
                        var c = new Cucumber(true, x, y, this);
                        cucumbers[x, y] = c;
                        rightFacingCucumbers.Add(c);
                    } else if (data[y][x] == 'v') {
                        var c = new Cucumber(false, x, y, this);
                        cucumbers[x, y] = c;
                        downFacingCucumbers.Add(c);
                    }
                }
            }
        }

        public bool TakeStep() {
            foreach (var c in rightFacingCucumbers) {
                c.CheckMoveForward();
            }

            var toMove = new List<Cucumber>();
            foreach (var c in rightFacingCucumbers) {
                if (c.CheckMoveForward()) {
                    toMove.Add(c);
                }
            }
            toMove.ForEach(c => c.Move());

            var toMove2 = new List<Cucumber>();
            foreach (var c in downFacingCucumbers) {
                if (c.CheckMoveForward()) {
                    toMove2.Add(c);
                }
            }
            toMove2.ForEach(c => c.Move());

            return toMove.Count() > 0 || toMove2.Count() > 0;
        }


        public class Cucumber {

            public readonly bool isRightFacing;
            public int x;
            public int y;
            private Seafloor seafloor;

            public Cucumber(bool isRightFacing, int x, int y, Seafloor seafloor) {
                this.isRightFacing = isRightFacing;
                this.x = x;
                this.y = y;
                this.seafloor = seafloor;
            }

            public bool CheckMoveForward() {
                if (isRightFacing) {
                    var newX = (x + 1) % seafloor.width;
                    return seafloor.cucumbers[newX, y] == null;
                } else {
                    var newY = (y + 1) % seafloor.height;
                    return seafloor.cucumbers[x, newY] == null;
                }
            }

            public void Move() {
                if (isRightFacing) {
                    var newX = (x + 1) % seafloor.width;
                    seafloor.cucumbers[newX, y] = this;
                    seafloor.cucumbers[x, y] = null;
                    x = newX;
                } else {
                    var newY = (y + 1) % seafloor.height;
                    seafloor.cucumbers[x, newY] = this;
                    seafloor.cucumbers[x, y] = null;
                    y = newY;
                }
            }
            
        }

    }

}
 