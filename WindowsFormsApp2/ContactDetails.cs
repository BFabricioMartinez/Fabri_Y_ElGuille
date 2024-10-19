using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class ContactDetails : Form
    {
        private BusinessLogicLayer _businessLogicLayer;
        private Contact _contact;
        public ContactDetails()
        {
            InitializeComponent();
            _businessLogicLayer = new BusinessLogicLayer();
        }

        private void ContactDetails_Load(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveContact();
            this.Close();
            ((Main) this.Owner).PopulateContacts();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void SaveContact()
        {
            Contact contact = new Contact();

            contact.Id = (_contact != null) ? _contact.Id : 0;

            contact.FirstName = txtFirstName.Text;
            contact.LastName = txtLastName.Text;
            contact.Phone = txtPhone.Text;
            contact.Address = txtAddress.Text;


            _businessLogicLayer.SaveContact(contact);
        }

        public void LoadContactForm(Contact contact)
        {
            _contact = contact;

            ClearContactForm();

            txtFirstName.Text = contact.FirstName;
            txtLastName.Text = contact.LastName;
            txtPhone.Text = contact.Phone;
            txtAddress.Text = contact.Address;

        }

        private void ClearContactForm()
        {
            txtFirstName.Text = string.Empty;
            txtLastName.Text = string.Empty;     
            txtPhone.Text = string.Empty;
            txtAddress.Text= string.Empty;
        }


    }
}
