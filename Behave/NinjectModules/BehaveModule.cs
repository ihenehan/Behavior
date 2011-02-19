using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject.Modules;
using Behavior.Common.Models;
using Behavior.Common.Repository;
using Behave.ViewModels;
using Behave.Views;

namespace Behave.NinjectModules
{
    public class BehaveModule : NinjectModule
    {
        public override void Load()
        {
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
            return File.ReadAllText("data.path");
        }
    }
}
