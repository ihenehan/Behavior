﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Behavior.Common.Configuration;

namespace Behavior.ArgParser
{
    public class BehaviorArgParser
    {
        private ArgDictionary<BehaviorConfiguration> map;

        public void Parse(BehaviorConfiguration config, params string[] args)
        {
            map = new ArgDictionary<BehaviorConfiguration>(config)
                          {
                              {"resultfile", (b,a) => b.ResultFile = Split(a)},
                              {"include", (b,a) => b.IncludeTags = Split(a).Split(",".ToCharArray()).ToList()},
                              {"exclude", (b,a) => b.ExcludeTags = Split(a).Split(",".ToCharArray()).ToList()},
                              {"datapath", (b,a) => b.DataPath = Split(a)},
                              {"host", (b,a) => b.Host = Split(a)},
                              {"fixture", (b,a) => b.FixtureType = Split(a)},
                              {"context", (b,a) => b.FixtureContext = Split(a)},
                              {"delay", (b,a) => b.GuiDelay = Split(a)},
                              {"islocal", (b,a) => b.IsLocal = SplitBool(a)},
                              {"localfixtureassembly", (b,a) => b.LocalFixtureAssembly = Split(a)},
                              {"localfixturename", (b,a) => b.LocalFixtureName = Split(a)},
                              {"savetables", (b,a) => b.SaveTables = SplitBool(a)}
                          };

            map.ParseAndExecute(args);

            AddTestVariables(config, args);
        }

        public string Split(string arg)
        {
            return arg.Split("=".ToCharArray())[1];
        }

        public bool SplitBool(string arg)
        {
            return Split(arg).ToLower().Equals("true");
        }

        public void AddTestVariables(BehaviorConfiguration config, string[] args)
        {
            var vars = args.ToList().Where(a => a.StartsWith("${")).ToList();

            config.TestVariables = new Dictionary<string, string>();

            vars.ForEach(v => config.TestVariables.Add(v.Split('=')[0], v.Split('=')[1]));
        }
    }
}
