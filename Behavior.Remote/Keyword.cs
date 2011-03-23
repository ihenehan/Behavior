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

namespace Behavior.Remote
{
    public class Keyword
    {
        private IRemoteClient proxy;
        private int guiDelay = 1;

        public string Name { get; set; }
        public Dictionary<string, string> Parameters { get; set; }
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

        public Keyword(Interaction interaction, IRemoteClient proxy, BehaviorConfiguration config)
        {
            Name = interaction.Name;
            Config = config;
            Proxy = proxy;
            Parameters = new Dictionary<string, string>();

            interaction.Children.ForEach(d => Parameters.Add((d as DataItem).Name, (d as DataItem).Data));

            if (!int.TryParse(Config.GuiDelay, out guiDelay))
                guiDelay = 0;

            if (guiDelay < 0)
                guiDelay = 0;

            KeywordExists = ValidateKeywordName(Name);

            if(KeywordExists)
                ParametersAreCorrect = ValidateParameters(Name, Parameters);
        }

        public InteractionResult Run()
        {
            Result result = new Result();

            if(Config.IsLocal)
                result = Fixture.run_keyword(Name, SetParameterValues(Parameters)) as Result;
            else
            {
                var ret = (XmlRpcStruct)Proxy.run_keyword(Name, SetParameterValues(Parameters));
                result = new Result(ret);
            }

            SetKeywordReturnVar(result);
            
            Delay.delay(guiDelay);

            return new InteractionResult(this, result);
        }

        public object[] SetParameterValues(Dictionary<string, string> parameters)
        {
            var values = new List<object>();

            foreach (string s in Proxy.get_parameter_names(Name))
                foreach (KeyValuePair<string, string> p in Parameters)
                    if (p.Key.Equals(s))
                        values.Add(Config.InsertTestVariables(p.Value));

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
            var namesList = new List<string>();
            var actualNames = Proxy.get_keyword_names();

            foreach (string s in actualNames)
                namesList.Add(s);
            
            return namesList.Any(n => n.Equals(name));
        }

        public bool ValidateParameters(string name, Dictionary<string, string> parameters)
        {
            var paramsList = new List<string>();

            var actualParams = Proxy.get_parameter_names(name);

            foreach (string s in actualParams)
                paramsList.Add(s);

            return paramsList.All(p => parameters.ContainsKey(p));
        }
    }
}
