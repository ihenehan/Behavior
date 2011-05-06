using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Behavior.Common.Repository;
using Behavior.Common.Models;
using Behavior.Common.Configuration;

namespace Behavior.Remote.Results
{
    public class TestRunResult : Item
    {
        private List<StoryResult> storyResults;

        public Result Result { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public int PassingStories
        {
            get
            {
                return storyResults.Count(sr => sr.Result.status.Equals("PASS"));
            }
        }

        public int PassingScenarios
        {
            get
            {
                var count = 0;

                foreach (StoryResult sr in storyResults)
                    foreach (ScenarioResult sc in sr.ScenarioResults)
                        if (sc.Result.status.Equals("PASS"))
                            count++;

                return count;
            }
        }

        public int PassingSteps
        {
            get
            {
                var count = 0;

                foreach (StoryResult sr in storyResults)
                    foreach (ScenarioResult sc in sr.ScenarioResults)
                        foreach(StepResult st in sc.StepResults)
                            if (st.Result.status.Equals("PASS"))
                                count++;

                return count;
            }
        }

        public int FailedStories 
        {
            get
            {
                return storyResults.Count(sr => sr.Result.status.Equals("FAIL"));
            }
        }

        public int FailedScenarios 
        {
            get
            {
                var count = 0;

                foreach (StoryResult sr in storyResults)
                    foreach (ScenarioResult sc in sr.ScenarioResults)
                        if (sc.Result.status.Equals("FAIL"))
                            count++;

                return count;
            }
        }

        public int FailedSteps 
        {
            get
            {
                var count = 0;

                foreach (StoryResult sr in storyResults)
                    foreach (ScenarioResult sc in sr.ScenarioResults)
                        foreach (StepResult st in sc.StepResults)
                            if (st.Result.status.Equals("FAIL"))
                                count++;

                return count;
            }
        }

        public string ExecutionTime
        {
            get
            {
                TimeSpan span = EndTime - StartTime;

                return string.Format("{0:D2}:{1:D2}:{2:D2}.{3:D3}", span.Hours, span.Minutes, span.Seconds, span.Milliseconds);
            }
        }

        public List<StoryResult> StoryResults 
        {
            get { return storyResults; }
            set { storyResults = value; }
        }

        public TestRunResult()
        {
            StoryResults = new List<StoryResult>();
        }

        public TestRunResult SetResult()
        {
            if (StoryResults.Any(sr => sr.Result.status.ToLower().Equals("fail")))
                Result = Result.CreateFail();
            else
                Result = Result.CreatePass();

            return this;
        }
    }
}
