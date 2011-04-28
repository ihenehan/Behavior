using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject;
using Behavior.Remote.Results;
using Behavior.ModelExtensions;
using Behavior.Builders;
using Behavior.Common.Models;
using Behavior.Remote.Client;
using CookComputing.XmlRpc;

namespace Behavior.ModelExtensions
{
    public static class StoryExtensions
    {
        private static Item storySetupScenario = new Item();
        private static Item setupScenario = new Item();
        private static List<Item> testScenarios = new List<Item>();
        private static Item teardownScenario = new Item();
        private static Item storyTeardownScenario = new Item();

        public static StoryResult Run(this Story story, ILauncherClient client)
        {
            var storyResult = Behavior.Kernel.Get<StoryResult>();

            storyResult.StoryName = story.Name;

            MapScenarios(story);
            
            testScenarios = Behavior.Kernel.Get<IScenarioBuilder>().Build(testScenarios);

            var fixtureUrl = "http://" + Behavior.Config.Host + "/" + Behavior.Config.FixtureContext;

            var httpResult = client.RequestFixtureLaunch(fixtureUrl, 10);


            if (httpResult.status.ToLower().Equals("pass"))
            {
                var proxy = XmlRpcProxyGen.Create(typeof(IRemoteClient)) as IRemoteClient;

                proxy.Url = httpResult.retrn.ToString();

                storyResult = RunScenarios(storyResult, proxy);

                client.StopFixtureServer(fixtureUrl);

                storyResult.SetResult();

                return storyResult;
            }
            else
            {
                var scenarioResult = new ScenarioResult();

                scenarioResult.InteractionResults.Add(new InteractionResult(httpResult));

                scenarioResult.SetResult();

                storyResult.ScenarioResults.Add(scenarioResult);

                storyResult.SetResult();

                return storyResult;
            }
        }

        public static void MapScenarios(Story story)
        {
            storySetupScenario = story.Children.FirstOrDefault(i => IsScenarioType(i, "Story Setup"));

            setupScenario = story.Children.FirstOrDefault(i => IsScenarioType(i, "Test Setup"));

            testScenarios = story.Children.Where(i => IsScenarioType(i, "Test")).ToList();

            teardownScenario = story.Children.FirstOrDefault(i => IsScenarioType(i, "Test Teardown"));

            storyTeardownScenario = story.Children.FirstOrDefault(i => IsScenarioType(i, "Story Teardown"));
        }

        public static bool IsScenarioType(Item i, string scenarioType)
        {
            var s = i as Scenario;

            if (s.ScenarioType.Equals(scenarioType))
                return true;

            return false;
        }

        public static StoryResult RunScenarios(StoryResult storyResult, IRemoteClient proxy)
        {
            testScenarios = testScenarios.OrderBy(d => d.Name).ToList();

            if (storySetupScenario != null)
                storyResult.ScenarioResults.Add((storySetupScenario as Scenario).Run(proxy));

            foreach (Scenario s in testScenarios)
            {
                if (setupScenario != null)
                    storyResult.ScenarioResults.Add((setupScenario as Scenario).Run(proxy));

                if (testScenarios != null && testScenarios.Count > 0)
                    storyResult.ScenarioResults.Add(s.Run(proxy));

                if (teardownScenario != null)
                    storyResult.ScenarioResults.Add((teardownScenario as Scenario).Run(proxy));
            }

            if (storyTeardownScenario != null)
                storyResult.ScenarioResults.Add((storyTeardownScenario as Scenario).Run(proxy));

            return storyResult;
        }
    }
}
