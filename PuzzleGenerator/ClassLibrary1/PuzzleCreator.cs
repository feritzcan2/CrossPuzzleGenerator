using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Generator;
using Generator.Data;
using PuzzleGenerator.Data;

namespace PuzzleGenerator.Generator
{
    public class PuzzleCreator
    {
        private Random _rand;
        private WordDataSet _dataSet;
        private int _rows;
        private int _columns;
        private PuzzleValidator _puzzleValidator;
        private PuzzleOptions _options;

        public PuzzleCreator(PuzzleOptions options)
        {
            _dataSet = new WordDataSet(options);
            _options = options;
            _rows = options.Rows;
            _columns = options.Columns;
            _puzzleValidator = new PuzzleValidator(options, _dataSet);
            _rand = new Random();
        }

        public Puzzle CreatePuzzle(PuzzleOptions options)
        {
            Puzzle puzzle;
            bool status = false;
            do
            {
                puzzle = new Puzzle
                {
                    Rows = options.Rows,
                    Columns = options.Columns,
                    PuzzleGrid = new char[options.Rows, options.Columns],
                    Words = new List<WordData>()
                };

                status = CreatePuzzleRecursively(puzzle, _options.MaxCharLength + 1);

                var watch = new Stopwatch();

            } while (!status || !_puzzleValidator.ValidatePuzzle(puzzle));
            return puzzle;
        }

        private bool CreatePuzzleRecursively(Puzzle puzzle, int currentWordLength)
        {
            if (currentWordLength == _options.MinCharLength)
            {
                return true;
            }
            var status = PlaceWordRecursively(puzzle, currentWordLength - 1);
            if (!status) return false;

            return CreatePuzzleRecursively(puzzle, currentWordLength - 1);
        }

        private bool PlaceWordRecursively(Puzzle puzzle, int wordLength)
        {
            var _cursor = new Cursor(_rows, _columns);
            _cursor.RandomizePosition();
            var wordSet = _dataSet.GetWordList(wordLength);
            var time = DateTime.UtcNow;
            var appendStatus = AppendCharsRecursively(puzzle, wordLength, 0, wordSet, _cursor, null, time);

            return appendStatus;
        }

        private bool AppendCharsRecursively(Puzzle puzzle, int length, int currentIndex, EqualLengthWordSet wordSet, Cursor cursor, string selectedWord, DateTime startTime)
        {
            if (currentIndex == length)
            {
                if (wordSet.GetWordCount() == 0)
                {
                    return false;
                }

                puzzle.Words.Add(new WordData
                {
                    Word = selectedWord ?? wordSet.GetFinalWord(),
                    Position = cursor.GetWordHistory()
                });
                return true;
            }

            var tempWordSet = wordSet.Clone();
            var tempSelectedWord = (string)selectedWord?.Clone();
            var tempPuzzle = puzzle.Clone();


            var tempCursor = new Cursor(cursor.Rows, cursor.Columns, cursor.CopyWordHistory(),null);
            tempCursor.SetPosition(cursor.X, cursor.Y);
            bool appendStatus;
            do
            {
                var didMove = tempCursor.MoveRandomly();
                if (!didMove) return false;

                appendStatus = TryChooseChar(tempPuzzle.PuzzleGrid, length, currentIndex, tempWordSet, tempCursor, ref tempSelectedWord);
                if (appendStatus == false)
                {
                    tempCursor.Rollback();
                    tempPuzzle = puzzle;
                }
                else
                {

                    var nextAppendStatus = AppendCharsRecursively(tempPuzzle, length, currentIndex + 1, tempWordSet, tempCursor, tempSelectedWord, startTime);
                    if (nextAppendStatus == false)
                    {
                        tempCursor.Rollback();
                        tempWordSet.RollBack();
                        tempPuzzle = puzzle.Clone();
                        appendStatus = false;
                        continue;
                    }
                    else
                    {
                        puzzle.PuzzleGrid = tempPuzzle.PuzzleGrid;
                        cursor = tempCursor;
                        return nextAppendStatus;
                    }
                }
            } while (!appendStatus);

            return true;
        }

        private bool TryChooseChar(char[,] puzzle, int length, int charIndex, EqualLengthWordSet wordSet, Cursor cursor, ref string selectedWord)
        {
            var cursorChar = GetCursorChar(cursor, puzzle);
            if (cursorChar.HasValue)
            {
                var result = LimitCharacter(wordSet, cursorChar.Value, charIndex, cursor);
                if (!result) return false;

                if (selectedWord != null && cursorChar != selectedWord[charIndex])
                {
                    selectedWord = wordSet.GetRandomWord();
                }
            }
            else
            {
                if (selectedWord == null)
                {
                    selectedWord = wordSet.GetRandomWord();
                    LimitCharacter(wordSet, selectedWord[charIndex], charIndex, cursor);
                    SetCursorChar(puzzle, selectedWord[charIndex], cursor);
                }
                else
                {
                    SetCursorChar(puzzle, selectedWord[charIndex], cursor);
                    LimitCharacter(wordSet, selectedWord[charIndex], charIndex, cursor);
                }

            }
            return true;
        }

     
        private bool LimitCharacter(EqualLengthWordSet wordSet, char cursorChar, int charIndex, Cursor cursor)
        {
            var hasWord = wordSet.LimitCharacter(cursorChar, charIndex);
            if (!hasWord)
            {
                wordSet.RollBack();
                return false;
            }
            cursor.AddToWordHistory(cursor.X, cursor.Y);
            return true;
        }

        private char? GetCursorChar(Cursor cursor, char[,] puzzle)
        {
            var c = puzzle[cursor.X, cursor.Y];
            if (c == '\0')
            {
                return null;
            }
            return c;
        }

        private void SetCursorChar(char[,] puzzle, char c, Cursor cursor)
        {
            puzzle[cursor.X, cursor.Y] = c;
        }
    }
}
