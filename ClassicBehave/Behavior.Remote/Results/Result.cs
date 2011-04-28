using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CookComputing.XmlRpc;

namespace Behavior.Remote.Results
{
    public class Result
    {
        public string status { get; set; }
        public string xml { get; set; }
        public object retrn { get; set; }
        public string error { get; set; }
        public string traceback { get; set; }

        public Result() { }

        public Result(XmlRpcStruct xmlRpcStruct)
        {
            status = (string)xmlRpcStruct["status"];
            retrn = (string)xmlRpcStruct["retrn"];
            xml = (string)xmlRpcStruct["xml"];
            error = (string)xmlRpcStruct["error"];
            traceback = (string)xmlRpcStruct["traceback"];
        }

        public static Result CreatePass()
        {
            return CreatePass(string.Empty);
        }

        public static Result CreatePass(object returnObject)
        {
            var result = ClearResult();
            result.status = "PASS";
            result.retrn = returnObject;
            return result;
        }

        public static Result CreateFail()
        {
            return CreateFail(string.Empty);
        }

        public static Result CreateFail(string error)
        {
            var result = ClearResult();
            result.status = "FAIL";
            result.error = error;
            return result;
        }

        public static Result CreateStubbed(string error)
        {
            var result = ClearResult();
            result.status = "FAIL";
            result.error = "KEYWORD_STUBBED: " + error;
            return result;
        }

        private static Result ClearResult()
        {
            Result result = new Result();
            result.status = "";
            result.xml = "";
            result.retrn = "";
            result.error = "";
            result.traceback = "";
            return result;
        }
    }
}
