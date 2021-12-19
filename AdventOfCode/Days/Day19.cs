using System;
using System.Collections.Generic;
using System.Linq;

public class Day19 : DayBase {

    public struct Coordinate {
        public int x;
        public int y;
        public int z;

        public Coordinate(string s) {
            var split = s.Split(',');
            x = int.Parse(split[0]);
            y = int.Parse(split[1]);
            z = int.Parse(split[2]);
        }

        public Coordinate(int x, int y, int z) {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Coordinate Rotate(int rotationIndex) {
            switch (rotationIndex) {
                default: return new Coordinate(x, y, z);
                case 1: return new Coordinate(x, -y, -z);
                case 2: return new Coordinate(-x, y, -z);
                case 3: return new Coordinate(-x, -y, z);

                case 4: return new Coordinate(y, z, x);
                case 5: return new Coordinate(y, -z, -x);
                case 6: return new Coordinate(-y, z, -x);
                case 7: return new Coordinate(-y, -z, x);

                case 8: return new Coordinate(z, x, y);
                case 9: return new Coordinate(z, -x, -y);
                case 10: return new Coordinate(-z, x, -y);
                case 11: return new Coordinate(-z, -x, y);

                case 12: return new Coordinate(x, z, -y);
                case 13: return new Coordinate(x, -z, y);
                case 14: return new Coordinate(-x, z, y);
                case 15: return new Coordinate(-x, -z, -y);

                case 16: return new Coordinate(z, y, -x);
                case 17: return new Coordinate(z, -y, x);
                case 18: return new Coordinate(-z, y, x);
                case 19: return new Coordinate(-z, -y, -x);

                case 20: return new Coordinate(y, x, -z);
                case 21: return new Coordinate(y, -x, z);
                case 22: return new Coordinate(-y, x, z);
                case 23: return new Coordinate(-y, -x, -z);
            }
        }

        public static bool operator ==(Coordinate c1, Coordinate c2) {
            return c1.x == c2.x && c1.y == c2.y && c1.z == c2.z;
        }

        public static bool operator !=(Coordinate c1, Coordinate c2) {
            return c1.x != c2.x || c1.y != c2.y || c1.z != c2.z;
        }

        public override bool Equals(object o) {
            if (o == null) { return false; }
            var second = (Coordinate)o;
            return this == second;
        }

        public override int GetHashCode() {
            return HashCode.Combine(x, y, z);
        }

        public static Coordinate operator -(Coordinate c1, Coordinate c2) {
            return new Coordinate(c1.x - c2.x, c1.y - c2.y, c1.z - c2.z);
        }

        public static Coordinate operator +(Coordinate c1, Coordinate c2) {
            return new Coordinate(c1.x + c2.x, c1.y + c2.y, c1.z + c2.z);
        }
    }

    public class Scanner {
        public List<Coordinate> scannedBeacons = new List<Coordinate>();
        public Coordinate positionRelativeToPrevious;
        public Coordinate positionRelativeToZero;
        public int orientationRelativeToPrevious;
        public Scanner previousScanner;
    }

    public class System {

        public List<Scanner> scanners = new List<Scanner>();
        public HashSet<Coordinate> beacons = new HashSet<Coordinate>();

        public System(List<string> data) {
            Scanner currentScanner = new Scanner();
            for (int i = 0; i < data.Count; i++) {
                var line = data[i];
                if (line.Length == 0) {
                    scanners.Add(currentScanner);
                    currentScanner = new Scanner();
                    continue;
                }
                if (line.Substring(0, 3) == "---") { continue; }
                currentScanner.scannedBeacons.Add(new Coordinate(line));
            }
            scanners.Add(currentScanner);

            MatchScanners();
            Solve();
        }

        public void MatchScanners() {
            var unfoundScanners = new List<Scanner>(scanners);
            unfoundScanners.Remove(scanners[0]);
            var scannersToCheck = new List<Scanner>();
            scannersToCheck.Add(scanners[0]);

            while (unfoundScanners.Count > 0) {
                var scanFrom = scannersToCheck[0];
                foreach (var otherScanner in unfoundScanners) {
                    for (int i = 0; i < 24; i++) {
                        var distBin = new Dictionary<Coordinate, int>();
                        foreach (var p1 in scanFrom.scannedBeacons) {
                            foreach (var p2 in otherScanner.scannedBeacons) {
                                var rot = p2.Rotate(i);
                                var relative = p1 - rot;
                                if (!distBin.ContainsKey(relative)) { distBin.Add(relative, 0); }
                                distBin[relative]++;
                            }
                        }
                        int highestOccurance = 0;
                        Coordinate commonDistance = new Coordinate();
                        foreach (var c in distBin) {
                            if (c.Value > highestOccurance) {
                                highestOccurance = c.Value;
                                commonDistance = c.Key;
                            }
                        }
                        if (highestOccurance >= 12) {
                            otherScanner.previousScanner = scanFrom;
                            otherScanner.positionRelativeToPrevious = commonDistance;
                            otherScanner.orientationRelativeToPrevious = i;
                            scannersToCheck.Add(otherScanner);
                            break; // no need to check other orientations
                        }
                    }
                }
                unfoundScanners = unfoundScanners.Except(scannersToCheck).ToList();
                scannersToCheck.Remove(scanFrom);
            }
        }

        private void Solve() {
            foreach (var scanner in scanners) {

                List<int> rotations = new List<int>() { scanner.orientationRelativeToPrevious };
                List<Coordinate> offsets = new List<Coordinate>() { scanner.positionRelativeToPrevious };
                var prev = scanner.previousScanner;
                while (prev != null) {
                    rotations.Add(prev.orientationRelativeToPrevious);
                    offsets.Add(prev.positionRelativeToPrevious);
                    prev = prev.previousScanner;
                }

                var scannerPos = new Coordinate();
                for (int i = 0; i < rotations.Count; i++) {
                    scannerPos = scannerPos.Rotate(rotations[i]);
                    scannerPos += offsets[i];
                }
                scanner.positionRelativeToZero = scannerPos;

                foreach (var b in scanner.scannedBeacons) {

                    var coord = b;
                    for (int i=0; i<rotations.Count; i++) {
                        coord = coord.Rotate(rotations[i]);
                        coord += offsets[i];
                    }
                    beacons.Add(coord);
                }
            }
        }

        public int ManhattenDistance() {
            int highestValue = 0;
            foreach (var a in scanners) {
                foreach (var b in scanners) {
                    var A = a.positionRelativeToZero;
                    var B = b.positionRelativeToZero;
                    var distance = (B.x - A.x) + (B.y - A.y) + (B.z - A.z);
                    if (distance > highestValue) { highestValue = distance; }
                }
            }
            return highestValue;
        }
    }

    protected override long PartOne(List<string> data) {
        var system = new System(data);
        return system.beacons.Count();
    }

    protected override long PartTwo(List<string> data) {
        var system = new System(data);
        return system.ManhattenDistance();
    }

}
 