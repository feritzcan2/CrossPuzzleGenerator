using System;
using System.Collections.Generic;

namespace PuzzleGenerator
{
    public class WordListAnalyzer
    {
        private IList<string> _words;
        private int _length;

        public WordListAnalyzer(IList<string> words,int legth)
        {
            _words = words;
            _length = legth;
        }
    }
}
