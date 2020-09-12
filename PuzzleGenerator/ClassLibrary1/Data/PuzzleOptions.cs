using System;
using System.Collections.Generic;
using System.Text;

namespace PuzzleGenerator.Data
{
    public class PuzzleOptions
    {
        public int Rows { get; set; }

        public int Columns { get; set; }

        public int MinCharLength { get; set; }

        public int MaxCharLength { get; set; }

        public string WordFilePath { get; set; }
        public int MinimumWordCount { get; set; }
    }
}
