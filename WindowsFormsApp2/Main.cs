using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp2.Misc;

namespace WindowsFormsApp2
{
    public partial class Main : Form
    {
        private BusinessLogicLayer _businessLogicLayer;

        #region CONSTRUCTORS
        public Main()
        {
            InitializeComponent();
            _businessLogicLayer = new BusinessLogicLayer();
        }

        #endregion

        #region EVENTS_HANDLERS
        private void Main_Load(object sender, EventArgs e)
        {
            PopulateContacts();
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            OpenContactDetailDialog();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            /*string texto = "ContraseñaSegura";
            string hash = HashHelper.GenerateMD5Hash(texto);

            MessageBox.Show($"Texto original: {texto}");
            MessageBox.Show($"Hash MD5: {hash}");*/

            
            PopulateContacts(txtSearch.Text);
            txtSearch.Text = null;
        }
        #endregion

        #region METHODS
        private void OpenContactDetailDialog()
        {
            ContactDetails fichita = new ContactDetails();
            fichita.ShowDialog(this);
        }

        public void PopulateContacts(string searchTxt = null)
        {
            gridContacts.DataSource = _businessLogicLayer.GetAllContacts(searchTxt);
        }
        #endregion

        private void gridContacts_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewCell cell = gridContacts.Rows[e.RowIndex].Cells[e.ColumnIndex];

            DataGridViewCell idToEdit = gridContacts.Rows[e.RowIndex].Cells[0];
            DataGridViewCell firstNameToEdit = gridContacts.Rows[e.RowIndex].Cells[1];
            DataGridViewCell lastNameToEdit = gridContacts.Rows[e.RowIndex].Cells[2];
            DataGridViewCell phoneToEdit = gridContacts.Rows[e.RowIndex].Cells[3];
            DataGridViewCell addressToEdit = gridContacts.Rows[e.RowIndex].Cells[4];

            //MessageBox.Show(cell.Value.ToString());

            switch (cell.Value.ToString())
            {
                case "Edit":
                    //MessageBox.Show("editar contacto" + idToEdit.Value.ToString());

                    Contact contact = new Contact();

                    contact.Id = (int) idToEdit.Value;
                    contact.FirstName = (string) firstNameToEdit.Value;
                    contact.LastName = (string) lastNameToEdit.Value;
                    contact.Phone = (string) phoneToEdit.Value;
                    contact.Address = (string) addressToEdit.Value;

                    ContactDetails contactDetail = new ContactDetails();
                    contactDetail.LoadContactForm(contact);
                    contactDetail.ShowDialog(this);
                    break;
                case "Delete":
                    //MessageBox.Show("borrar contacto" + idToEdit.Value.ToString());
                    DialogResult res = MessageBox.Show("Seguro que queres eliminar el registro?", "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (res == DialogResult.Yes)
                    {
                        DeleteContact((int)idToEdit.Value);
                        PopulateContacts();
                    }
                    
                    
                    break;
                default:
                    break;
            }

            #region TEST
            /* DataGridViewCell cell = gridContacts.Rows[e.RowIndex].Cells[e.ColumnIndex];
             DataGridViewCell idToEdit = gridContacts.Rows[e.RowIndex].Cells[0];
             DataGridViewCell firstNameToEdit = gridContacts.Rows[e.RowIndex].Cells[1];
             DataGridViewCell lastNameToEdit = gridContacts.Rows[e.RowIndex].Cells[2];
             DataGridViewCell phoneToEdit = gridContacts.Rows[e.RowIndex].Cells[3];
             DataGridViewCell addressToEdit = gridContacts.Rows[e.RowIndex].Cells[4];

             switch (cell.Value.ToString())
             {
                 case "Edit":
                       //MessageBox.Show("queres editar el registro"+ firstNameToEdit.Value.ToString());

                         Contact contact = new Contact();

                         contact.Id = (int)idToEdit.Value;
                         contact.FirstName = (string) firstNameToEdit.Value;
                         contact.LastName = (string)lastNameToEdit.Value;
                         contact.Phone = (string)phoneToEdit.Value;
                         contact.Address = (string)addressToEdit.Value;

                         ContactDetails contactDetails = new ContactDetails();

                         contactDetails.LoadContactForm(contact);
                         contactDetails.ShowDialog(this);
                     break;

                 case "Delete":
                     MessageBox.Show("queres borrar el registro" + idToEdit.Value.ToString());

                     break;

             }*/

            /*DataGridViewLinkCell cell = (DataGridViewLinkCell)gridContacts.Rows[e.RowIndex].Cells[e.ColumnIndex];
             if(cell.Value.ToString()=="Edit")
             {
                 ContactDetails contactDetails = new ContactDetails();
                 contactDetails.LoadContactForm(new Contact
                 {
                     Id = int.Parse(gridContacts.Rows[e.RowIndex].Cells[0].Value.ToString()),
                     FirstName = gridContacts.Rows[e.RowIndex].Cells[1].Value.ToString(),
                     LastName = gridContacts.Rows[e.RowIndex].Cells[2].Value.ToString(),
                     Phone = gridContacts.Rows[e.RowIndex].Cells[3].Value.ToString(),
                     Address = gridContacts.Rows[e.RowIndex].Cells[4].Value.ToString(),
                 });
                 contactDetails.ShowDialog(this);
             }*/

            #endregion
        }

        private void DeleteContact(int id)
        { 
            _businessLogicLayer.DeleteContact(id);
        }
    }
}



