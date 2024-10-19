using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp2
{
    
    public class BusinessLogicLayer
    {
        private DataAccessLayer _dataAccessLayer;

        public BusinessLogicLayer()
        { 
            _dataAccessLayer = new DataAccessLayer();
        }

        public Contact SaveContact(Contact contact)
        { 
           if(contact.Id == 0) 
               _dataAccessLayer.InsertContact(contact);
           else
               _dataAccessLayer.UpdateContact(contact);
           return contact;
        }
        public List<Contact> GetAllContacts() 
        {
           return  _dataAccessLayer.GetContacts(); 
        }
    }
}
