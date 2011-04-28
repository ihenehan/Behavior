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
using Ninject;
using Behavior.Common.Models;
using Behave.ViewModels;

namespace Behave.Views
{
    public partial class StoryView : UserControl
    {
        public StoryView()
        {
            InitializeComponent();

            Loaded += (sender, e) => MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
        }

        private void UserControl_LostFocus(object sender, RoutedEventArgs e)
        {
            var model = DataContext as StoryViewModel;

            model.Update();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var model = DataContext as StoryViewModel;

            model.CreateScenario();
        }
    }
}
