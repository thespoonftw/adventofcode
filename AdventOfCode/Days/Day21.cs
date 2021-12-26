using System;
using System.Collections.Generic;
using System.Linq;

public class Day21 : DayBase {

    protected override long PartOne(List<string> data) {
        var dirac = new DiracDice(data);
        return dirac.ResolveGame();
    }

    protected override long PartTwo(List<string> data) {
        var dirac = new QuantumDice(data);
        return dirac.ResolveGame();
    }

    private static int ModPosition(int position) {
        return ((position - 1) % 10) + 1;
    }

    public class DiracDice {

        private Player player1;
        private Player player2;
        private int currentDieIndex = 0;
        private bool isPlayer1Turn = true;
        private Player losingPlayer;

        public DiracDice(List<string> input) {
            player1 = new Player() { position = int.Parse(input[0].Split(' ')[4]) };
            player2 = new Player() { position = int.Parse(input[1].Split(' ')[4]) };
        }

        public int ResolveGame() {
            while (losingPlayer == null) {
                TakeTurn();
            }
            return currentDieIndex * losingPlayer.score;
        }

        public void TakeTurn() {
            var rollResult = GetDieResult() + GetDieResult() + GetDieResult();
            var currentPlayer = isPlayer1Turn ? player1 : player2;
            currentPlayer.position = ModPosition(currentPlayer.position + rollResult);
            currentPlayer.score += currentPlayer.position;
            if (currentPlayer.score >= 1000) {
                losingPlayer = isPlayer1Turn ? player2 : player1;
            }
            isPlayer1Turn = !isPlayer1Turn;
        }

        private int GetDieResult() {
            currentDieIndex++;
            return ((currentDieIndex - 1) % 100) + 1;
        }

        private class Player {
            public int position;
            public int score;
        }
    }

    public class QuantumDice {

        private Dictionary<QuantumKey, long> dict = new Dictionary<QuantumKey, long>();

        private long p1Victories;
        private long p2Victories;
        private bool isPlayer1Turn = true;

        public QuantumDice(List<string> input) {
            int p1Pos = int.Parse(input[0].Split(' ')[4]);
            int p2Pos = int.Parse(input[1].Split(' ')[4]);
            var key = new QuantumKey(0, 0, p1Pos, p2Pos);
            dict.Add(key, 1);
        }

        public long ResolveGame() {
            bool onGoingGames = true;
            while (onGoingGames) {
                onGoingGames = TakeTurn();
            }
            return Math.Max(p1Victories, p2Victories);
        }

        private int[] probabilities = new int[] { 0, 0, 0, 1, 3, 6, 7, 6, 3, 1 };

        // returns true if there are ongoing games still
        private bool TakeTurn() {
            var b = isPlayer1Turn;
            var newDict = new Dictionary<QuantumKey, long>();
            foreach (var pair in dict) {
                for (int i = 3; i <= 9; i++) {
                    var newPos = ModPosition(pair.Key.GetPosition(b) + i);
                    var newScore = pair.Key.GetScore(b) + newPos;
                    if (newScore >= 21) {
                        if (b) { p1Victories += pair.Value * probabilities[i]; }
                        else { p2Victories += pair.Value * probabilities[i]; }
                    } else {
                        var newKey = new QuantumKey(pair.Key, isPlayer1Turn, newPos, newScore);
                        if (!newDict.ContainsKey(newKey)) { newDict.Add(newKey, 0); }
                        newDict[newKey] += pair.Value * probabilities[i];
                    }
                }
            }
            dict = newDict;
            isPlayer1Turn = !isPlayer1Turn;
            //int sum = newDict.Values.ToList().Sum();
            return newDict.Count != 0;
            
        }

        private struct QuantumKey {
            public int p1Score;
            public int p2Score;
            public int p1Position;
            public int p2Position;
            
            public QuantumKey(int p1Score, int p2Score, int p1Position, int p2Position) {
                this.p1Score = p1Score;
                this.p2Score = p2Score;
                this.p1Position = p1Position;
                this.p2Position = p2Position;
            }

            public QuantumKey(QuantumKey prevKey, bool isP1, int position, int score) {
                if (isP1) {
                    p1Position = position;
                    p1Score = score;
                    p2Position = prevKey.p2Position;
                    p2Score = prevKey.p2Score;
                } else {
                    p1Position = prevKey.p1Position;
                    p1Score = prevKey.p1Score;
                    p2Position = position;
                    p2Score = score;
                }
            }

            public int GetScore(bool isP1) {
                return isP1 ? p1Score : p2Score;
            }

            public int GetPosition(bool isP1) {
                return isP1 ? p1Position : p2Position;
            }

            public static bool operator ==(QuantumKey c1, QuantumKey c2) {
                return c1.p1Score == c2.p1Score && c1.p2Score == c2.p2Score && c1.p1Position == c2.p1Position && c1.p2Position == c2.p2Position;
            }

            public static bool operator !=(QuantumKey c1, QuantumKey c2) {
                return c1.p1Score != c2.p1Score || c1.p2Score != c2.p2Score || c1.p1Position != c2.p1Position || c1.p2Position != c2.p2Position;
            }

            public override bool Equals(object o) {
                if (o == null) { return false; }
                var second = (QuantumKey)o;
                return this == second;
            }

            public override int GetHashCode() {
                return HashCode.Combine(p1Position, p2Position, p1Score, p2Score);
            }
        }


    }

}
 