using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Behavior.Common.Models;

namespace Behavior.Common.Repository
{
    public interface ISerializer
    {
        string DataPath { get; set; }
        void Save<T>(T t, string name);
        //void Save<T>(T t, string name, string extension);
        T ReadFile<T>(string fileName);
        T Get<T>(Guid id);
        //T Get<T>(string itemType, Guid id);
        List<T> GetAll<T>();
    }
}
