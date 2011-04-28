using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Behavior.Common.Repository;
using Behavior.Common.Models;
using Behavior.Common.Configuration;

namespace Behavior.Remote.Results
{
    public class TestRunResult : Item
    {
        private List<StoryResult> storyResults;

        public Result Result { get; set; }
        public string DataPath { get; set; }
        public List<StoryResult> StoryResults 
        {
            get { return storyResults; }
            set { storyResults = value; }
        }

        public TestRunResult(string dataPath)
        {
            StoryResults = new List<StoryResult>();

            DataPath = dataPath;
        }

        public void SetResult()
        {
            if (StoryResults.Any(sr => sr.Result.status.ToLower().Equals("fail")))
                Result = Result.CreateFail();
            else
                Result = Result.CreatePass();
        }

        public void ToFile(string fileName)
        {
            var serializer = new ItemSerializer(DataPath);

            serializer.Save<TestRunResult>(this, fileName);
        }
    }
}
