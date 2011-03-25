using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Behavior.Common.Models;

namespace Behavior.Common.Repository
{
    public class ItemRepository : IRepository
    {
        private string dataPath = "";
        private string projectsPath;
        private string storiesPath;
        private string scenariosPath;
        private ISerializer itemSerializer;

        public string DataPath
        {
            get { return dataPath; }
            set
            {
                dataPath = value;

                if (!Directory.Exists(dataPath))
                    Directory.CreateDirectory(dataPath);

                projectsPath = dataPath + "\\project";

                if (!Directory.Exists(projectsPath))
                    Directory.CreateDirectory(projectsPath);

                storiesPath = dataPath + "\\story";

                if (!Directory.Exists(storiesPath))
                    Directory.CreateDirectory(storiesPath);

                scenariosPath = dataPath + "\\scenario";

                if (!Directory.Exists(scenariosPath))
                    Directory.CreateDirectory(scenariosPath);

                itemSerializer.DataPath = dataPath;
            }
        }

        public ItemRepository(ISerializer serializer)
        {
            itemSerializer = serializer;
        }

        public Item Create<T>() where T : class, new()
        {
            var item = new T() as Item;

            Save(item);

            return item;
        }

        public void Delete<T>(T t)
        {
            var item = t as Item;

            DeleteFile(item.Id, item.Type);
        }

        public void DeleteFile(Guid id, string type)
        {
            var path = dataPath + "\\" + type.ToLower() + "\\" + id.ToString() + ".jsn";

            if (File.Exists(path))
                File.Delete(path);
        }

        public void Save<T>(T t)
        {
            var item = t as Item;

            itemSerializer.Save(t, item.Id.ToString());
        }

        public Item Get<T>(Guid id)
        {
            return itemSerializer.Get<T>(id) as Item;
        }

        public List<T> GetAll<T>()
        {
            return itemSerializer.GetAll<T>();
        }

        public List<T> GetByIds<T>(List<Guid> ids)
        {
            var items = new List<T>();

            ids.ForEach(i => items.Add(itemSerializer.Get<T>(i)));
            
            return items;
        }
        
        public List<Story> GetItemsWithTaggedChildren(List<Story> stories, List<string> includeTags, List<string> excludeTags)
        {
            var taggedItems = new List<Story>();

            stories.ForEach(s => s.Scenarios = GetByIds<Scenario>(s.ChildrenIds));

            foreach (Story s in stories)
            {
                var taggedChildren = GetItemsByTags(s.Scenarios, includeTags, excludeTags);

                if (taggedChildren.Count > 0)
                {
                    taggedItems.Add(s);
                    s.Scenarios = taggedChildren;
                }
            }

            return taggedItems;
        }

        public List<Scenario> GetItemsByTags(List<Scenario> scenarios, List<string> includeTags, List<string> excludeTags)
        {
            if (includeTags != null && includeTags.Count > 0)
            {
                var includeItems = scenarios.Where(s => includeTags.Any(i => s.Tags.Any(t => t.ToLower().Equals(i.ToLower())))).ToList();

                if (excludeTags != null && excludeTags.Count > 0)
                    return includeItems.Where(s => !excludeTags.Any(e => s.Tags.Any(t => t.ToLower().Equals(e.ToLower())))).ToList();

                return includeItems;
            }

            return new List<Scenario>();
        }
    }
}
