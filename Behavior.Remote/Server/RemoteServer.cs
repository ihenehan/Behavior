using CookComputing.XmlRpc;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Behavior.Remote.Results;
using Behavior.Remote.Attributes;

namespace Behavior.Remote.Server
{
    public abstract class RemoteServer : XmlRpcListenerService, IRemoteServer    
    {
        private List<object> _boundInstances;
        private List<MethodInfo> _testMethods;

        public List<object> BoundInstances
        {
            get
            {
                if (_boundInstances == null)
                {
                    _boundInstances = new List<object>();

                    var foundTypes = this.GetType().Assembly.GetExportedTypes();

                    var boundTypes = new List<Type>();

                    foreach (Type type in foundTypes)
                    {
                        var attributes = type.GetCustomAttributes(false).ToList();

                        foreach(object o in attributes)
                            if(o.GetType().Equals(typeof(BindFixture)))
                                boundTypes.Add(type);
                    }
                    
                    foreach (Type type in boundTypes)
                        _boundInstances.Add(type.GetConstructor(new Type[] {}).Invoke(new object[] {}));
                }

                return _boundInstances;
            }
        }

        public List<MethodInfo> TestMethods
        {
            get
            {
                if (_testMethods == null)
                {
                    _testMethods = new List<MethodInfo>();

                    foreach (object instance in BoundInstances)
                        foreach (MethodInfo m in instance.GetType().GetMethods())
                            foreach (object attr in m.GetCustomAttributes(false).ToList())
                                if(attr.GetType().Equals(typeof(BindKeyword)))
                                    _testMethods.Add(m);
                }
                return _testMethods;
            }
        }

        public MethodInfo GetMethod(string name)
        {
            return TestMethods.First<MethodInfo>(m => m.Name.Equals(name));
        }

        public List<ParameterInfo> GetParameters(string methodName)
        {
            return GetMethod(methodName).GetParameters().ToList<ParameterInfo>();
        }

        public object GetInstanceContainingMethod(string name)
        {
            return BoundInstances.First(i => i.GetType().GetMethods().Any(m => m.Name.Equals(name)));
        }

        [XmlRpcMethod("get_keyword_names")]
        public object[] get_keyword_names()
        {
            var methodNames = new List<string>();

            TestMethods.ForEach(m => methodNames.Add(m.Name));

            return methodNames.ToArray();
        }

        [XmlRpcMethod("get_keyword_arguments")]
        public object[] get_keyword_arguments(string name)
        {
            var arguments = GetParameters(name);
            
            var args = new List<string>();

            arguments.ForEach(a => args.Add(a.ParameterType.ToString().Replace("System.", "")));

            return args.ToArray();
        }

        [XmlRpcMethod("get_parameter_names")]
        public object[] get_parameter_names(string keywordName)
        {
            var arguments = GetParameters(keywordName);

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
                var instance = GetInstanceContainingMethod(name);

                return instance.GetType().InvokeMember(name, BindingFlags.InvokeMethod | BindingFlags.OptionalParamBinding, null, this, args);
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

        [XmlRpcMethod("get_keyword_documentation")]
        public string get_keyword_documentation(string name)
        {
            return "NotImplemented.";
        }

        public abstract void Dispose();
    }
}
