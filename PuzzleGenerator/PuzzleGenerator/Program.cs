using System;
using System.Collections.Generic;
using System.IO;
using System.Json;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PuzzleGenerator;
using PuzzleGenerator.Data;
using PuzzleGenerator.Generator;

namespace Generator
{
    class Program
    {

        static void Create(PuzzleCreator puzzleCreator,int index, PuzzleOptions options)
        {
            int a = index;
            var puzzle = puzzleCreator.CreatePuzzle(options);


            string jsonString = "someString";
            try
            {
                var json = puzzle.ToString();
                var tmpObj = JsonValue.Parse(json);
                StreamWriter sw = new StreamWriter(@"/Users/feritozcan/Desktop/wordbender/PuzzleGeneratorC#/PuzzleGenerator/PuzzleGenerator/puzzles/puzzle_" + a + ".txt");
                sw.WriteLine(json);
                sw.Close();
            }
            catch (FormatException fex)
            {
                //Invalid json format
                Console.WriteLine(fex);
            }
            catch (Exception ex) //some other exception
            {
                Console.WriteLine(ex.ToString());
            }
        }
        static void Main(string[] args)
        {

           // var path = (@"C:\Users\ferit.ozcan\Desktop\wordbender\PuzzleGenerator\PuzzleGenerator\PuzzleGenerator\allwords.txt");
            var path = (@"/Users/feritozcan/Desktop/wordbender/PuzzleGeneratorC#/PuzzleGenerator/PuzzleGenerator/allwords.txt");
             //var path = (@"C:\Users\beytullah\Desktop\WordBender\PuzzleGenerator\PuzzleGenerator\PuzzleGenerator\allwords.txt");
            try
            {
                var options = new PuzzleOptions
                {
                    Rows = 4,
                    Columns = 4,
                    MaxCharLength = 12,
                    MinimumWordCount=150,
                    MinCharLength = 3,
                    WordFilePath = path
                };

                PuzzleCreator puzzleCreator = new PuzzleCreator(options);
                var actions = new List<Action>();
                var listt = new List<int>();
                for (int a = 0; a < 10000; a++)
                {
                   var puzzle = puzzleCreator.CreatePuzzle(options);
                    puzzle.PrintPuzzle();
                    Console.ReadLine();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Ex: " + e.ToString());
            }
        }
    }
}
