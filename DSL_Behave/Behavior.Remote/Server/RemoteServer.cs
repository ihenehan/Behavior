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

        public Dictionary<string, object> StoryContext { get; set; }
        public Dictionary<string, object> ScenarioContext { get; set; }

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
                                   attr.GetType().Equals(typeof(BeforeScenario)) ||
                                   attr.GetType().Equals(typeof(AfterScenario)))
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

                if (type.ToLower().Equals("beforescenario"))
                    method = TestMethods.FirstOrDefault(m => m.GetCustomAttributes(false).Any(a => IsBeforeScenario(a, attribute)));

                if (type.ToLower().Equals("afterscenario"))
                    method = TestMethods.FirstOrDefault(m => m.GetCustomAttributes(false).Any(a => IsAfterScenario(a, attribute)));

                if (method != null)
                    return (method as MethodInfo).Name;

                return "";
            }
            catch (Exception e)
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

            try
            {
                var instance = GetInstanceContainingMethod(name);

                var returnObject = instance.GetType().InvokeMember(get_method_by_attribute(name, "step") as string, 
                                                       BindingFlags.InvokeMethod | BindingFlags.OptionalParamBinding, 
                                                       null, 
                                                       this, 
                                                       args);
                if (returnObject == null)
                    return Result.CreatePass("");

                return returnObject as Result;
            }
            catch (Exception ex)
            {
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

        [XmlRpcMethod("reset_scenario_context")]
        public object reset_scenario_context()
        {
            ScenarioContext = new Dictionary<string, object>();
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
            catch (Exception e)
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
            catch (Exception e)
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
            catch (Exception e)
            {
                return false;
            }
        }

        public bool IsBeforeScenario(object attribute, string name)
        {
            try
            {
                if ((attribute as BeforeScenario).Name.Equals(name))
                    return true;
                else
                    return false;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool IsAfterScenario(object attribute, string name)
        {
            try
            {
                if ((attribute as AfterScenario).Name.Equals(name))
                    return true;
                else
                    return false;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public abstract void Dispose();
    }
}
