using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CookComputing.XmlRpc;
using Behavior.Common.Models;
using Behavior.Remote.Results;
using Behavior.Remote.Client;
using Behavior.Remote.Server;
using Behavior.Common.Configuration;
using System.Reflection;

namespace Behavior.Remote
{
    public class Keyword
    {
        private IRemoteClient proxy;
        private int guiDelay = 1;

        public string Name { get; set; }
        public Dictionary<string, object> Parameters { get; set; }
        public IRemoteServer Fixture { get; set; }
        public bool KeywordExists { get; set; }
        public bool ParametersAreCorrect { get; set; }
        public string KeywordReturn { get; set; }
        public string ReturnName { get; set; }
        public BehaviorConfiguration Config { get; set; }
        
        public IRemoteClient Proxy
        {
            get { return proxy; }
            set { proxy = value; }
        }

        public Keyword(ScenarioStep step, IRemoteClient proxy, BehaviorConfiguration config)
        {
            Name = step.Name;
            Config = config;
            Proxy = proxy;
            Parameters = new Dictionary<string, object>();

            var argCount = 0;

            foreach (object p in step.Parameters)
            {
                if (p.GetType().Equals(typeof(string)))
                    Parameters.Add("arg" + argCount, p as string);
                else
                    Parameters.Add("arg" + argCount, p);

                argCount++;
            }

            if (!int.TryParse(Config.GuiDelay, out guiDelay))
                guiDelay = 0;

            if (guiDelay < 0)
                guiDelay = 0;

            KeywordExists = ValidateKeywordName(Name);

            if(KeywordExists)
                ParametersAreCorrect = ValidateParameters(Name, Parameters);
        }

        public StepResult Run()
        {
            Result result = new Result();

            if (Config.IsLocal)
                result = Proxy.run_keyword(Name, SetParameterValues(Parameters)) as Result;
            else
            {
                var ret = (XmlRpcStruct)Proxy.run_keyword(Name, SetParameterValues(Parameters));
                result = new Result(ret);
            }

            //SetKeywordReturnVar(result);

            Delay.delay(guiDelay);

            return new StepResult(this, result);
        }

        public object[] SetParameterValues(Dictionary<string, object> parameters)
        {
            var values = new List<object>();

            for (int i = 0; i < parameters.Count; i++)
                values.Add(parameters["arg" + i]);

            return values.ToArray<object>();
        }

        public void SetKeywordReturnVar(Result result)
        {
            KeywordReturn = (string)result.retrn;

            var varName = ReturnName;

            if(string.IsNullOrEmpty(varName))                
                varName = "${" + Name + "Return}";

            if (Config.TestVariables.ContainsKey(varName))
                Config.TestVariables[varName] = KeywordReturn;
            else
                Config.TestVariables.Add(varName, KeywordReturn);
        }

        public bool ValidateKeywordName(string name)
        {
            var methodName = Proxy.get_method_by_attribute(name, "step") as string;

            if (string.IsNullOrEmpty(methodName))
                return false;
            
            return true;
        }

        public bool ValidateParameters(string name, Dictionary<string, object> parameters)
        {
            var paramCount = 0;

            paramCount = Proxy.get_keyword_arguments(name).Count();

            return paramCount.Equals(parameters.Count);
        }
    }
}
