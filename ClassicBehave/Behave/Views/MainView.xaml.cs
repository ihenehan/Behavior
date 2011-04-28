using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Behave.ViewModels;
using Behavior.Common.Models;
using System.Windows.Controls.Primitives;

namespace Behave.Views
{
    public partial class MainView : Window
    {
        public MainView(MainViewModel mainViewModel)
        {
            InitializeComponent();

            mainViewModel.LoadProjects();

            this.DataContext = mainViewModel;
        }

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var treeView = sender as TreeView;

            var viewModel = treeView.SelectedItem;

            Details_StackPanel.Children.Clear();

            if (viewModel != null)
            {
                if (viewModel.GetType().Equals(typeof(ProjectViewModel)))
                    Details_StackPanel.Children.Add(new ProjectView() { DataContext = viewModel });

                if (viewModel.GetType().Equals(typeof(StoryViewModel)))
                    Details_StackPanel.Children.Add(new StoryView() { DataContext = viewModel });

                if (viewModel.GetType().Equals(typeof(ScenarioViewModel)))
                    Details_StackPanel.Children.Add(new ScenarioView() { DataContext = viewModel });
            }
        }

        private void DeleteItem_Click(object sender, RoutedEventArgs e)
        {
            var button = e.OriginalSource as Button;

            if (button != null && button.Content.ToString().ToLower().Contains("delete"))
            {
                if (button.DataContext.GetType().Equals(typeof(ScenarioViewModel)))
                {
                    if (ConfirmDelete("Scenario"))
                    {
                        var model = DataContext as MainViewModel;

                        var scenarioViewModel = button.DataContext as ScenarioViewModel;

                        model.DeleteScenario(scenarioViewModel);

                        Details_StackPanel.Children.Clear();
                    }
                }
                if (button.DataContext.GetType().Equals(typeof(StoryViewModel)))
                {
                    if (ConfirmDelete("Story"))
                    {
                        var model = DataContext as MainViewModel;

                        var storyViewModel = button.DataContext as StoryViewModel;

                        model.DeleteStory(storyViewModel);

                        Details_StackPanel.Children.Clear();
                    }
                }

                if (button.DataContext.GetType().Equals(typeof(ProjectViewModel)))
                {
                    if (ConfirmDelete("Project"))
                    {
                        var model = DataContext as MainViewModel;

                        var projectViewModel = button.DataContext as ProjectViewModel;

                        model.DeleteProject(projectViewModel);

                        Details_StackPanel.Children.Clear();
                    }
                }
            }

            e.Handled = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var model = DataContext as MainViewModel;

            model.CreateProject();
        }

        private bool ConfirmDelete(string item)
        {
            var result = MessageBox.Show("Are you sure you want to delete this " + item.ToLower() + "?", "Confirm " + item + " Delete", MessageBoxButton.OKCancel, MessageBoxImage.Exclamation, MessageBoxResult.Cancel);

            if (result.Equals(MessageBoxResult.OK))
                return true;

            return false;
        }
    }
}
