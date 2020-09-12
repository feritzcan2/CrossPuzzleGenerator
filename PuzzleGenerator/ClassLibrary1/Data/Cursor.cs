using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PuzzleGenerator.Generator
{
    public class Cursor
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }

        private Random _rand;
        private LinkedList<Tuple<int, int>> _wordHistory;
        private List<Tuple<int, int>> _movementHistory;
        private Tuple<int, int> _latestPosition;
        private int _latestCharCount;


        public Cursor(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
            _rand = new Random();
            _movementHistory = new List<Tuple<int, int>>();
            _wordHistory = new LinkedList<Tuple<int, int>>();
            _latestCharCount = 0;
        }

        public Cursor Clone()
        {
            var cursor = new Cursor(Rows, Columns, _wordHistory, _movementHistory);
            return cursor;
        }

        public Cursor(int rows, int columns, LinkedList<Tuple<int, int>> wordHistory, List<Tuple<int, int>>? _latestPosition)
        {
            _latestCharCount = wordHistory.Count;
            Rows = rows;
            Columns = columns;
            _rand = new Random();
            _movementHistory = new List<Tuple<int, int>>();
            _wordHistory = new LinkedList<Tuple<int, int>>();
            foreach (var history in wordHistory)
                _wordHistory.AddLast(Tuple.Create<int, int>(history.Item1, history.Item2));

        }

        public void SetWordHistory(LinkedList<Tuple<int, int>> wordHistory)
        {
            _wordHistory = wordHistory;
        }

        public void RandomizePosition()
        {
            SetPosition(_rand.Next(Rows), _rand.Next(Columns));
        }

        public void SetPosition(int x, int y)
        {
            _latestPosition = Tuple.Create<int, int>(X, Y);
            _latestCharCount = _wordHistory.Count;
            X = x;
            Y = y;
        }

        public void AddToMovementHistory(int x, int y)
        {
            _movementHistory.Add( Tuple.Create<int, int>(x, y));
        }

        public void Rollback()
        {
            X = _latestPosition.Item1;
            Y = _latestPosition.Item2;
            if (_wordHistory.Count > _latestCharCount)
            {
                _latestCharCount--;
                _wordHistory.RemoveLast();
            }
        }


        public void AddToWordHistory(int X, int Y)
        {
            _wordHistory.AddLast(Tuple.Create<int, int>(X, Y));

        }

        public LinkedList<Tuple<int, int>> GetWordHistory()
        {
            return _wordHistory;
        }

        public LinkedList<Tuple<int, int>> CopyWordHistory()
        {
            var copy = new LinkedList<Tuple<int, int>>();
            foreach (var pos in _wordHistory)
                copy.AddLast(Tuple.Create<int, int>(pos.Item1, pos.Item2));

            return copy;
        }

        private List<Tuple<int, int>> GetPossiblePositions()
        {
            var positions = new List<Tuple<int, int>>();
            if (X < Rows - 1) positions.Add(Tuple.Create<int, int>(X + 1, Y)); // asagı
            if (X > 0) positions.Add(Tuple.Create<int, int>(X - 1, Y)); // yukarı
            if (Y > 0) positions.Add(Tuple.Create<int, int>(X, Y - 1)); //  sola 
            if (Y < Columns - 1) positions.Add(Tuple.Create<int, int>(X, Y + 1));// sağa
            if (X < Rows - 1 && Y < Columns - 1) positions.Add(Tuple.Create<int, int>(X + 1, Y + 1)); // asagı sağa
            if (X < Rows - 1 && Y > 0) positions.Add(Tuple.Create<int, int>(X + 1, Y - 1)); // asagı sola
            if (X > 0 && Y < Columns - 1) positions.Add(Tuple.Create<int, int>(X - 1, Y + 1)); // yukarı sağa
            if (X > 0 && Y > 0) positions.Add(Tuple.Create<int, int>(X - 1, Y - 1)); // yukarı sola
            positions = positions.Except(_movementHistory).Except(_wordHistory).ToList();

            return positions;
        }

        public bool MoveRandomly()
        {
            var possiblePositions = GetPossiblePositions();
            if (!possiblePositions.Any()) return false;

            var position = possiblePositions[_rand.Next(possiblePositions.Count)];

            SetPosition(position.Item1, position.Item2);
            _movementHistory.Add(position);
            return true;
        }

        public bool MoveByValidationMatrix(bool[,] validationMatrix)
        {
            _movementHistory = new List<Tuple<int, int>>();
            for (int a = 0; a < validationMatrix.GetLength(0); a++)
            {
                for (int b = 0; b < validationMatrix.GetLength(1); b++)
                {
                    if (!validationMatrix[a, b])
                    {
                        SetPosition(a, b);
                        _movementHistory.Add(Tuple.Create<int,int>(a,b));

                        return true;
                    }
                }
            }
            return false;
        }
    }
}
