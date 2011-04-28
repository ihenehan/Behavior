using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Behave.ViewModels
{
    public class TreeViewItemViewModel : ViewModelBase
    {
        static readonly TreeViewItemViewModel DummyChild = new TreeViewItemViewModel();

        readonly ObservableCollection<TreeViewItemViewModel> children;
        readonly TreeViewItemViewModel parent;

        protected bool isExpanded;
        protected bool isSelected;
        protected bool shouldDelete;

        protected TreeViewItemViewModel(TreeViewItemViewModel parent, bool lazyLoadChildren)
        {
            this.parent = parent;

            isExpanded = false;
            isSelected = false;

            children = new ObservableCollection<TreeViewItemViewModel>();

            if (lazyLoadChildren)
                children.Add(DummyChild);
        }

        private TreeViewItemViewModel()
        {
        }

        public ObservableCollection<TreeViewItemViewModel> Children
        {
            get { return children; }
        }

        public bool HasDummyChild
        {
            get { return this.Children.Count == 1 && this.Children[0] == DummyChild; }
        }

        public bool IsExpanded
        {
            get { return isExpanded; }
            set
            {
                if (value != isExpanded)
                {
                    isExpanded = value;
                    this.OnPropertyChanged("IsExpanded");
                }

                if (isExpanded && parent != null)
                    parent.IsExpanded = true;

                if (this.HasDummyChild)
                {
                    this.Children.Remove(DummyChild);
                    this.LoadChildren();
                }
            }
        }

        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                if (value != isSelected)
                {
                    isSelected = value;
                    this.OnPropertyChanged("IsSelected");
                }
            }
        }

        public bool ShouldDelete
        {
            get { return shouldDelete; }
            set { shouldDelete = value; }
        }

        protected virtual void LoadChildren()
        {
        }

        public TreeViewItemViewModel Parent
        {
            get { return parent; }
        }
    }
}