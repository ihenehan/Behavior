using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject;
using Behavior.Remote.Results;
using Behavior.ModelExtensions;
using Behavior.Common.Models;
using Behavior.Remote.Client;
using CookComputing.XmlRpc;
using Behavior.Common.Parser;
using System.Reflection;
using Behavior.Remote.Server;
using Behavior.Common.Repository;

namespace Behavior.ModelExtensions
{
    public static class StoryExtensions
    {
        public static StoryResult Run(this Story story, ILauncherClient client)
        {
            var storyResult = Behavior.Kernel.Get<StoryResult>();

            storyResult.Story = story;
            storyResult.StartTime = DateTime.Now;

            var fixtureUrl = "http://" + Behavior.Config.Host + "/" + Behavior.Config.FixtureContext;

            var httpResult = Result.CreatePass();

            if (!Behavior.Config.IsLocal)
                httpResult = client.RequestFixtureLaunch(fixtureUrl, 10);

            if (httpResult.status.ToLower().Equals("pass"))
            {
                IRemoteClient proxy = CreateProxy(httpResult.retrn.ToString());

                storyResult = RunScenarios(story, storyResult, proxy);

                if(!Behavior.Config.IsLocal)
                    client.StopFixtureServer(fixtureUrl);

                storyResult.EndTime = DateTime.Now;
                storyResult.SetResult();

                return storyResult;
            }
            else
            {
                var scenarioResult = new ScenarioResult();

                scenarioResult.StepResults.Add(new StepResult(httpResult));

                scenarioResult.SetResult();

                storyResult.ScenarioResults.Add(scenarioResult);

                storyResult.EndTime = DateTime.Now;
                storyResult.SetResult();

                return storyResult;
            }
        }

        public static IRemoteClient CreateProxy(string url)
        {
            IRemoteClient proxy;

            if (Behavior.Config.IsLocal)
                proxy = new RemoteServerProxy(Behavior.Config);
            else
            {
                proxy = XmlRpcProxyGen.Create(typeof(IRemoteClient)) as IRemoteClient;

                proxy.Url = url;
            }

            return proxy;
        }

        public static StoryResult RunScenarios(Story story, StoryResult storyResult, IRemoteClient proxy)
        {
            story.IncludeTags = Behavior.Config.IncludeTags;
            story.ExcludeTags = Behavior.Config.ExcludeTags;
            story.Repository = Behavior.Kernel.Get<IRepository>();

            foreach (Scenario s in story.TestSequence)
            {
                if (s.ScenarioType.Equals("Context Reset"))
                {
                    proxy.reset_scenario_context();
                    
                    continue;
                }

                storyResult.ScenarioResults.Add(s.Run(proxy));
            }

            return storyResult;
        }
    }
}
