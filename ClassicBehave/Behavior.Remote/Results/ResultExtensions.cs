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

        public static Result Stubbed(this Result result, string error)
        {
            result = clearResult(result);
            result.status = "FAIL";
            result.error = "KEYWORD_STUBBED: " + error;
            return result;
        }

        public static Result PassKeyword(this Result result, object retrn, string xml)
        {
            result = clearResult(result);
            result.retrn = retrn;
            result.status = "PASS";
            result.xml = xml;
            return result;
        }

        public static Result FailKeyword(this Result result, string error, string traceback, string xml)
        {
            result = clearResult(result);
            result.status = "FAIL";
            result.xml = xml;
            result.error = error;
            result.traceback = traceback;
            return result;
        }

        public static Result ExpectedFailure(this Result result, string error, string xml)
        {
            result = clearResult(result);
            result.status = "PASS";
            result.error = "EXPECTED_FAILURE:" + error;
            result.xml = xml;
            return result;
        }

        public static Result FailInteraction(this Result result, string error, string traceback, string xml)
        {
            result = clearResult(result);
            result.status = "FAIL";
            result.xml = xml;
            result.error = error;
            result.traceback = traceback;
            return result;
        }

        public static Result PassInteraction(this Result result, string xml)
        {
            result = clearResult(result);
            result.status = "PASS";
            result.xml = xml;
            return result;
        }

        public static Result PassScenario(this Result result, string xml)
        {
            result = clearResult(result);
            result.status = "PASS";
            result.xml = xml;
            return result;
        }

        public static Result FailScenario(this Result result, string error, string traceback, string xml)
        {
            result = clearResult(result);
            result.status = "FAIL";
            result.xml = xml;
            result.error = error;
            result.traceback = traceback;
            return result;
        }

        public static Result PassStory(this Result result, string xml)
        {
            result = clearResult(result);
            result.status = "PASS";
            result.xml = xml;
            return result;
        }

        public static Result FailStory(this Result result, string error, string traceback, string xml)
        {
            result = clearResult(result);
            result.status = "FAIL";
            result.xml = xml;
            result.error = error;
            result.traceback = traceback;
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

        public static Result KeywordNotFound(this Result result, string keywordError)
        {
            result = clearResult(result);
            result.status = "FAIL";
            result.error = "KEYWORD_NOT_IMPLEMENTED: " + keywordError;
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
