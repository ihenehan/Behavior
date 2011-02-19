using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Ninject;
using Behavior.Common.Models;
using Behavior.Common.Repository;

namespace Behave.ViewModels
{
    public class StoryViewModel : TreeViewItemViewModel
    {
        public Story Story
        {
            get
            {
                return story;
            }
            set
            {
                story = value;
                base.OnPropertyChanged("Story");
            }
        }

        private Story story;
        private IRepository repo;

        public StoryViewModel(Story story, ProjectViewModel projectViewModel)
            : base(projectViewModel, true)
        {
            Story = story;

            ShouldDelete = false;

            repo = App.kernel.Get<IRepository>();
        }

        protected override void LoadChildren()
        {
            Story.Scenarios = repo.GetByIds<Scenario>(Story.ChildrenIds);

            foreach (Scenario scenario in Scenarios)
                base.Children.Add(new ScenarioViewModel(scenario, this));
        }


        public string Name
        {
            get { return Story.Name; }
            set 
            { 
                Story.Name = value;

                OnPropertyChanged("Name");

                OnPropertyChanged("Header");

                Update();
            }
        }

        public string Header
        {
            get { return "Story: " + Story.Name; }
        }

        public string Description
        {
            get { return Story.Description; }
            set 
            { 
                Story.Description = value;

                OnPropertyChanged("Description");

                Update();
            }
        }

        public string Tags
        {
            get
            {
                string tags = "";

                foreach (string t in Story.Tags)
                    tags = tags + t + ", ";

                return tags.Trim().TrimEnd(',');

            }
            set
            {
                var split = value.Split(',');

                Story.Tags.Clear();

                foreach (string s in split)
                    Story.Tags.Add(s.Trim());

                OnPropertyChanged("Tags");

                Update();
            }
        }

        public void Update()
        {
            if (!ShouldDelete)
                repo.Save<Story>(Story);
        }

        public List<Scenario> Scenarios
        {
            get 
            {
                if (Story.Scenarios.Count == 0)
                    Story.Scenarios = repo.GetByIds<Scenario>(Story.ChildrenIds);

                Story.Scenarios.Sort(delegate(Scenario s1, Scenario s2) { return s1.Name.CompareTo(s2.Name); });

                return Story.Scenarios; 
            }
            set
            {
                Scenarios = value;

                CreateViewModels();

                OnPropertyChanged("Scenarios");
            }
        }

        public void CreateScenario()
        {
            var scenario = new Scenario() { ParentId = Story.Id };

            var interaction = new Interaction() { ParentId = scenario.Id };

            var dataItem = new DataItem() { ParentId = interaction.Id };

            interaction.DataItems.Add(dataItem);

            scenario.Interactions.Add(interaction);

            scenario.Name = "Scenario Name";

            Scenarios.Add(scenario);

            Story.ChildrenIds.Add(scenario.Id);

            var scenarioViewModel = new ScenarioViewModel(scenario, this) { IsSelected = true, IsExpanded = true };

            base.Children.Add(scenarioViewModel);

            Update();
        }

        public void DeleteScenario(ScenarioViewModel scenarioViewModel)
        {
            scenarioViewModel.IsSelected = false;

            scenarioViewModel.IsExpanded = false;

            Scenarios.Remove(scenarioViewModel.Scenario);

            Story.ChildrenIds.Remove(scenarioViewModel.Scenario.Id);

            base.Children.Clear();

            OnPropertyChanged("Scenarios");

            OnPropertyChanged("Children");

            repo.Delete<Scenario>(scenarioViewModel.Scenario);

            repo.Save<Story>(Story);
        }

        public void CreateViewModels()
        {
            base.Children.Clear();

            foreach (Scenario s in Scenarios)
                base.Children.Add(new ScenarioViewModel(s, this));

            OnPropertyChanged("Children");
        }
    }
}
