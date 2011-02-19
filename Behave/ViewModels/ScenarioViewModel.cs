using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Ninject;
using Behavior.Common.Models;
using Behavior.Common.Repository;
using Behavior.Common.LookUps;

namespace Behave.ViewModels
{
    public class ScenarioViewModel : TreeViewItemViewModel
    {
        public ObservableCollection<InteractionViewModel> InteractionViewModels
        {
            get 
            {
                interactionViewModels.OrderBy(ivm => ivm.Interaction.Index);

                return interactionViewModels; 
            }
            set 
            { 
                interactionViewModels = value; 
            }
        }

        private ObservableCollection<InteractionViewModel> interactionViewModels = new ObservableCollection<InteractionViewModel>();

        public Scenario Scenario
        {
            get
            {
                return scenario;
            }
            set
            {
                scenario = value;

                OnPropertyChanged("Scenario");
            }
        }

        private Scenario scenario;
        private IRepository repo;

        public ScenarioViewModel(Scenario scenario, StoryViewModel storyViewModel)
            : base(storyViewModel, true)
        {
            Scenario = scenario;

            ShouldDelete = false;

            CreateViewModels();

            repo = App.kernel.Get<IRepository>();
        }

        public bool Expanded
        {
            get { return Scenario.IsExpanded; }
            set 
            { 
                Scenario.IsExpanded = value;

                OnPropertyChanged("Expanded");
            }
        }

        public string Name
        {
            get { return Scenario.Name; }
            set 
            { 
                Scenario.Name = value;

                OnPropertyChanged("Name");

                OnPropertyChanged("Header");

                Update();
            }
        }

        public string Header
        {
            get { return Scenario.Name; }
        }

        public string Description
        {
            get { return Scenario.Description; }
            set 
            { 
                Scenario.Description = value;

                OnPropertyChanged("Description");

                Update();
            }
        }

        public bool Selected
        {
            get { return Scenario.Selected; }
            set
            {
                Scenario.Selected = value;

                OnPropertyChanged("Selected");

                Update();
            }
        }

        public List<string> ScenarioTypes
        {
            get { return StringLookUps.ScenarioTypes; }
        }

        public string ScenarioType
        {
            get { return Scenario.ScenarioType; }
            set
            {
                Scenario.ScenarioType = value;

                OnPropertyChanged("ScenarioType");

                Update();
            }
        }

        public bool ExpectFailure
        {
            get { return Scenario.ExpectFailure; }
            set 
            { 
                Scenario.ExpectFailure = value;

                OnPropertyChanged("ExpectFail");

                Update();
            }
        }

        public string ExpectedResult
        {
            get { return Scenario.ExpectedResult; }
            set 
            { 
                Scenario.ExpectedResult = value;

                OnPropertyChanged("ExpectedResult");

                Update();
            }
        }

        public string Tags
        {
            get
            {
                string tags = "";

                Scenario.Tags.ForEach(t => tags = tags + t + ", ");

                return tags.TrimEnd(", ".ToCharArray());
            }
            set
            {
                Scenario.Tags = new List<string>();

                string[] tags = value.Split(',');

                foreach (string t in tags)
                    Scenario.Tags.Add(t.Trim());    

                OnPropertyChanged("Tags");

                Update();
            }
        }

        public void Update()
        {
            if (!ShouldDelete)
                repo.Save<Scenario>(Scenario);
        }

        public List<Interaction> Interactions
        {
            get 
            {
                Scenario.Interactions.Sort(delegate(Interaction i1, Interaction i2) { return i1.Index.CompareTo(i2.Index); });

                return Scenario.Interactions; 
            }
            set
            {
                Scenario.Interactions = value;

                Scenario.Interactions = Scenario.Interactions.OrderBy(i => i.Index).ToList();

                CreateViewModels();

                Update();

                OnPropertyChanged("Interactions");
            }
        }

        public void CreateInteraction()
        {
            var index = GetLastIndex();

            var interaction = new Interaction() { ParentId=Scenario.Id, Index=index };

            var dataItem = new DataItem() { ParentId = interaction.Id };

            interaction.DataItems.Add(dataItem);

            Interactions.Add(interaction);

            CreateViewModels();

            Update();
        }

        public void DeleteInteraction(Interaction interaction)
        {
            if (Scenario.Interactions.Count > 1)
            {
                InteractionViewModels.Remove(InteractionViewModels.FirstOrDefault(d => d.Interaction.Equals(interaction)));

                Interactions.Remove(interaction);

                OnPropertyChanged("Interactions");
            }
        }

        public void CreateViewModels()
        {
            if (InteractionViewModels == null)
                InteractionViewModels = new ObservableCollection<InteractionViewModel>();

            InteractionViewModels.Clear();

            foreach (Interaction i in Interactions)
                InteractionViewModels.Add(new InteractionViewModel(i));

            interactionViewModels.OrderBy(ivm => ivm.Interaction.Index);

            OnPropertyChanged("InteractionViewModels");
        }

        public int GetLastIndex()
        {
            if (InteractionViewModels.Count > 0)
            {
                InteractionViewModels.OrderBy(ivm => ivm.Interaction.Index);

                return InteractionViewModels.Last().Interaction.Index + 1;
            }

            return 0;
        }

        public void ReIndex()
        {
            Interactions = Interactions.OrderBy(i => i.Index).ToList();

            int index = 0;

            foreach (Interaction i in Interactions)
            {
                i.IsLast = false;

                i.Index = index;

                index++;
            }

            Interactions.Last().IsLast = true;

            CreateViewModels();

            OnPropertyChanged("Interactions");
        }

        public void MoveInteractionUp(Interaction interaction)
        {
            if (interaction.Index == 0)
                return;

            var newIndex = interaction.Index - 1;

            foreach (Interaction i in Interactions)
                if (i.Index == newIndex)
                {
                    interaction.Index = newIndex;
                    
                    i.Index++;

                    break;
                }

            ReIndex();
        }

        public void MoveInteractionDown(Interaction interaction)
        {
            if (interaction.IsLast)
                return;

            var newIndex = interaction.Index + 1;

            foreach (Interaction i in Interactions)
                if (i.Index == newIndex)
                {
                    interaction.Index = newIndex;

                    i.Index--;

                    break;
                }

            ReIndex();
        }
    }
}
