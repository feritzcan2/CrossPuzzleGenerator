using System;
using System.Collections.Generic;
using System.Linq;

namespace Generator
{
    public static class Extensions
    {
        public static List<string> LimitCharacter(this IList<string> list, char value, int index, int length)
        {
            return list.Where(x => x.Length >= length && x[index] == value).ToList();
        }
    }
}
