using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Behavior.Common.Models;

namespace Behavior.Remote.Results
{
    public class ScenarioResult
    {
        private List<InteractionResult> interactionResults;

        public Result Result { get; set; }
        public string ScenarioName { get; set; }
        public List<InteractionResult> InteractionResults 
        {
            get { return interactionResults; }
            set{ interactionResults = value; }
        }

        public ScenarioResult()
        {
            InteractionResults = new List<InteractionResult>();
        }

        public void SetResult()
        {
            if (InteractionResults.Any(ir => ir.Result.status.ToLower().Equals("fail")))
                Result = Result.CreateFail();
            else
                Result = Result.CreatePass();
        }
    }
}
