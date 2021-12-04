using System;
using System.Collections.Generic;
using System.Linq;

public class Day04 : DayBase {

    protected override string PartOne(List<string> data) {
        var system = new BingoSystem(data);
        List<BingoBoard> victoriousBoard = null;
        int lastCalled = 0;
        foreach (var n in system.callingNumbers) {
            lastCalled = n;
            victoriousBoard = system.CallNumber(n);
            if (victoriousBoard.Count > 0) { break; }
        }
        return (victoriousBoard[0].SumOfUnmarked() * lastCalled).ToString();
    }

    protected override string PartTwo(List<string> data) {
        var system = new BingoSystem(data);
        int lastCalled = 0;
        BingoBoard finalBoard = null;
        foreach (var n in system.callingNumbers) {
            lastCalled = n;
            var victoriousBoards = system.CallNumber(n);
            system.boards = system.boards.Except(victoriousBoards).ToList();
            if (system.boards.Count == 0) { finalBoard = victoriousBoards[0]; break; }
        }
        var x = finalBoard.SumOfUnmarked();
        return (finalBoard.SumOfUnmarked() * lastCalled).ToString();
    }

    class BingoSystem {

        public List<int> callingNumbers;
        public List<BingoBoard> boards = new List<BingoBoard>();

        public BingoSystem(List<string> rows) {
            callingNumbers = rows[0].Split(',').Select(n => int.Parse(n)).ToList();
            for (int i=2; i<rows.Count; i+=6) {
                boards.Add(new BingoBoard(rows.GetRange(i, 5)));
            }
        }

        public List<BingoBoard> CallNumber(int calledNumber) {
            boards.ForEach(b => b.CallBingoNumber(calledNumber));
            return boards.Where(b => b.CheckForVictory()).ToList();
        }

    }

    class BingoBoard {

        int[,] numberArray = new int[5, 5];
        bool[,] bingoArray = new bool[5, 5];

        public BingoBoard(List<string> rows) {
            for (int i=0; i<5; i++) {
                for (int j=0; j<5; j++) {
                    var split = rows[i].Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    numberArray[i, j] = int.Parse(split[j]);
                }
            }
        }

        public void CallBingoNumber(int number) {
            for (int i=0; i<5; i++) {
                for (int j=0; j<5; j++) {
                    if (numberArray[i, j] == number) {
                        bingoArray[i, j] = true;
                        return;
                    }
                }
            }
        }

        public bool CheckForVictory() {
            for (int i=0; i<5; i++) {
                if (bingoArray[i, 0] && bingoArray[i, 1] && bingoArray[i, 2] && bingoArray[i, 3] && bingoArray[i, 4]) { return true; }
                if (bingoArray[0, i] && bingoArray[1, i] && bingoArray[2, i] && bingoArray[3, i] && bingoArray[4, i]) { return true; }
            }
            return false;
        }

        public int SumOfUnmarked() {
            var sum = 0;
            for (int i = 0; i < 5; i++) {
                for (int j = 0; j < 5; j++) {
                    if (!bingoArray[i, j]) {
                        sum += numberArray[i, j];
                    }
                }
            }
            return sum;
        }

    }

}
 