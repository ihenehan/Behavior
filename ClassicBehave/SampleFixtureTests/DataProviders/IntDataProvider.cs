using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SampleFixtureTests.TestDataTypes;
using CookComputing.XmlRpc;
using Newtonsoft.Json;

namespace SampleFixtureTests.DataProviders
{
    public class IntDataProvider
    {
        public List<IntTestData> DataList { get; set; }

        public IntDataProvider(int count)
        {
            //DataList = new List<IntTestData>();

            //for (int i = 0; i < count; i++)
            //{
            //    var data = new IntTestData()
            //    {
            //        Index = i,
            //        Tags = new List<string>() {"test", "regression"},
            //        FirstInt = i,
            //        SecondInt = 10 * i,
            //        ExpectedAdd = i + (10 * i),
            //        ExpectedSubtract = i - (10 * i)
            //    };

            //    DataList.Add(data);
            //}

            //var text = JsonConvert.SerializeObject(DataList, Formatting.Indented);

            //File.WriteAllText("IntDataProviderFormatted.txt", text);

            var readText = File.ReadAllText("IntDataProviderFormatted.txt");

            DataList = JsonConvert.DeserializeObject<List<IntTestData>>(readText).Take(count).ToList();
        }
    }
}
