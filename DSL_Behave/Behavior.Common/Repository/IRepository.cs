using System;
using System.Collections.Generic;
using Behavior.Common.Models;
using Behavior.Common.Configuration;

namespace Behavior.Common.Repository
{
    public interface IRepository
    {
        string DataPath { get; set; }

        List<Story> GetAllStories(BehaviorConfiguration config);

        List<Scenario> GetItemsByTags(List<Scenario> items, List<string> includeTags, List<string> excludeTags);
    }
}
