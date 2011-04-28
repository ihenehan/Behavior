using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Behavior.Remote.Results
{
    public class InteractionResult
    {
        public Result Result { get; set; }
        public string KeywordName { get; set; }

        public InteractionResult() { }

        public InteractionResult(Result result)
        {
            Result = result;
        }

        public InteractionResult(Keyword keyword, Result result)
        {
            Result = result;
            KeywordName = keyword.Name;
        }

        public InteractionResult(Keyword keyword)
        {
            KeywordName = keyword.Name;

            Result = Result.CreateFail(string.Format("Failure to access keyword {0}.\r\n Keyword exists = {1}.\r\n Parameters are correct = {2}", 
                keyword.Name,
                keyword.KeywordExists, 
                keyword.ParametersAreCorrect));
        }
    }
}
