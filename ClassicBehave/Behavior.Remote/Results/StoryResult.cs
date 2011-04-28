using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Behavior.Common.Models;

namespace Behavior.Remote.Results
{
    public class StoryResult
    {
        private List<ScenarioResult> scenarioResults;

        public Result Result { get; set; }
        public string StoryName { get; set; }
        public List<ScenarioResult> ScenarioResults 
        {
            get { return scenarioResults; }
            set { scenarioResults = value; }
        }

        public StoryResult()
        {
            ScenarioResults = new List<ScenarioResult>();
        }

        public void SetResult()
        {
            if (ScenarioResults.Any(sr => sr.Result.status.ToLower().Equals("fail")))
                Result = Result.CreateFail();
            else
                Result = Result.CreatePass();
        }
    }
}
