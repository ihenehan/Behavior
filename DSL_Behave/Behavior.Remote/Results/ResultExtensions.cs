using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security;

namespace Behavior.Remote.Results
{
    public static class ResultExtensions
    {
        public static Result Pass(this Result result, object returnObject)
        {
            result = clearResult(result);
            result.status = "PASS";
            result.retrn = returnObject;
            return result;
        }

        public static Result Fail(this Result result, string error)
        {
            result = clearResult(result);
            result.status = "FAIL";
            result.error = error;
            return result;
        }

        public static Result Exception(this Result result, string error, string traceback)
        {
            result = clearResult(result);
            result.status = "FAIL";
            result.error = SecurityElement.Escape(error);
            result.traceback = SecurityElement.Escape(traceback);
            return result;
        }

        private static Result clearResult(Result result)
        {
            result.status = "";
            result.xml = "";
            result.retrn = "";
            result.error = "";
            result.traceback = "";
            return result;
        }
    }
}
