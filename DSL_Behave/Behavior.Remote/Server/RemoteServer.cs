using CookComputing.XmlRpc;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Behavior.Common.Models;
using Behavior.Remote.Results;
using Behavior.Remote.Attributes;
using NUnit.Framework;

namespace Behavior.Remote.Server
{
    public abstract class RemoteServer : XmlRpcListenerService, IRemoteServer    
    {
        private List<object> _boundInstances;
        private List<MethodInfo> _testMethods;

        public Dictionary<string, object> StoryContext { get; set; }
        public Dictionary<string, object> CriterionContext { get; set; }

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
                            if(o.GetType().Equals(typeof(Fixture)))
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
                                if(attr.GetType().Equals(typeof(Step)) ||
                                   attr.GetType().Equals(typeof(BeforeStory)) ||
                                   attr.GetType().Equals(typeof(AfterStory)) ||
                                   attr.GetType().Equals(typeof(BeforeCriterion)) ||
                                   attr.GetType().Equals(typeof(AfterCriterion)))
                                    _testMethods.Add(m);
                }
                return _testMethods;
            }
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

        [XmlRpcMethod("get_method_by_attribute")]
        public object get_method_by_attribute(string attribute, string type)
        {
            try
            {
                var method = new object();

                if (type.ToLower().Equals("step"))
                    method = TestMethods.FirstOrDefault(m => m.GetCustomAttributes(false).Any(a => IsStep(a, attribute)));

                if (type.ToLower().Equals("beforestory"))
                    method = TestMethods.FirstOrDefault(m => m.GetCustomAttributes(false).Any(a => IsBeforeStory(a, attribute)));

                if (type.ToLower().Equals("afterstory"))
                    method = TestMethods.FirstOrDefault(m => m.GetCustomAttributes(false).Any(a => IsAfterStory(a, attribute)));

                if (type.ToLower().Equals("beforecriterion"))
                    method = TestMethods.FirstOrDefault(m => m.GetCustomAttributes(false).Any(a => IsBeforeCriterion(a, attribute)));

                if (type.ToLower().Equals("aftercriterion"))
                    method = TestMethods.FirstOrDefault(m => m.GetCustomAttributes(false).Any(a => IsAfterCriterion(a, attribute)));

                if (method != null)
                    return (method as MethodInfo).Name;

                return "";
            }
            catch (Exception)
            {
                return "";
            }
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

            args = MarshallArgs(args);

            try
            {
                var instance = GetInstanceContainingMethod(name);

                var returnObject = instance.GetType().InvokeMember(get_method_by_attribute(name, "step") as string, 
                                                       BindingFlags.InvokeMethod | BindingFlags.OptionalParamBinding, 
                                                       null, 
                                                       instance, 
                                                       args);
                if (returnObject == null)
                    return Result.CreatePass("");

                return returnObject as Result;
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    return result.Exception(ex.InnerException.Message, "");

                return result.Exception(ex.Message, ex.ToString());
            }
        }

        [XmlRpcMethod("set_story_context")]
        public void set_story_context(Dictionary<string, object> context)
        {
            StoryContext = context;
        }

        [XmlRpcMethod("get_story_context")]
        public object get_story_context()
        {
            return StoryContext;
        }

        [XmlRpcMethod("reset_criterion_context")]
        public object reset_criterion_context()
        {
            CriterionContext = new Dictionary<string, object>();
            return Result.CreatePass();
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

        public object[] MarshallArgs(object[] args)
        {
            var processedArgs = new List<object>();

            foreach (object o in args)
                if (o.GetType().Equals(typeof(XmlRpcStruct)))
                {
                    if ((o as XmlRpcStruct)["Type"].Equals("Table"))
                        processedArgs.Add(new Table(o as XmlRpcStruct));
                }
                else
                    processedArgs.Add(o);

            return processedArgs.ToArray();
        }

        public MethodInfo GetMethod(string name)
        {
            return TestMethods.First<MethodInfo>(m => m.Name.Equals(name));
        }

        public List<ParameterInfo> GetParameters(string methodName)
        {
            return GetMethod(get_method_by_attribute(methodName, "step") as string).GetParameters().ToList<ParameterInfo>();
        }

        public object GetInstanceContainingMethod(string name)
        {
            return BoundInstances.First(i => i.GetType().GetMethods().Any(m => m.Name.Equals(get_method_by_attribute(name, "step"))));
        }

        public bool IsStep(object attribute, string name)
        {
            try
            {
                if (attribute.GetType().Equals(typeof(Step)))
                {
                    if ((attribute as Step).Name.Equals(name))
                        return true;
                    else
                        return false;
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool IsBeforeStory(object attribute, string name)
        {
            try
            {
                if ((attribute as BeforeStory).Name.Equals(name))
                    return true;
                else
                    return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool IsAfterStory(object attribute, string name)
        {
            try
            {
                if ((attribute as AfterStory).Name.Equals(name))
                    return true;
                else
                    return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool IsBeforeCriterion(object attribute, string name)
        {
            try
            {
                if ((attribute as BeforeCriterion).Name.Equals(name))
                    return true;
                else
                    return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool IsAfterCriterion(object attribute, string name)
        {
            try
            {
                if ((attribute as AfterCriterion).Name.Equals(name))
                    return true;
                else
                    return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public abstract void Dispose();
    }
}
