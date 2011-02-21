using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject.Modules;
using Behavior.Builders;
using Behavior.Common.Repository;
using Behavior.Remote.Client;

namespace Behavior.NinjectModules
{
    public class BehaviorModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IScenarioBuilder>()
                .To<ScenarioBuilder>();

            Bind<ILauncherClient>()
                .To<LauncherClient>();

            Bind<IRepository>()
                .To<ItemRepository>()
                .InSingletonScope()
                .WithPropertyValue("DataPath", GetDataPath());

            Bind<ISerializer>()
                .To<ItemJsonSerializer>()
                .InSingletonScope()
                .WithConstructorArgument("dataPath", GetDataPath());
        }

        public string GetDataPath()
        {
            return File.ReadAllText("Behavior.data.path");
        }
    }
}
