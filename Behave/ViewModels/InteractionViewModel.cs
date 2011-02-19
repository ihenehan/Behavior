using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Ninject;
using Behavior.Common.Models;
using Behavior.Common.Repository;
using Ninject.Parameters;

namespace Behave.ViewModels
{
    public class InteractionViewModel : ViewModelBase
    {
        public Interaction Interaction
        {
            get
            {
                return interaction;
            }
            set
            {
                interaction = value;

                base.OnPropertyChanged("Interaction");
            }
        }

        public ObservableCollection<DataItemViewModel> DataItemViewModels
        {
            get { return dataItemViewModels; }
            set 
            { 
                dataItemViewModels = value;

                OnPropertyChanged("DataItemViewModels");
            }
        }

        private ObservableCollection<DataItemViewModel> dataItemViewModels = new ObservableCollection<DataItemViewModel>();
        private Interaction interaction;
        private IRepository repo;

        public InteractionViewModel(Interaction interaction)
        {
            Interaction = interaction;

            Interaction.DataItems = Interaction.DataItems.OrderBy(d => d.Index).ToList();

            foreach (DataItem d in Interaction.DataItems)
                dataItemViewModels.Add(new DataItemViewModel(d));

            repo = App.kernel.Get<IRepository>();
        }

        public string Name
        {
            get { return Interaction.Name; }
            set 
            { 
                Interaction.Name = value;

                OnPropertyChanged("Name");

                Update();
            }
        }

        public bool ExpectFailure
        {
            get { return Interaction.ExpectFailure; }
            set
            {
                Interaction.ExpectFailure = value;

                OnPropertyChanged("ExpectFailure");

                Update();
            }
        }

        public string ReturnName
        {
            get { return Interaction.ReturnName; }
            set
            {
                Interaction.ReturnName = value;

                OnPropertyChanged("ReturnName");

                Update();
            }
        }

        public void Update()
        {
            //repo.Save(Interaction);
        }

        public List<DataItem> DataItems
        {
            get
            {
                Interaction.DataItems = Interaction.DataItems.OrderBy(d => d.Index).ToList();

                return Interaction.DataItems;
            }
            set
            {
                Interaction.DataItems = value;

                Interaction.DataItems = Interaction.DataItems.OrderBy(d => d.Index).ToList();

                Update();

                CreateViewModels();

                OnPropertyChanged("DataItems");
            }
        }

        public void AddNewDataItem()
        {
            var index = GetLastIndex();

            var newDataItem = new DataItem() { ParentId = Interaction.Id, Index=index };

            Interaction.DataItems.Add(newDataItem);

            Interaction.ChildrenIds.Add(newDataItem.Id);

            DataItemViewModels.Add(new DataItemViewModel(newDataItem));

            OnPropertyChanged("DataItems");
        }

        public void DeleteDataItem(DataItem dataItem)
        {
            if (Interaction.DataItems.Count > 1)
            {
                DataItemViewModels.Remove(DataItemViewModels.FirstOrDefault(d => d.DataItem.Equals(dataItem)));

                DataItems.Remove(dataItem);

                OnPropertyChanged("DataItems");
            }
        }

        public void CreateViewModels()
        {
            if (DataItemViewModels == null)
                DataItemViewModels = new ObservableCollection<DataItemViewModel>();

            DataItemViewModels.Clear();

            foreach (DataItem d in DataItems)
                DataItemViewModels.Add(new DataItemViewModel(d));

            DataItemViewModels.OrderBy(d => d.DataItem.Index);
        }

        public int GetLastIndex()
        {
            if (DataItemViewModels.Count > 0)
            {
                DataItemViewModels.OrderBy(ivm => ivm.DataItem.Index);

                return DataItemViewModels.Last().DataItem.Index + 1;
            }

            return 0;
        }
    }
}
