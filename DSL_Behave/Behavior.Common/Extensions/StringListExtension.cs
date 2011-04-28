using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Behavior.Common.Extensions
{
    public static class StringListExtension
    {
        public static List<string> PrepAllLines(this List<string> lines)
        {
            for (int i = 0; i < lines.Count; i++)
            {
                lines[i] = lines[i].Trim();
                lines[i] = lines[i].Replace("\t", "");
            }

            lines.RemoveAll(l => string.IsNullOrEmpty(l));

            lines.RemoveAll(l => l.StartsWith("#"));

            return lines;
        }
    }
}
