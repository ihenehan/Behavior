using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using Behavior.Common.Models;

namespace Behavior.Common.Repository
{
    public class ItemJsonSerializer : ISerializer
    {
        private string dataPath;

        public string DataPath
        {
            get { return dataPath; }
            set { dataPath = value; }
        }

        public ItemJsonSerializer(string dataPath)
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

            var fileName = string.Format("{0}\\{1}\\{2}.jsn", dataPath, item.Type.ToLower(), name);

            var text = JsonConvert.SerializeObject(item, Formatting.Indented);

            File.WriteAllText(fileName, text);
        }

        public void Save<T>(T t, string name, string extension)
        {
            File.WriteAllText(name + extension, JsonConvert.SerializeObject(t, Formatting.Indented));
        }

        public T Get<T>(string itemType, Guid id)
        {
            return ReadFile<T>(string.Format("{0}\\{1}\\{2}.jsn", dataPath, itemType.ToLower(), id.ToString()));
        }

        public T Get<T>(Guid id)
        {
            var text = ReadFile<T>(string.Format("{0}\\{1}\\{2}.jsn", dataPath, GetItemFolder<T>(), id.ToString()));
            
            return text;
        }

        public List<T> GetAll<T>()
        {
            var items = new List<T>();

            var itemPath = string.Format("{0}\\{1}", dataPath, GetItemFolder<T>());

            if (Directory.Exists(itemPath))
            {
                var jsonFiles = Directory.GetFiles(itemPath, "*.jsn", SearchOption.TopDirectoryOnly);

                foreach (string f in jsonFiles)
                    items.Add(ReadFile<T>(f));
            }
            return items;
        }

        public string GetItemFolder<T>()
        {
            return typeof(T).ToString().Split('.').ToList().Last().ToLower();
        }
    }
}
