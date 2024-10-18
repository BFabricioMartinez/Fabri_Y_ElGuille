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

        }
        #endregion

        #region METHODS
        private void OpenContactDetailDialog()
        {
            ContactDetails fichita = new ContactDetails();
            fichita.ShowDialog(this);
        }
        public void PopulateContacts()
        {
            gridContacts.DataSource = _businessLogicLayer.GetAllContacts();
        }
        #endregion

    }
}



