using System;
using System.Collections.Generic;
using System.Linq;

public class Day20 : DayBase {

    public List<bool> imageEnhancementAlgorithm;
    public bool[,] array;

    public void LoadData(List<string> data) {
        imageEnhancementAlgorithm = new List<bool>();
        var firstLine = data[0];
        for (int i=0; i<firstLine.Length; i++) {
            imageEnhancementAlgorithm.Add(firstLine[i] == '#');
        }

        var width = data[2].Length;
        array = new bool[width, width];
        for (int x = 0; x < width; x++) { 
            for (int y = 0; y < width; y++) {
                array[x, y] = data[y + 2][x] == '#';
            }
        }
    }
    
    public void IncreaseArraySize() {
        int newWidth = array.GetLength(0) + 2;
        var biggerArray = new bool[newWidth, newWidth];
        for (int x = 1; x < newWidth - 1; x++) { 
            for (int y = 1; y < newWidth - 1; y++) {
                biggerArray[x, y] = array[x - 1, y - 1];            
            }
        }
        array = biggerArray;
    }

    public void DecreaseArraySize() {
        int newWidth = array.GetLength(0) - 2;
        var smallerArray = new bool[newWidth, newWidth];
        for (int x = 0; x < newWidth; x++) {
            for (int y = 0; y < newWidth; y++) {
                smallerArray[x, y] = array[x + 1, y + 1];
            }
        }
        array = smallerArray;
    }

    public void Enhance() {
        int width = array.GetLength(0);
        var enhancedArray = new bool[width, width];
        for (int y = 1; y < width - 1; y++) {
            for (int x = 1; x < width - 1; x++) { 

                var binaryString = "";
                binaryString += GetBinaryChar(x - 1, y - 1);
                binaryString += GetBinaryChar(x + 0, y - 1);
                binaryString += GetBinaryChar(x + 1, y - 1);
                binaryString += GetBinaryChar(x - 1, y + 0);
                binaryString += GetBinaryChar(x + 0, y + 0);
                binaryString += GetBinaryChar(x + 1, y + 0);
                binaryString += GetBinaryChar(x - 1, y + 1);
                binaryString += GetBinaryChar(x + 0, y + 1);
                binaryString += GetBinaryChar(x + 1, y + 1);
                var referenceIndex = Convert.ToInt32(binaryString, 2);
                var isChecked = imageEnhancementAlgorithm[referenceIndex];
                enhancedArray[x, y] = isChecked;
            }
        }
        array = enhancedArray;
    }

    public int CountLights() {
        int counter = 0;
        int width = array.GetLength(0);
        for (int x = 0; x < width; x++) { 
            for (int y = 0; y < width; y++) {
                if (array[x, y]) { counter++; }
            }
        }
        return counter;
    }

    public void PrintArray() { 
        int width = array.GetLength(0);
        for (int y = 0; y < width; y++) {
            for (int x = 0; x < width; x++) { 
                Console.Write(array[x, y] ? '#' : '.');
            }
            Console.Write("\n");
        }
    }

    private char GetBinaryChar(int x, int y) {
        return array[x, y] ? '1' : '0';
    }

    protected override long PartOne(List<string> data) {
        LoadData(data);
        IncreaseArraySize();
        IncreaseArraySize();
        IncreaseArraySize();
        IncreaseArraySize();
        Enhance();
        DecreaseArraySize();     
        Enhance();
        DecreaseArraySize();
        return CountLights();
    }

    // 5362 incorrect

    protected override long PartTwo(List<string> data) {
        LoadData(data);
        for (int i = 0; i<25; i++) {
            IncreaseArraySize();
            IncreaseArraySize();
            IncreaseArraySize();
            IncreaseArraySize();
            Enhance();
            DecreaseArraySize();
            Enhance();
            DecreaseArraySize();
        }
        PrintArray();
        return CountLights();
    }

}
 