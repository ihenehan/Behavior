using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Reflection;
using Behave.Views;
using Behave.ViewModels;
using Behavior.Common.Repository;
using Behavior.Common.Models;
using Ninject;

namespace Behave
{
    public partial class App : Application
    {
        public static StandardKernel kernel = new StandardKernel();

        public App() 
        {
            InitializeComponent();
        }

        private void OnStartup(object sender, StartupEventArgs e)
        {
            kernel = new StandardKernel();

            kernel.Load(AppDomain.CurrentDomain.GetAssemblies());

            var mainView = kernel.Get<MainView>();

            mainView.Show();
        }
    }
}
