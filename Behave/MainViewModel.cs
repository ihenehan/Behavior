using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using BehaveMVVM.Commands;
using BehaveMVVM.Models;

namespace BehaveMVVM.ViewModels
{
    public class MainViewModel
    {
        private DelegateCommand clearContactBookCommand;
        private DelegateCommand exitCommand;
        private ObservableCollection<ContactViewModel> contacts = new ObservableCollection<ContactViewModel>();

        public MainViewModel()
        {
        }

        public ObservableCollection<ContactViewModel> Contacts
        {
            get { return contacts; }
        }

        public ICommand ClearContactBookCommand
        {
            get
            {
                if (clearContactBookCommand == null)
                {
                    clearContactBookCommand = new DelegateCommand(ClearContactBook, CanClearContactBook);
                }
                return clearContactBookCommand;
            }
        }

        public ICommand ExitCommand
        {
            get
            {
                if (exitCommand == null)
                {
                    exitCommand = new DelegateCommand(Exit);
                }
                return exitCommand;
            }
        }

        public void LoadContacts(string filename)
        {
            List<Contact> contactsList = ContactsDataSource.Load(filename);
            foreach (Contact contact in contactsList)
            {
                contacts.Add(new ContactViewModel(contact));
            }
        }

        private bool CanClearContactBook()
        {
            return contacts.Count > 0;
        }

        private void ClearContactBook()
        {
            contacts.Clear();
        }

        private void Exit()
        {
            Application.Current.Shutdown();
        }
    }
}
