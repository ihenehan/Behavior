using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Behavior.Common.Models;
using Behavior.Remote.Results;

namespace Behavior.Reports
{
    public class ResultReport
    {
        public Result Result { get; set; }

        public ResultReport(Result result)
        {
            Result = result;
        }

        public string ToHtml()
        {
            var htmlBuilder = new StringBuilder();

            if(!string.IsNullOrEmpty(Result.retrn.ToString()))
                htmlBuilder.AppendLine("<Font Color=green>---->Return: " + Result.retrn.ToString() + "</Font>");

            if(!string.IsNullOrEmpty(Result.error))
                htmlBuilder.AppendLine("<Font Color=red>---->Error: " + Result.error + "</Font>");

            if(!string.IsNullOrEmpty(Result.traceback))
                htmlBuilder.AppendLine("<Font Color=red>---->Stacktrace: " + Result.traceback + "</Font>");

            return htmlBuilder.ToString();
        }
    }
}
