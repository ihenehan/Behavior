using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Behavior.Remote.Results
{
    public class StepResult
    {
        public Result Result { get; set; }
        public string KeywordName { get; set; }

        public StepResult() { }

        public StepResult(Result result)
        {
            Result = result;
        }

        public StepResult(Keyword keyword, Result result)
        {
            Result = result;
            KeywordName = keyword.Name;
        }

        public StepResult(Keyword keyword)
        {
            KeywordName = keyword.Name;

            Result = Result.CreateFail(string.Format("Can't find step '{0}'.</br>- - - - ->Step exists = {1}.\r\n Parameters are correct = {2}", 
                keyword.Name,
                keyword.KeywordExists, 
                keyword.ParametersAreCorrect));
        }
    }
}
