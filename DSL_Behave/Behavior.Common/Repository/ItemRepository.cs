using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Behavior.Common.Models;
using Behavior.Common.Configuration;

namespace Behavior.Common.Repository
{
    public class ItemRepository : IRepository
    {
        private string dataPath = "";
        private ISerializer itemSerializer;

        public string DataPath
        {
            get { return dataPath; }
            set
            {
                dataPath = value;

                if (!Directory.Exists(dataPath))
                    Directory.CreateDirectory(dataPath);

                itemSerializer.DataPath = dataPath;
            }
        }

        public ItemRepository(ISerializer serializer)
        {
            itemSerializer = serializer;
        }

        public List<Story> GetAllStories(BehaviorConfiguration config)
        {
            return itemSerializer.GetAllStories(config);
        }

        public List<Criterion> GetItemsByTags(List<Criterion> criteria, List<string> includeTags, List<string> excludeTags)
        {
            if (includeTags != null && includeTags.Count > 0)
            {
                var includeItems = criteria.Where(s => s.Tags.Any(t => includeTags.Any(i => i.ToLower().Equals(t.Name.ToLower())))).ToList();

                if (excludeTags != null && excludeTags.Count > 0)
                    return includeItems.Where(s => !excludeTags.Any(e => s.Tags.Any(t => t.Name.ToLower().Equals(e.ToLower())))).ToList();

                return includeItems;
            }

            return new List<Criterion>();
        }
    }
}
