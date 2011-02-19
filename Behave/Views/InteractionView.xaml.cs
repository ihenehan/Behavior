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

namespace Behave.Views
{
    public partial class InteractionView : UserControl
    {
        public InteractionView()
        {
            InitializeComponent();
        }

        private void Add_DataItem_Button_Click(object sender, RoutedEventArgs e)
        {
            var model = DataContext as InteractionViewModel;

            model.AddNewDataItem();

            e.Handled = true;
        }

        private void DeleteDataItem_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to delete this parameter?", "Confirm Parameter Delete", MessageBoxButton.OKCancel, MessageBoxImage.Exclamation, MessageBoxResult.Cancel);

            if (result.Equals(MessageBoxResult.OK))
            {
                var model = DataContext as InteractionViewModel;

                var dataModel = (e.OriginalSource as Button).DataContext as DataItemViewModel;

                model.DeleteDataItem(dataModel.DataItem);
            }

            e.Handled = true;
        }

        private void UserControl_LostFocus(object sender, RoutedEventArgs e)
        {
            var model = DataContext as InteractionViewModel;

            if(model != null)
                model.Update();
        }

        
    }
}
