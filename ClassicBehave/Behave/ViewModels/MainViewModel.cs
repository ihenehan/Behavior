using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Linq;
using Behave.Commands;
using Behavior.Common.Models;
using Behave.ViewModels;
using Behavior.Common.Repository;
using System;

namespace Behave.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private IRepository repo;
        private DelegateCommand exitCommand;
        private ObservableCollection<ProjectViewModel> projectViewModels = new ObservableCollection<ProjectViewModel>();

        public ObservableCollection<ProjectViewModel> ProjectViewModels
        {
            get { return projectViewModels; }
            set { projectViewModels = value; }
        }

        public MainViewModel(IRepository repository)
        {
            repo = repository;
        }

        public ICommand ExitCommand
        {
            get
            {
                if (exitCommand == null)
                    exitCommand = new DelegateCommand(Exit);
                
                return exitCommand;
            }
        }

        public void LoadProjects()
        {
            UpdateProjects();
        }
        
        public void UpdateProjects()
        {
            var projects = repo.GetAll<Project>();

            projects.Sort(delegate(Project p1, Project p2) { return p1.Name.CompareTo(p2.Name); });

            foreach (Project project in projects)
                ProjectViewModels.Add(new ProjectViewModel(project));
        }

        public void CreateProject()
        {
            var project = new Project();

            project.Name = "Name";

            var projectViewModel = new ProjectViewModel(project) { IsSelected = true, IsExpanded = true }; 

            ProjectViewModels.Add(projectViewModel);

            repo.Save<Project>(project);

            OnPropertyChanged("ProjectViewModels");
        }

        public void DeleteProject(ProjectViewModel projectViewModel)
        {
            projectViewModel.ShouldDelete = true;

            projectViewModel.IsSelected = false;

            projectViewModel.IsExpanded = false;

            projectViewModel.Children.Clear();

            ProjectViewModels.Remove(projectViewModel);

            repo.Delete<Project>(projectViewModel.Project);
        }

        public void DeleteStory(StoryViewModel storyViewModel)
        {
            storyViewModel.ShouldDelete = true;

            var projectViewModel = ProjectViewModels.First(p => p.Children.Contains(storyViewModel));

            projectViewModel.DeleteStory(storyViewModel);
        }

        public void DeleteScenario(ScenarioViewModel scenarioViewModel)
        {
            scenarioViewModel.ShouldDelete = true;

            var projectViewModel = ProjectViewModels.First(p => p.Children.Any(c => c.Children.Contains(scenarioViewModel)));

            var storyViewModel = projectViewModel.Children.First(c => c.Children.Contains(scenarioViewModel)) as StoryViewModel;

            storyViewModel.DeleteScenario(scenarioViewModel);
        }
        
        private void Exit()
        {
            Application.Current.Shutdown();
        }
    }
}
