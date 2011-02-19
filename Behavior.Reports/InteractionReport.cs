using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Behavior.Common.Models;
using Behavior.Remote.Results;

namespace Behavior.Reports
{
    public class InteractionReport
    {
        public InteractionResult InteractionResult { get; set; }

        public InteractionReport(InteractionResult interactionResult)
        {
            InteractionResult = interactionResult;
        }

        public string ToHtml()
        {
            var htmlBuilder = new StringBuilder();

            htmlBuilder.Append("<Div>-->" + InteractionResult.KeywordName + ": " + InteractionResult.Result.status + "<Div>");

            htmlBuilder.AppendLine(new ResultReport(InteractionResult.Result).ToHtml());

            htmlBuilder.Append("</Div>");

            return htmlBuilder.ToString();
        }
    }
}
