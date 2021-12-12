using System;
using System.Collections.Generic;
using System.Linq;

public class Day12 : DayBase {

    protected override long PartOne(List<string> data) {
        var network = new CaveNetwork(data);
        return network.GetNumberOfPaths(true);
    }

    protected override long PartTwo(List<string> data) {
        var network = new CaveNetwork(data);
        return network.GetNumberOfPaths(false);
    }

    class CaveNetwork {

        List<Cave> caves = new List<Cave>();

        public CaveNetwork(List<string> data) {
            foreach (var entry in data) {
                var split = entry.Split('-');
                if (!caves.Any(c => c.name == split[0])) { caves.Add(new Cave(split[0])); }
                if (!caves.Any(c => c.name == split[1])) { caves.Add(new Cave(split[1])); }
                var cave1 = caves.Where(c => c.name == split[0]).First();
                var cave2 = caves.Where(c => c.name == split[1]).First();
                cave1.connections.Add(cave2);
                cave2.connections.Add(cave1);
            }
        }

        public int GetNumberOfPaths(bool isPartOne) {
            var start = caves.Where(c => c.type == CaveType.start).First();
            var onGoingPaths = new List<Path>();
            var finishedPaths = new List<Path>();
            onGoingPaths.Add(new Path(start));
            
            while (onGoingPaths.Count > 0) {
                var newPaths = new List<Path>();

                foreach (var p in onGoingPaths) {

                    var connections = p.GettNextConnections();
                    foreach (var c in connections) {

                        if (c.type == CaveType.end) {
                            finishedPaths.Add(new Path(p, c));

                        } else if (c.type == CaveType.large) {
                            newPaths.Add(new Path(p, c));

                        } else if (c.type == CaveType.small && p.CanVisitSmallCave(c, isPartOne)) {
                            newPaths.Add(new Path(p, c));
                        }
                    }
                }
                onGoingPaths = newPaths;
            }
            return finishedPaths.Count;
        }

        class Cave {

            public string name;
            public CaveType type;
            public List<Cave> connections = new List<Cave>();

            public Cave(string caveName) {
                name = caveName;
                if (caveName == "start") {
                    type = CaveType.start;
                } else if (caveName == "end") {
                    type = CaveType.end;
                } else if (caveName.ToCharArray().ToList().All(c => char.IsUpper(c))) {
                    type = CaveType.large;
                } else {
                    type = CaveType.small;
                }
            }
        }

        enum CaveType { 
            small,
            large,
            start,
            end,
        }

        class Path {

            private List<Cave> traversedCaves = new List<Cave>();
            private bool hasDoubleVisitedSmallCave = false;

            public Path(Cave startCave) {
                traversedCaves.Add(startCave);
            }

            public Path(Path existingPath, Cave nextConnection) {
                hasDoubleVisitedSmallCave = existingPath.hasDoubleVisitedSmallCave;
                traversedCaves.AddRange(existingPath.traversedCaves);
                traversedCaves.Add(nextConnection);

                if (nextConnection.type == CaveType.small && existingPath.traversedCaves.Contains(nextConnection)) {
                    hasDoubleVisitedSmallCave = true; 
                }
            }

            public List<Cave> GettNextConnections() {
                return traversedCaves.Last().connections;
            }

            public bool CanVisitSmallCave(Cave c, bool isPartOne) {
                if (isPartOne) {
                    return !traversedCaves.Contains(c);
                } else {
                    return !traversedCaves.Contains(c) || !hasDoubleVisitedSmallCave;
                }
                
            }

        }

    }

    

}
 