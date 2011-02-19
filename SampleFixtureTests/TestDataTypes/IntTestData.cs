using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SampleFixtureTests.TestDataTypes
{
    public class IntTestData
    {
        public int Index { get; set; }
        public List<string> Tags { get; set; }

        public int FirstInt { get; set; }
        public int SecondInt { get; set; }
        public int ExpectedAdd { get; set; }
        public int ExpectedSubtract { get; set; }

        public override string ToString()
        {
            return string.Format("{0} +/- {1} should equal {2}/{3}",
                                FirstInt.ToString(),
                                SecondInt.ToString(),
                                ExpectedAdd.ToString(),
                                ExpectedSubtract.ToString()
                                );
        }
    }
}
