using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp2.Register;

namespace WindowsFormsApp2.Login
{
    public partial class LoginForm : Form
    {

        private BLL loginBLL = new BLL();
        public LoginForm()
        {
            InitializeComponent();
            BoxContraseña.PasswordChar = '*';
        }


        public void OpenRegister()
        {
            RegisterForm fichita = new RegisterForm();
            fichita.ShowDialog(this);
        }

        private void BtnLogin_Click_1(object sender, EventArgs e)
        {
            string usuario = BoxUsuario.Text.Trim();
            string contraseña = BoxContraseña.Text.Trim();
            try
            {
                if (loginBLL.ValidarCredenciales(usuario, contraseña))
                {
                    Main mainForm = new Main();
                    this.Hide();
                    mainForm.Show();
                }
                else
                {
                    MessageBox.Show("Usuario o contraseña incorrectos");
                    // Opcionalmente, limpiar el campo de contraseña
                    BoxContraseña.Clear();
                    // Dar foco al campo de usuario
                    BoxUsuario.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnRegister_Click_1(object sender, EventArgs e)
        {
            OpenRegister();
        }

        private void BoxUsuario_TextChanged(object sender, EventArgs e)
        {

        }

        private void BoxContraseña_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
