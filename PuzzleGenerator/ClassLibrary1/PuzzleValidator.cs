using System;
using System.Collections.Generic;
using System.Linq;
using Generator.Data;
using PuzzleGenerator;
using PuzzleGenerator.Data;
using PuzzleGenerator.Generator;

namespace Generator
{
    public class PuzzleValidator
    {
        private PuzzleOptions _options;
        private WordDataSet _dataSet;

        public PuzzleValidator(PuzzleOptions options, WordDataSet dataSet)
        {
            _options = options;
            _dataSet = dataSet;
        }

        public bool ValidatePuzzle(Puzzle puzzle)
        {
            //Console.WriteLine(puzzle.ToString());
            ValidateAutoGenousWordsNotExist(puzzle);
           // Console.WriteLine(puzzle.ToString());

            var puzzleWordsValidated = ValidatePuzzleWords(puzzle);

            return puzzleWordsValidated;
        }

        private bool ValidatePuzzleWords(Puzzle puzzle)
        {
            if (puzzle.Words.Count < _options.MinimumWordCount) return false;
            var minLength = _options.MinCharLength;
            var maxLength = _options.MaxCharLength;

            foreach (var word in puzzle.Words)
            {
                var wordFromPositions = "";
                foreach (var position in word.Position)
                {
                    wordFromPositions += puzzle.PuzzleGrid[position.Item1, position.Item2];
                }
                if (!wordFromPositions.Equals(word.Word)) { 
                    return false; }
            }
            var wordLengths = puzzle.Words.Select(x => x.Word.Length).ToList();
      

            var words = puzzle.Words.OrderBy(x => x.Word.Length).ToList();

            foreach (var word in words)
            {
                if (word.Word.Length < minLength|| word.Word.Length > maxLength) {
                    return false; 
                }
                
            }

            return true;
        }

        private bool ValidateAutoGenousWordsNotExist(Puzzle puzzle)
        {
            var isValidated = GetEmptyPuzzleValidationMatrix();
            var cursor = new Cursor(_options.Rows, _options.Columns);
            while (cursor.MoveByValidationMatrix(isValidated))
            {
                var notExist = ValidateAutoGenousWordNotExistOnCursorPositionRecursively(puzzle, cursor, _dataSet.GetWords(), "");
                if (!notExist) return false;
                else isValidated[cursor.X, cursor.Y] = true;
                cursor = new Cursor(_options.Rows, _options.Columns);

            }

            return true;
        }

        private bool ValidateAutoGenousWordNotExistOnCursorPositionRecursively(Puzzle puzzle, Cursor cursor, IList<string> wordSet, string currentWord)
        {
            var tempCursor = cursor.Clone();
            tempCursor.SetPosition(cursor.X, cursor.Y);

            var tempWordSet = new List<string>(wordSet);
            var tempCurrentWord = (string)currentWord.Clone();
            var currentChar = puzzle.PuzzleGrid[tempCursor.X, tempCursor.Y];
            tempCurrentWord += currentChar;
            tempCursor.AddToWordHistory(tempCursor.X, tempCursor.Y);


            tempWordSet = tempWordSet.LimitCharacter(currentChar, tempCurrentWord.Length - 1, tempCurrentWord.Length);

            var hasWord = tempWordSet.Contains(tempCurrentWord);
            if (tempWordSet.Count() == 0)
            {
                return true;
            }
            else if (hasWord )
            {
                var wordExistInPuzzle = puzzle.Words.Where(x => x.Word.Equals(tempCurrentWord))
                                                    .Count() > 0;

                if (!wordExistInPuzzle)
                {
                   // Console.WriteLine("Validation:  " + tempCurrentWord);
                    puzzle.Words.Add(new WordData
                    {
                        Position = tempCursor.GetWordHistory(),
                        Word = tempCurrentWord
                    }); 
                };
            }

            while (tempCursor.MoveRandomly())
            {
                var status = ValidateAutoGenousWordNotExistOnCursorPositionRecursively(puzzle, tempCursor, tempWordSet, tempCurrentWord);
                tempCursor.Rollback();
                if (!status)
                {
                    return false;

                }

            }

            return true;
        }

        private bool[,] GetEmptyPuzzleValidationMatrix()
        {
            var validated = new bool[_options.Rows, _options.Columns];
            for (int a = 0; a < _options.Rows; a++)
            {
                for (int b = 0; b < _options.Columns; b++)
                {
                    validated[a, b] = false;
                }
            }

            return validated;
        }



    }
}
