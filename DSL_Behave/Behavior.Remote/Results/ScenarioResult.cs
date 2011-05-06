using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Behavior.Common.Models;

namespace Behavior.Remote.Results
{
    public class CriterionResult
    {
        private List<StepResult> stepResults;

        public Result Result { get; set; }
        public Criterion Criterion { get; set; }
        public List<StepResult> StepResults 
        {
            get { return stepResults; }
            set{ stepResults = value; }
        }

        public CriterionResult()
        {
            StepResults = new List<StepResult>();
        }

        public void SetResult()
        {
            if (StepResults.Count.Equals(0))
                Result = Result.CreateFail("No steps defined.");

            else if (StepResults.Any(ir => ir.Result.status.ToLower().Equals("fail")))
                Result = Result.CreateFail();

            else
                Result = Result.CreatePass();
        }
    }
}
