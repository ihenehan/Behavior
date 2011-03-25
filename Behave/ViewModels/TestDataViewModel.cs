using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Behavior.Common.Models;
using Behavior.Common.Repository;

namespace Behave.ViewModels
{
    public class TestDataViewModel : ViewModelBase
    {
        private IRepository repo;
        private List<TestData> testDataRows;
        private Scenario scenario;

        public List<TestData> TestDataRows
        {
            get { return testDataRows; }
            set 
            { 
                testDataRows = value;
                OnPropertyChanged("TestDataRows");
            }
        }

        public TestDataViewModel(IRepository repository, Scenario scenario)
        {
            this.repo = repository;
            
            this.scenario = scenario;

            TestDataRows = scenario.TestDataRows;
        }

        public List<string> HeaderRow
        {
            get
            {
                var firstRow = TestDataRows.First();

                return firstRow.Keys.ToList();
            }
        }
    }
}
