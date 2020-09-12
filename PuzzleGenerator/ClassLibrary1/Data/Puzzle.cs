using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Generator.Data
{
    public class Puzzle
    {
        public char[,] PuzzleGrid { get; set; }

        public int Rows { get; set; }

        public int Columns { get; set; }

        public IList<WordData> Words { get; set; }

        public string ToString()
        {

            var wordData = "[";
            Words = Words.OrderBy(x => x.Word.Length).ToList();
            foreach (var word in Words)
            {
                wordData += "\"" + word.Word + "\"";
                if (word != Words.Last())
                {
                    wordData += ",";
                }
            }
            wordData += "]";
            var data = "[";
            for (int a = 0; a < Rows; a++)
            {
                var rowData = "[";
                for (int b = 0; b < Columns; b++)
                {
                    rowData += "\"" + PuzzleGrid[a, b] + "\"";
                    if (b != Columns - 1) rowData += ",";
                }
                data += (rowData + "]");
                if (a != Rows - 1) data += ",";
            }
            data += "]";

            var positionData = "[";
            foreach (var word in Words)
            {

                positionData += "[";
                foreach (var position in word.Position)
                {

                    var posData = "[";

                    posData += "" + position.Item1 + ",";
                    posData += "" + position.Item2;
                    posData += "]";
                    if (position != word.Position.Last.Value)
                    {
                        posData += ",";
                    }
                    positionData += posData;

                }


                positionData += "]";
                if (word != Words[Words.Count - 1])
                {
                    positionData += ",";
                }

            }
            positionData += "]";

            var json = "{ \"words\":" + wordData;

            json += ",\"data\":" + data;
            json += " ,";
            json += "\"position\":" + positionData;
            json += "}";
            return json;



        }

        public Puzzle Clone()
        {
            return new Puzzle
            {
                PuzzleGrid = CopyPuzzleMatrix(),
                Rows = Rows,
                Columns = Columns,
                Words = Words
            };
        }

        private char[,] CopyPuzzleMatrix()
        {
            var tempPuzzle = (char[,])PuzzleGrid;
            for (int a = 0; a < Rows; a++)
                for (int b = 0; b < Columns; b++)
                    tempPuzzle[a, b] = PuzzleGrid[a, b];

            return tempPuzzle;
        }

        public void PrintPuzzle()
        {
            foreach (var word in Words)
            {
                Console.WriteLine("");

                Console.Write("Word: '" + word.Word + "'");
                Console.Write("   Position: ");
                foreach (var pos in word.Position)
                {
                    Console.Write("(" + pos.Item1 + "," + pos.Item2 + ")=>");
                }

            }
            Console.WriteLine("");
            Console.WriteLine("");

            Console.WriteLine("--------------------------------");
            for (int row = 0; row < Rows; row++)
            {
                Console.WriteLine("--------------------------------");

                for (int column = 0; column < Columns; column++)
                {
                    Console.Write("|  ");
                    Console.Write(PuzzleGrid[row, column]);
                    Console.Write("   |");
                }
                Console.WriteLine("--------------------------------");

            }
        }


    }
}
