using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject;
using Behavior.Common.Models;
using Behavior.Common.Repository;

namespace Behave.ViewModels
{
    public class DataItemViewModel : ViewModelBase
    {
        public DataItem DataItem
        {
            get
            {
                return dataItem;
            }
            set
            {
                dataItem = value;

                base.OnPropertyChanged("DataItem");
            }
        }

        private DataItem dataItem;
        private IRepository repo;

        public DataItemViewModel(DataItem dataItem)
        {
            DataItem = dataItem;

            repo = App.kernel.Get<IRepository>();
        }

        public string Data
        {
            get 
            {
                if (string.IsNullOrEmpty(dataItem.Name) && string.IsNullOrEmpty(dataItem.Data))
                    return "";

                return dataItem.Name + "=" + dataItem.Data; 
            }
            set 
            {
                var parameterSplit = value.Split('=');

                dataItem.Name = parameterSplit[0].Trim();

                if(parameterSplit.Count() > 1)
                    dataItem.Data = parameterSplit[1].Trim();

                OnPropertyChanged("Data");

                Update();
            }
        }

        public void Update()
        {
            //repo.Save(DataItem);   
        }
    }
}
