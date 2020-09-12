using PuzzleGenerator.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PuzzleGenerator
{
    public class WordDataSet
    {
        private PuzzleOptions _options;
        private Dictionary<int, EqualLengthWordSet> _equalLengthWordSetDictionary;
        private IList<string> _words;

        public WordDataSet(PuzzleOptions options)
        {
            _options = options;
            InitDataStructures();
            GenerateDataSet();
        }

        private void InitDataStructures()
        {
            _words = new List<string>();
            _equalLengthWordSetDictionary = new Dictionary<int, EqualLengthWordSet>();
            for (int a = _options.MinCharLength; a <= _options.MaxCharLength; a++)
                _equalLengthWordSetDictionary.Add(a, new EqualLengthWordSet(a));
        }

        public IList<string> GetWords()
        {
            return _words;
        }

        public EqualLengthWordSet GetWordList(int length)
        {
            return _equalLengthWordSetDictionary[length];
        }

        private void GenerateDataSet()
        {
            String word;
            try
            {
                StreamReader sr = new StreamReader(_options.WordFilePath);
                word = sr.ReadLine();
                while (word != null)
                {
                    word = sr.ReadLine();
                    if (word != null)
                    {
                        var isLengthOk = word.Length >= _options.MinCharLength
                                && word.Length <= _options.MaxCharLength;

                        if (isLengthOk)
                        {
                            _equalLengthWordSetDictionary[word.Length].AddWord(word);
                            _words.Add(word);
                        }
                    }


                }
                sr.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            finally
            {
                Console.WriteLine("Executing finally block.");
            }
        }


    }
}
