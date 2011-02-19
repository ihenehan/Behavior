using CookComputing.XmlRpc;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Behavior.Remote.Results;

namespace Behavior.Remote.Server
{
    public abstract class RemoteServer : XmlRpcListenerService, IRemoteServer    
    {
        [XmlRpcMethod("get_keyword_names")]
        public object[] get_keyword_names()
        {
            var methods = this.GetType().GetMethods().ToList<MethodInfo>();
            
            var methodNames = new List<string>();

            methods.ForEach(m => methodNames.Add(m.Name));

            return methodNames.ToArray();
        }

        [XmlRpcMethod("get_keyword_arguments")]
        public object[] get_keyword_arguments(string name)
        {
            var arguments = this.GetType().GetMethod(name).GetParameters().ToList<ParameterInfo>();
            
            var args = new List<string>();

            arguments.ForEach(a => args.Add(a.ParameterType.ToString().Replace("System.", "")));

            return args.ToArray();
        }

        [XmlRpcMethod("get_keyword_documentation")]
        public string get_keyword_documentation(string name)
        {
            return "NotImplemented.";
        }

        [XmlRpcMethod("get_parameter_names")]
        public object[] get_parameter_names(string keywordName)
        {
            var arguments = this.GetType().GetMethod(keywordName).GetParameters().ToList<ParameterInfo>();
            
            var args = new List<string>();

            arguments.ForEach(a => args.Add(a.Name.ToString()));

            return args.ToArray();
        }

        [XmlRpcMethod("run_keyword")]
        public object run_keyword(string name, object[] args)
        {
            var result = new Result();

            try
            {
                return this.GetType().InvokeMember(name, BindingFlags.InvokeMethod | BindingFlags.OptionalParamBinding, null, this, args);
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
                return result.Exception(ex.Message, ex.ToString());
            }
        }

        [XmlRpcMethod("Echo")]
        public Result Echo(string message)
        {
            Result result = new Result();

            return result.Pass(message);
        }

        public abstract void Dispose();
    }
}
