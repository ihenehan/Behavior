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

            if (story.Criteria.Count.Equals(0))
            {
                storyResult.EndTime = DateTime.Now;
                storyResult.Result = Result.CreateFail("No criteria defined.");
                return storyResult;
            }

            var fixtureUrl = "http://" + Behavior.Config.Host + "/" + Behavior.Config.FixtureContext;

            var httpResult = Result.CreatePass();

            if (!Behavior.Config.IsLocal)
                httpResult = client.RequestFixtureLaunch(fixtureUrl);

            if (httpResult.status.ToLower().Equals("pass"))
            {
                IRemoteClient proxy = CreateProxy(httpResult.retrn.ToString());

                storyResult = RunCriteria(story, storyResult, proxy);

                if(!Behavior.Config.IsLocal)
                    client.DeleteRequest(fixtureUrl);

                storyResult.EndTime = DateTime.Now;
                storyResult.SetResult();

                return storyResult;
            }
            else
            {
                var criterionResult = new CriterionResult();

                criterionResult.StepResults.Add(new StepResult(httpResult));

                criterionResult.SetResult();

                storyResult.CriterionResults.Add(criterionResult);

                storyResult.EndTime = DateTime.Now;
                storyResult.SetResult();

                return storyResult;
            }
        }

        public static IRemoteClient CreateProxy(string url)
        {
            IRemoteClient proxy;

            if (Behavior.Config.IsLocal)
                proxy = new LocalServer(Behavior.Config);
            else
            {
                proxy = XmlRpcProxyGen.Create(typeof(IRemoteClient)) as IRemoteClient;

                proxy.Url = url;
            }

            return proxy;
        }

        public static StoryResult RunCriteria(Story story, StoryResult storyResult, IRemoteClient proxy)
        {
            story.IncludeTags = Behavior.Config.IncludeTags;
            story.ExcludeTags = Behavior.Config.ExcludeTags;
            story.Repository = Behavior.Kernel.Get<IRepository>();

            foreach (Criterion s in story.TestSequence)
            {
                if (s.CriterionType.Equals("Context Reset"))
                {
                    proxy.reset_criterion_context();
                    
                    continue;
                }

                storyResult.CriterionResults.Add(s.Run(proxy));
            }

            return storyResult;
        }
    }
}
