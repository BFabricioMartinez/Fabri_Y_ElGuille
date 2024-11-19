using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp2.Login
{
    public class BLL
    {
        private DAL loginDal;

        public BLL()
        {
            loginDal = new DAL();
        }

        public bool ValidarCredenciales(string usuario, string contraseña)
        {
            // Validaciones de lógica de negocio
            if (string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(contraseña))
            {
                throw new Exception("Usuario y contraseña son requeridos");
            }

            if (usuario.Length < 3)
            {
                throw new Exception("El usuario debe tener al menos 3 caracteres");
            }

            if (contraseña.Length < 4)
            {
                throw new Exception("La contraseña debe tener al menos 4 caracteres");
            }

            // Si pasa las validaciones, llama al DAL
            return loginDal.ValidarUsuario(usuario, contraseña);
        }

        public int ObtenerIdUsuario(string usuario)
        {
            if (string.IsNullOrEmpty(usuario))
            {
                throw new Exception("Usuario es requerido");
            }

            return loginDal.ObtenerIdUsuario(usuario);
        }
    }
}