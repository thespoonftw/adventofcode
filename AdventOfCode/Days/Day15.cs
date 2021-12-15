using System;
using System.Collections.Generic;
using System.Linq;

public class Day15 : DayBase {

    protected override long PartOne(List<string> data) {
        var searcher = new Searcher(data, 1);
        return searcher.GetRisk();
    }

    protected override long PartTwo(List<string> data) {
        var searcher = new Searcher(data, 5);
        return searcher.GetRisk();
    }

    class Searcher {

        private readonly Node[,] array;
        private readonly List<Node> nodesToCheck = new List<Node>();
        private readonly int gridSize;
        private readonly Node startNode;
        private readonly Node endNode;

        public Searcher(List<string> data, int gridSizeMultiplier) {

            var smallGridSize = data.Count;
            gridSize = smallGridSize * gridSizeMultiplier;
            array = new Node[gridSize, gridSize];
            
            for (int i = 0; i < gridSizeMultiplier; i++) {
                for (int j = 0; j < gridSizeMultiplier; j++) {
                    for (int x = 0; x < smallGridSize; x++) {
                        for (int y = 0; y < smallGridSize; y++) {
                            var value = data[y][x] - '0';
                            var modValue = (value + i + j - 1) % 9 + 1;                            
                            var X = x + (i * smallGridSize);
                            var Y = y + (j * smallGridSize);
                            var node = new Node(X, Y, modValue, gridSize);
                            array[X, Y] = node;
                        }
                    }
                }
            }

            startNode = array[0, 0];
            endNode = array[gridSize - 1, gridSize - 1];
            nodesToCheck.Add(startNode);
            startNode.costSoFar = 0;
            startNode.fScore = startNode.heuristic;            
        }

        private void SolveNetwork() {
            while (true) {
                var lowestUnvisited = GetNextNode();

                if (lowestUnvisited == endNode) { break; }

                var unvisitedNeighbours = GetUnvisitedNeighbours(lowestUnvisited);

                foreach (var neighbour in unvisitedNeighbours) {
                    var newCostSoFar = lowestUnvisited.costSoFar + neighbour.value;
                    if (newCostSoFar < neighbour.costSoFar) {
                        neighbour.UpdateCosts(lowestUnvisited, newCostSoFar);
                        nodesToCheck.Add(neighbour);
                    }
                }

                lowestUnvisited.isVisited = true;
                nodesToCheck.Remove(lowestUnvisited);
            }
        }

        public int GetRisk() {
            SolveNetwork();
            var counter = 0;
            Node currentNode = endNode;
            while (currentNode != startNode) {
                //Console.WriteLine(currentNode.x + "," + currentNode.y);
                counter += currentNode.value;
                currentNode = currentNode.previous;                
            }
            return counter;
        }

        private Node GetNextNode() {
            Node returner = nodesToCheck[0];
            foreach (var n in nodesToCheck) {  
                if (n.fScore < returner.fScore) { returner = n; }
            }
            return returner;
        }

        private List<Node> GetUnvisitedNeighbours(Node node) {
            var returner = new List<Node>();
            var x = node.x;
            var y = node.y;
            if (x > 0) { returner.Add(array[x - 1, y]); }
            if (y > 0) { returner.Add(array[x, y - 1]); }
            if (x < gridSize - 1) { returner.Add(array[x + 1, y]); }
            if (y < gridSize - 1) { returner.Add(array[x, y + 1]); }
            returner.RemoveAll(n => n.isVisited);
            return returner;
        }
    }

    class Node {

        public readonly int x;
        public readonly int y;
        public readonly int value;
        public readonly int heuristic;

        public int costSoFar;
        public int fScore;
        public Node previous;
        public bool isVisited = false;

        public Node(int x, int y, int value, int gridSize) {
            this.x = x;
            this.y = y;
            this.value = value;
            heuristic = gridSize + gridSize - x - y - 1;
            costSoFar = gridSize * 10;
            fScore = gridSize * 10;
        }

        public void UpdateCosts(Node previous, int costSoFar) {
            this.previous = previous;
            this.costSoFar = costSoFar;
            fScore = heuristic + costSoFar;
        }
    
    }

}
 