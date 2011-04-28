﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject.Modules;
using Behavior.Common.Repository;
using Behavior.Remote.Client;
using Behavior.Remote.Results;

namespace Behavior.NinjectModules
{
    public class BehaviorModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ILauncherClient>()
                .To<LauncherClient>();

            Bind<IRepository>()
                .To<ItemRepository>()
                .InSingletonScope()
                .WithPropertyValue("DataPath", GetDataPath());

            Bind<ISerializer>()
                .To<ItemSerializer>()
                .InSingletonScope()
                .WithConstructorArgument("dataPath", GetDataPath());

            Bind<TestRunResult>()
                .To<TestRunResult>()
                .WithConstructorArgument("dataPath", GetDataPath());
        }

        public string GetDataPath()
        {
            return File.ReadAllText("Behavior.data.path");
        }
    }
}
