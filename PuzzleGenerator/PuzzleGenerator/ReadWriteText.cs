//using System;
//using System.Globalization;
//using System.IO;

//namespace PuzzleGenerator
//{
//    class ReadWriteText
//    {
//        static void Main(string[] args)
//        {
//            String line;
//            try
//            {
//                //Pass the file path and file name to the StreamReader constructor
//                StreamReader sr = new StreamReader(@"C:\Users\ferit.ozcan\Desktop\wordbender\PuzzleGenerator\PuzzleGenerator\PuzzleGenerator\wordsearchpuzzle.txt");


//                //Read the first line of text
//                line = sr.ReadLine().TrimEnd();
//                var text = line;
//                StreamWriter sw = new StreamWriter(@"C:\Users\ferit.ozcan\Desktop\wordbender\PuzzleGenerator\PuzzleGenerator\PuzzleGenerator\allwords.txt");

//                var maxCharrs = 0;
//                var culture = CultureInfo.InstalledUICulture;
//                //Continue to read until you reach end of file
//                while (line != null)
//                {

//                    //write the lie to console window

//                    //Read the next line
//                    line = sr.ReadLine()?.TrimEnd();
//                    if (line != null && !line.Contains(" ") && line.Length > 1)
//                    {
//                        if (line.Length > maxCharrs)
//                        {
//                            maxCharrs = line.Length;
//                            Console.WriteLine(maxCharrs);
//                            Console.WriteLine(line);
//                        }
//                        line = line.ToUpper(culture);
//                        text += "\n";
//                        text += line;

//                    }

//                }
//                sw.WriteLine(text);
//                sw.Close();

//                //close the file
//                sr.Close();
//                Console.ReadLine();
//            }
//            catch (Exception e)
//            {
//                Console.WriteLine("Exception: " + e.Message);
//            }
//            finally
//            {
//                Console.WriteLine("Executing finally block.");
//            }
//        }
//    }
//}
