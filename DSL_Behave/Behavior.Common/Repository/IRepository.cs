using System;
using System.Collections.Generic;
using Behavior.Common.Models;

namespace Behavior.Common.Repository
{
    public interface IRepository
    {
        string DataPath { get; set; }

        //T Get<T>(string name);

        //void Save<T>(T t);

        List<Story> GetAllStories(bool saveTables);

        //List<Story> GetStoriesWithTaggedChildren(List<Story> items, List<string> includeTags, List<string> excludeTags);

        List<Scenario> GetItemsByTags(List<Scenario> items, List<string> includeTags, List<string> excludeTags);
    }
}
