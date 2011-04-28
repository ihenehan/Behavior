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
using Ninject;
using System.Windows.Shapes;
using Behave.ViewModels;

namespace Behave.Views
{
    public partial class ProjectView : UserControl
    {
        public ProjectView()
        {
            InitializeComponent();
        }

        private void UserControl_LostFocus(object sender, RoutedEventArgs e)
        {
            var model = DataContext as ProjectViewModel;

            model.Update();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var model = DataContext as ProjectViewModel;

            model.CreateStory();
        }
    }
}
