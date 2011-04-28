using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Ninject;
using Behavior.Common.Models;
using Behavior.Common.Repository;

namespace Behave.ViewModels
{
    public class ProjectViewModel : TreeViewItemViewModel
    {
        public Project Project 
        { 
            get
            {
                return project;
            }
            set
            {
                project = value;

                base.OnPropertyChanged("Project");
            }
        }

        private Project project;
        private IRepository repo;

        public ProjectViewModel(Project project)
            : base(null, true)
        {
            Project = project;

            ShouldDelete = false;

            repo = App.kernel.Get<IRepository>();
        }

        protected override void LoadChildren()
        {
            foreach (Story story in Stories)
                base.Children.Add(new StoryViewModel(story, this));
        }

        public string Name
        {
            get { return Project.Name; }
            set 
            { 
                Project.Name = value;

                OnPropertyChanged("Name");

                OnPropertyChanged("Header");

                Update();
            }
        }

        public string Header
        {
            get { return "Project: " + Project.Name; }
        }

        public string Description
        {
            get { return Project.Description; }
            set 
            { 
                Project.Description = value;

                OnPropertyChanged("Description");

                Update();
            }
        }

        public string Tags
        {
            get 
            { 
                string tags = "";

                foreach (string t in Project.Tags)
                    tags = tags + t + ", ";

                return tags.Trim().TrimEnd(',');
 
            }
            set 
            {
                var split = value.Split(',');

                Project.Tags.Clear();

                foreach(string s in split)
                    Project.Tags.Add(s.Trim());

                OnPropertyChanged("Tags");

                Update();
            }
        }

        public void Update()
        {
            if(!ShouldDelete)
                repo.Save<Project>(Project);
        }

        public List<Story> Stories
        {
            get 
            {
                if (Project.Stories.Count == 0)
                    Project.Stories = repo.GetByIds<Story>(Project.ChildrenIds);

                Project.Stories.Sort(delegate(Story s1, Story s2) { return s1.Name.CompareTo(s2.Name); });

                return Project.Stories;

            }
            set
            {
                Project.Stories = value;

                CreateViewModels();

                OnPropertyChanged("Stories");
            }
        }

        public void CreateStory()
        {
            var story = new Story() { ParentId = Project.Id };

            story.Name = "Name";

            Stories.Add(story);

            repo.Save<Story>(story);

            Project.ChildrenIds.Add(story.Id);

            var storyViewModel = new StoryViewModel(story, this) { IsSelected = true, IsExpanded = true };

            Children.Add(storyViewModel);

            Update();
        }

        public void DeleteStory(StoryViewModel storyViewModel)
        {
            storyViewModel.IsSelected = false;

            storyViewModel.IsExpanded = false;

            Stories.Remove(storyViewModel.Story);

            Project.ChildrenIds.Remove(storyViewModel.Story.Id);

            base.Children.Clear();

            OnPropertyChanged("Stories");

            OnPropertyChanged("Children");

            repo.Delete<Story>(storyViewModel.Story);

            repo.Save<Project>(Project);
        }

        public void CreateViewModels()
        {
            base.Children.Clear();

            foreach (Story s in Stories)
                base.Children.Add(new StoryViewModel(s, this));

            OnPropertyChanged("Children");
        }
    }
}
