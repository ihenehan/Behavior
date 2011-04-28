using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Behavior.Common.Models;
using Behavior.Remote.Results;

namespace Behavior.Reports
{
    public class StepReport
    {
        public StepResult StepResult { get; set; }

        public StepReport(StepResult stepResult)
        {
            StepResult = stepResult;
        }

        public string ToHtml()
        {
            var htmlBuilder = new StringBuilder();

            htmlBuilder.Append("<Div>-->" + StepResult.KeywordName + ": " + StepResult.Result.status + "<Div>");

            htmlBuilder.AppendLine(new ResultReport(StepResult.Result).ToHtml());

            htmlBuilder.Append("</Div>");

            return htmlBuilder.ToString();
        }
    }
}
