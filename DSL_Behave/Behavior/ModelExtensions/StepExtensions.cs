using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject;
using CookComputing.XmlRpc;
using Behavior.Common.Models;
using Behavior.Remote;
using Behavior.Remote.Results;
using Behavior.Remote.Client;

namespace Behavior.ModelExtensions
{
    public static class StepExtensions
    {
        public static StepResult Run(this ScenarioStep step, IRemoteClient proxy)
        {
            var keyword = new Keyword(step, proxy, Behavior.Config) ;

            var result = new StepResult();

            if (keyword.KeywordExists && keyword.ParametersAreCorrect)
            {
                var ret = keyword.Run();

                ret.KeywordName = step.InsertValues();

                return ret;
            }
            
            return new StepResult(keyword);
        }

        public static string InsertValues(this ScenarioStep step)
        {
            if (step.Name.Contains("[arg]"))
            {
                var outstring = "";

                var split = step.Name.Replace("[", "!").Replace("]", "!").Split('!').ToList();

                split.Remove("arg");

                var count = 0;

                foreach (object p in step.Parameters)
                {
                    outstring = outstring + split[count] + "\"" + p.ToString() + "\"";

                    count++;
                }

                return outstring + split.Last();
            }

            return step.Name;
        }
    }
}
