using System;
using System.Collections.Generic;
using Behavior.Common.Models;

namespace Behavior.Common.Repository
{
    public interface IRepository
    {
        string DataPath { get; set; }

        Item Create<T>() where T : class, new();
        
        void Delete<T>(T t);
        
        Item Get<T>(Guid id);

        List<T> GetAll<T>();

        List<T> GetByIds<T>(List<Guid> ids);

        void Save<T>(T t);

        List<Story> GetItemsWithTaggedChildren(List<Story> items, List<string> includeTags, List<string> excludeTags);

        List<Scenario> GetItemsByTags(List<Scenario> items, List<string> includeTags, List<string> excludeTags);
    }
}
