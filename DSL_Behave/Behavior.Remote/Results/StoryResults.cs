using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Behavior.Common.Models;

namespace Behavior.Remote.Results
{
    public class StoryResult
    {
        private List<CriterionResult> criterionResults;

        public Result Result { get; set; }
        public Story Story { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public string ExecutionTime
        {
            get
            {
                TimeSpan span = EndTime - StartTime;

                return string.Format("{0:D2}:{1:D2}:{2:D2}.{3:D3}", span.Hours, span.Minutes, span.Seconds, span.Milliseconds);
            }
        }

        public List<CriterionResult> CriterionResults
        {
            get { return criterionResults; }
            set { criterionResults = value; }
        }

        public StoryResult()
        {
            CriterionResults = new List<CriterionResult>();
        }

        public void SetResult()
        {
            if (CriterionResults.Any(sr => sr.Result.status.ToLower().Equals("fail")))
                Result = Result.CreateFail();
            else
                Result = Result.CreatePass();
        }
    }
}
