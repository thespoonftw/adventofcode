using System;
using System.Collections.Generic;
using System.Linq;

public class Day16 : DayBase {

    protected override long PartOne(List<string> data) {
        var decoder = new Decoder(data[0]);
        return decoder.outerPacket.GetSimpleValue();
    }

    protected override long PartTwo(List<string> data) {
        var decoder = new Decoder(data[0]);
        return decoder.outerPacket.GetComplexValue();
    }

    class Decoder {

        private int indexer = 0;
        private readonly string input;
        public readonly Packet outerPacket;

        public Decoder(string line) {

            input = HexStringToBits(line);
            outerPacket = GetPacket();
        }

        public class Packet {
            public long version;
            public long typeId;
            public long literalValue;
            public List<Packet> subPackets = new List<Packet>();

            public long GetSimpleValue() {
                return version + subPackets.Sum(p => p.GetSimpleValue());
            }

            public long GetComplexValue() {
                switch (typeId) {
                    case 0: return subPackets.Sum(p => p.GetComplexValue());
                    case 1: long returner = 1; subPackets.ForEach(p => returner *= p.GetComplexValue()); return returner;
                    case 2: return subPackets.Min(p => p.GetComplexValue());
                    case 3: return subPackets.Max(p => p.GetComplexValue());
                    case 4: return literalValue;
                    case 5: return subPackets[0].GetComplexValue() > subPackets[1].GetComplexValue() ? 1 : 0;
                    case 6: return subPackets[0].GetComplexValue() < subPackets[1].GetComplexValue() ? 1 : 0;
                    case 7: return subPackets[0].GetComplexValue() == subPackets[1].GetComplexValue() ? 1 : 0;
                    default: return 0;
                }
            }
        }

        private Packet GetPacket() {
            var returner = new Packet();
            returner.version = GetNextInt(3);
            returner.typeId = GetNextInt(3);

            // literal value
            if (returner.typeId == 4) {
                returner.literalValue = GetLiteralValue();

            // next 15 bits are a number that represents total length in bits of subpackets in this packet
            } else if (GetNextInt(1) == 0) {                
                var lengthOfSubpackets = GetNextInt(15);
                var endIndex = indexer + lengthOfSubpackets;
                while (indexer < endIndex) {
                    returner.subPackets.Add(GetPacket());
                }

            // next 11 bits are a number representing number of sub packets in this packet
            } else {                
                var numberOfSubpackets = GetNextInt(11);
                for (int i=0; i<numberOfSubpackets; i++) {
                    returner.subPackets.Add(GetPacket());
                }
            }
            return returner;
        }

        private long GetNextInt(int amount) {
            var returner = BitsToInt(input.Substring(indexer, amount));
            indexer += amount;
            return returner;
        }

        private long GetLiteralValue() {
            var numberString = "";
            bool endbit = false;
            while (!endbit)
            {
                endbit = input[indexer] == '0';
                numberString += input.Substring(indexer + 1, 4);
                indexer += 5;
            }
            return BitsToInt(numberString);
        }

        private string HexStringToBits(string s) {
            var returner = "";
            s.ToList().ForEach(c => returner += HexCharToBits(c));
            return returner;
        }

        private string HexCharToBits(char c) {
            var i = c - '0';
            if (i > 9) { i -= 7; }
            return Convert.ToString(i, 2).PadLeft(4, '0');            
        }

        private long BitsToInt(string s) {
            return Convert.ToInt64(s, 2);
        }
    }

}
 