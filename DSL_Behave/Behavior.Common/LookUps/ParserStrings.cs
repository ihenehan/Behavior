using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Behavior.Common.LookUps
{
    public class ParserStrings
    {
        private static List<string> blockTypes = new List<string> { "Before Story", "After Story", "Story", "Scenario Common"
                                                                  , "Before Scenario", "After Scenario", "Scenario", "Test Data"};

        private static List<string> keywords = new List<string> { "Given", "When", "Then", "And" };


        public static List<string> BlockTypes
        {
            get { return blockTypes; }
        }

        public static List<string> Keywords
        {
            get { return keywords; }
        }
    }
}
