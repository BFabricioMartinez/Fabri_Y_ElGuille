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

        #region CONSTRUCTORS
        public Main()
        {
            InitializeComponent();
        }

        #endregion

        #region EVENTS_HANDLERS
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
            fichita.ShowDialog();
        }
        #endregion

    }
}



