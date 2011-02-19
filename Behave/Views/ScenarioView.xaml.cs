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
using Behavior.Common.Models;
using Behave.ViewModels;

namespace Behave.Views
{
    public partial class ScenarioView : UserControl
    {
        public ScenarioView()
        {
            InitializeComponent();
        }

        private void UserControl_LostFocus(object sender, RoutedEventArgs e)
        {
            var model = DataContext as ScenarioViewModel;

            model.Update();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var model = DataContext as ScenarioViewModel;

            model.CreateInteraction();
        }

        private void DeleteInteraction_Click(object sender, RoutedEventArgs e)
        {
            var button = e.OriginalSource as Button;

            if (button != null)
            {
                if (button.Content.ToString().ToLower().Equals("del"))
                {
                    var result = MessageBox.Show("Are you sure you want to delete this interaction?", "Confirm Interaction Delete", MessageBoxButton.OKCancel, MessageBoxImage.Exclamation, MessageBoxResult.Cancel);

                    if (result.Equals(MessageBoxResult.OK))
                    {
                        var model = DataContext as ScenarioViewModel;

                        var interactionViewModel = button.DataContext as InteractionViewModel;

                        model.DeleteInteraction(interactionViewModel.Interaction);
                    }

                    e.Handled = true;
                }

                if (button.Content.ToString().ToLower().Equals("up"))
                {
                    var model = DataContext as ScenarioViewModel;

                    var interactionViewModel = button.DataContext as InteractionViewModel;

                    model.MoveInteractionUp(interactionViewModel.Interaction);
                }

                if (button.Content.ToString().ToLower().Equals("down"))
                {
                    var model = DataContext as ScenarioViewModel;

                    var interactionViewModel = button.DataContext as InteractionViewModel;

                    model.MoveInteractionDown(interactionViewModel.Interaction);
                }
            }
        }
    }
}
