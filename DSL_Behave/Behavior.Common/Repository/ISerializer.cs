using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Behavior.Common.Models;
using Behavior.Common.Configuration;

namespace Behavior.Common.Repository
{
    public interface ISerializer
    {
        string DataPath { get; set; }

        void Save<T>(T t, string name);

        T ReadFile<T>(string fileName);

        List<Story> GetAllStories(BehaviorConfiguration config);
    }
}
