using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PuzzleGenerator
{
    public class EqualLengthWordSet
    {
        private LinkedList<IList<string>> _history;
        private int _length;
        private IList<string> _words;
        private Random _rand;

        public EqualLengthWordSet(int length)
        {
            _length = length;

            _history = new LinkedList<IList<string>>();
            _words = new List<string>();
            _rand = new Random();
        }

        private EqualLengthWordSet(int length, IList<string> words)
        {
            _length = length;

            _history = new LinkedList<IList<string>>();
            _words = new List<string>();
            _words = words.Select(x => x).ToList();
            _rand = new Random();
        }

        public EqualLengthWordSet Clone()
        {
            return new EqualLengthWordSet(_length, _words);
        }

        public int GetWordCount()
        {
            return _words.Count;
        }

        public string GetFinalWord()
        {
            if (_words.Count == 0) return null;
            return _words[0];
        }

        public void AddWord(string word)
        {
            _words.Add(word);
        }

        public bool LimitCharacter(char value, int index)
        {
            var limitedList = _words.Where(x => x[index] == value).ToList();

            _history.AddLast(_words);
            if (!limitedList.Any()) return false;

            _words = limitedList;
            return true;
        }

        public string GetRandomWord()
        {
            return _words[_rand.Next(_words.Count)];
        }

        public bool RollBack()
        {
            if (_history.Count == 0)
                return false;

            var latest = _history.Last();
            _words = latest;
            _history.RemoveLast();
            return true;
        }

        public void PushHistory()
        {
            _history.Append(_words);
        }
    }
}
