﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject;
using Behavior.Common.Models;
using Behavior.Common.Repository;
using Behavior.Common.LookUps;
using Behavior.Common.Configuration;
using Behavior.ModelExtensions;
using Behavior.Builders;
using Behavior.Remote.Results;
using Behavior.Reports;
using Behavior.Remote;
using Behavior.Remote.Client;
using Behavior.ArgParser;

namespace Behavior
{
    public class Behavior
    {
        public static StandardKernel Kernel = new StandardKernel();
        public static BehaviorConfiguration Config = new BehaviorConfiguration();
        public static BehaviorArgParser Parser = new BehaviorArgParser();

        static void Main(string[] args)
        {
            Parser.Parse(Config, args);
            

            Kernel = new StandardKernel();

            Kernel.Load(AppDomain.CurrentDomain.GetAssemblies());

            var repo = Kernel.Get<IRepository>();

            var serial = Kernel.Get<ISerializer>();

            var testResult = Kernel.Get<TestRunResult>();

            if (File.Exists("Config.cfg"))
                Config = serial.ReadFile<BehaviorConfiguration>("Config.cfg");

            repo.DataPath = Config.DataPath;

            var stories = repo.GetItemsWithTaggedChildren(repo.GetAll<Story>(), Config.IncludeTags, Config.ExcludeTags).OrderBy(s => s.Name).ToList();

            
            stories.ForEach(s => testResult.StoryResults.Add((s as Story).Run(Kernel.Get<ILauncherClient>())));

            testResult.SetResult();

            
            var testReport = new TestRunReport(testResult);

            testReport.ToFile("TestResults.html");
        }
    }
}
