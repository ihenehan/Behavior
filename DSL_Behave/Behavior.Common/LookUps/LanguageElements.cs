using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Behavior.Common.LookUps
{
    public class LanguageElements
    {
        public static List<string> BlockTypes
        {
            get
            {
                return new List<string> { "Before Story", "After Story", "Story", "Criterion Common",
                                          "Before Criterion", "After Criterion", "Criterion"};
            }
        }

        public static List<string> Keywords
        {
            get { return new List<string> { "Given", "When", "Then", "And", "Do", "Verify" }; }
        }

        public static string CommentToken
        {
            get { return "#"; }
        }

        public static string TagToken
        {
            get { return "@"; }
        }

        public static string ArgToken
        {
            get { return "[arg]"; }
        }

        public static string TableDelimiter
        {
            get { return "|"; }
        }
    }
}
