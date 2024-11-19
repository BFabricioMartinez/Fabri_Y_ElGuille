using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp2.Login;

namespace WindowsFormsApp2.Register
{
    public partial class RegisterForm : Form
    {
        private BLL_Reg registerBLL = new BLL_Reg();

        public RegisterForm()
        {
            InitializeComponent();
            LoginForm.ActiveForm.Hide();
            this.FormClosing += RegisterForm_FormClosing;


        }

        private void BtnReg_Click(object sender, EventArgs e)
        {
            string usuario = BoxUserReg.Text.Trim();
            string contraseña = BoxContraReg.Text.Trim();

            try
            {
                if (registerBLL.RegistrarUsuario(usuario, contraseña))
                {
                    MessageBox.Show("Registro exitoso.");
                    // Aquí puedes redirigir al usuario al formulario principal
                    // Cerrar el formulario de registro actual
                    this.Close();

                    // Crear una nueva instancia del formulario de Login y mostrarla
                    LoginForm loginForm = new LoginForm();
                    loginForm.Show();
                }
                else
                {
                    MessageBox.Show("Error al registrar el usuario.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void RegisterForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Mostrar el formulario de Login cuando se cierre RegisterForm
            LoginForm loginForm = new LoginForm();
            loginForm.Show();
        }

        private void iconButton2_Click(object sender, EventArgs e)
        {

        }
    }
}