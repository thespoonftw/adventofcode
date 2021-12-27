using System;
using System.Collections.Generic;
using System.Linq;

public class Day23 : DayBase {

    protected override long PartOne(List<string> data) {
        var initial = new Level(data, true);
        var solution = Solve(initial);
        return solution.cost;
    }

    protected override long PartTwo(List<string> data) {
        var initial = new Level(data, false);
        var solution = Solve(initial);
        return solution.cost;
    }

    public Level Solve(Level input) {
        HashSet<Level> existingLevels = new HashSet<Level>() { input };
        Level bestSolution = new Level(100000); // discard any solution which is worse than this
        while (existingLevels.Count > 0) {
            var newLevels = new HashSet<Level>();
            foreach (var p in existingLevels) {
                foreach (var l in p.GetPossibleLevels()) {
                    MergeInto(newLevels, l);
                }
            }
            var keptLevels = new HashSet<Level>();
            foreach (var l in newLevels) {
                if (l.IsComplete()) {
                    if (l.cost < bestSolution.cost) { bestSolution = l; }
                } else if (l.cost < bestSolution.cost) {
                    keptLevels.Add(l);
                }
            }
            existingLevels = keptLevels;
        }
        return bestSolution;
    }

    public static void MergeInto(HashSet<Level> hashset, Level level) {
        if (hashset.TryGetValue(level, out Level actualValue) && actualValue.cost > level.cost) {
            hashset.Remove(actualValue);
        }
        hashset.Add(level);
    }

    public class Level {

        public int cost;
        public int roomHeight;
        public State[] corridor;
        public State[,] room;
        public (State[], State[,]) Values => (corridor, room);
        public readonly int[] costs = { 1, 10, 100, 1000 };
        public List<string> log = new List<string>();

        public static bool operator ==(Level a, Level b) => a.GetHashCode() == b.GetHashCode();
        public static bool operator !=(Level a, Level b) => a.GetHashCode() != b.GetHashCode();
        public override bool Equals(object obj) => this == (Level)obj;

        public override int GetHashCode() {
            var hash = new HashCode();
            for (int i=0; i<7; i++) { hash.Add(corridor[i]); }
            for (int x=0; x<4; x++) { 
                for (int y=0; y<roomHeight; y++) {
                    hash.Add(room[x, y]);
                }
            }
            return hash.ToHashCode();
        }

        public Level(List<string> data, bool isPartOne) {
            corridor = new State[7];
            for (int i = 0; i < 7; i++) { corridor[i] = State._; }
            roomHeight = isPartOne ? 2 : 4;
            room = new State[4, roomHeight];
            var secondRow = isPartOne ? 1 : 3;
            room[0, 0] = CharToState(data[2][3]);
            room[0, secondRow] = CharToState(data[3][3]);
            room[1, 0] = CharToState(data[2][5]);
            room[1, secondRow] = CharToState(data[3][5]);
            room[2, 0] = CharToState(data[2][7]);
            room[2, secondRow] = CharToState(data[3][7]);
            room[3, 0] = CharToState(data[2][9]);
            room[3, secondRow] = CharToState(data[3][9]);
            if (!isPartOne) {
                room[0, 1] = State.D;
                room[0, 2] = State.D;
                room[1, 1] = State.C;
                room[1, 2] = State.B;
                room[2, 1] = State.B;
                room[2, 2] = State.A;
                room[3, 1] = State.A;
                room[3, 2] = State.C;
            }
        }

        private State CharToState(char i) {
            switch (i) {
                default: return State.A;
                case 'B': return State.B;
                case 'C': return State.C;
                case 'D': return State.D;
            }
        }

        public Level(Level toClone) {
            cost = toClone.cost;
            roomHeight = toClone.roomHeight;
            corridor = (State[])toClone.corridor.Clone();
            room = (State[,])toClone.room.Clone();
            log = new List<string>(toClone.log);
        }

        public Level(int cost) {
            this.cost = cost;
        }

        public override string ToString() {
            var returner = "[" + string.Join(",", corridor) + "] ";
            for (int x = 0; x < 4; x++) {
                returner += "[";
                for (int y = 0; y < roomHeight; y++) {
                    returner += room[x, y];
                }
                returner += "]";
            }
            returner += cost;
            return returner;
        }

        // check if those waiting in the corridor can enter their desired room. Returns: (corridorIndex, roomIndex)
        public List<(int, int)> GetCorridorsToMove() {
            var returner = new List<(int, int)>();
            for (int corridorIndex=0; corridorIndex<7; corridorIndex++) {
                var state = corridor[corridorIndex];
                if (state == State._) { continue; }
                int roomIndex = (int)state;

                if (room[roomIndex, 0] != State._) { continue; } // top element must be empty
                bool checker = true;
                for (int y = 1; y < roomHeight; y++) {
                    if ((int)room[roomIndex, y] != roomIndex && room[roomIndex, y] != State._) { checker = false; break; }
                }
                if (!checker) { continue; } // lower elements must be correct or empty

                if (IsCorridorClear(corridorIndex, roomIndex)) { returner.Add((corridorIndex, roomIndex)); }
            }
            return returner;
        }

        // check if those waiting in rooms can go to a corridor. Returns: (corridorIndex, roomIndex)
        public List<(int, int)> GetRoomsToMove() {
            var returner = new List<(int, int)>();
            for (int roomIndex = 0; roomIndex < 4; roomIndex++) {

                if (room[roomIndex, roomHeight - 1] == State._) { continue; } // bottom element must not be empty 
                bool inCorrect = false; // column must contain some incorrect non-empty elements
                for (int y = 0; y < roomHeight; y++) {
                    if (room[roomIndex, y] != State._ && (int)room[roomIndex, y] != roomIndex) { inCorrect = true; }
                }
                if (!inCorrect) { continue; }

                for (int i = 0; i < 7; i++) {
                    if (corridor[i] == State._ && IsCorridorClear(i, roomIndex)) {
                        returner.Add((i, roomIndex));
                    }
                }
            }
            return returner;
        }

        // used in both directions
        private bool IsCorridorClear(int corridorIndex, int roomIndex) {
            float roomPosition = roomIndex + 1.5f;
            if (roomPosition > corridorIndex) {
                for (int i = corridorIndex + 1; i < roomPosition; i++) {
                    if (corridor[i] != State._) { return false; }
                }
            } else {
                for (int i = corridorIndex - 1; i > roomPosition; i--) {
                    if (corridor[i] != State._) { return false; }
                }
            }
            return true;
        }

        private int GetDistanceRoomToCorridor(int corridorIndex, int roomIndex, bool isRoomToCorridor) {
            float roomPosition = roomIndex + 1.5f;
            int counter = ((int)(MathF.Abs(corridorIndex - roomPosition) * 2) + 1);
            if (corridorIndex == 0 || corridorIndex == 6) { counter--; }
            var start = isRoomToCorridor ? 0 : 1;
            var end = isRoomToCorridor ? roomHeight - 1 : roomHeight;
            for (int i = start; i < end; i++) {
                if (room[roomIndex, i] == State._) counter++;
            }
            return counter;
        }

        public Level GetLevelAfterMoving(int corridorIndex, int roomIndex, bool isRoomToCorridor) {
            var returner = new Level(this);
            int rowNumber = 0;
            if (isRoomToCorridor) {
                for (int y = 0; y < roomHeight; y++) {
                    if (room[roomIndex, y] != State._) { // get first non-empty row
                        rowNumber = y;
                        break;
                    }
                }
            } else {
                for (int y = roomHeight - 1; y >= 0; y--) {
                    if (room[roomIndex, y] == State._) { // get last empty row
                        rowNumber = y;
                        break;
                    }
                }
            }
            var moverType = isRoomToCorridor ? room[roomIndex, rowNumber] : corridor[corridorIndex];
            int distance = GetDistanceRoomToCorridor(corridorIndex, roomIndex, isRoomToCorridor);
            int cost = distance * costs[(int)moverType];            
            returner.cost += cost;
            returner.corridor[corridorIndex] = isRoomToCorridor ? moverType : State._;
            returner.room[roomIndex, rowNumber] = isRoomToCorridor ? State._ : moverType;
            returner.log.Add(returner.ToString());
            return returner;
        }
        
        public HashSet<Level> GetPossibleLevels() {
            var returner = new HashSet<Level>();
            var rmovers = GetRoomsToMove();
            foreach (var r in rmovers) {
                MergeInto(returner, GetLevelAfterMoving(r.Item1, r.Item2, true));
            }
            var cmovers = GetCorridorsToMove();
            foreach (var c in cmovers) {
                MergeInto(returner, GetLevelAfterMoving(c.Item1, c.Item2, false));
            }            
            return returner;
        }

        public bool IsComplete() {
            for (int x = 0; x < 4; x++) {
                for (int y = 0; y < roomHeight; y++) {
                    if ((int)room[x, y] != x) { return false; }
                }
            }
            return true;
        }

    }

    public enum State {
        A,
        B,
        C,
        D,
        _,
    }
}
 