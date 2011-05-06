using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using Behavior.Common.Models;
using Behavior.Common.Parser;
using Behavior.Common.Configuration;

namespace Behavior.Common.Repository
{
    public class ItemSerializer : ISerializer
    {
        private string dataPath;

        public string DataPath
        {
            get { return dataPath; }
            set { dataPath = value; }
        }

        public ItemSerializer(string dataPath)
        {
            DataPath = dataPath;
        }

        public T ReadFile<T>(string fileName)
        {
            var text = File.ReadAllText(fileName);

            return JsonConvert.DeserializeObject<T>(text);
        }

        public void Save<T>(T t, string name)
        {
            var item = t as Item;

            var fileName = string.Format("{0}\\{1}\\{2}.jsn", dataPath, item.GetType().Name.ToLower(), name);

            var text = JsonConvert.SerializeObject(item, Formatting.Indented);

            File.WriteAllText(fileName, text);
        }
        
        public List<Story> GetAllStories(BehaviorConfiguration config)
        {
            var stories = new List<Story>();

            var storyPath = string.Format("{0}\\{1}", dataPath, GetItemFolder<Story>());

            if (Directory.Exists(storyPath))
            {
                var files = Directory.GetFiles(storyPath, "*.story", SearchOption.TopDirectoryOnly);

                foreach(string f in files)
                {
                    var parser = new StoryParser(f);

                    var story = parser.AssembleBlocks();

                    if(story.Scenarios.Any(s => s.ShouldRun(config.IncludeTags, config.ExcludeTags)))
                        stories.Add(story);

                    if (config.IncludeTags.Count == 0)
                        if (story.Scenarios.Count == 0)
                            stories.Add(story);
                }
            }

            //if(saveTables)
            //    SaveNewTableFiles(stories);

            return stories;
        }

        public void SaveNewTableFiles(List<Story> stories)
        {
            foreach (Story f in stories)
            {
                foreach (Scenario s in f.Scenarios)
                {
                    if (s.Table != null)
                    {
                        if(string.IsNullOrEmpty(s.Table.Name))
                            s.Table.Name = DateTime.Now.ToShortDateString().Replace('/', '_');

                        var filePath = string.Format("{0}\\{1}\\{2}.jsn", dataPath, GetItemFolder<Table>(), s.Table.Name.Replace(" ", "_"));

                        if (File.Exists(filePath))
                            File.Delete(filePath);

                        Save<Table>(s.Table, s.Table.Name.Replace(" ", "_"));
                    }
                }
            }
        }

        public string GetItemFolder<T>()
        {
            return typeof(T).ToString().Split('.').ToList().Last().ToLower();
        }
    }
}
