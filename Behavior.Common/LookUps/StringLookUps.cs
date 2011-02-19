using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Behavior.Common.LookUps
{
    public class StringLookUps
    {
        private static List<string> scenarioTypes = new List<string>() { "Test", "Test Setup", "Test Teardown", "Story Setup", "Story Teardown" };
        
        public static List<string> ScenarioTypes
        {
            get { return scenarioTypes; }
        }

        public static string ScenarioTypesDefault
        {
            get { return scenarioTypes[0]; }
        }
    }
}
