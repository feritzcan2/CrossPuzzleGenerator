using System;
using System.Collections.Generic;

namespace Generator.Data
{
    public class WordData
    {
        public LinkedList<Tuple<int, int>> Position { get; set; }

        public string Word { get; set; }
    }
}
