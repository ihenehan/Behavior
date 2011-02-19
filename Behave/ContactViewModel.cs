using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BehaveMVVM.ViewModels
{
    /// <summary>
    /// View Model only exposes a subset of the underlying
    /// model properties
    /// </summary>
    public class ContactViewModel : ViewModelBase
    {
        private Models.Contact contact;

        public ContactViewModel(Models.Contact contact)
        {
            this.contact = contact;
        }

        public string FirstName
        {
            get { return contact.FirstName; }
        }

        public string LastName
        {
            get { return contact.LastName; }
        }

        public string Email
        {
            get { return contact.Email; }
        }
    }
}
