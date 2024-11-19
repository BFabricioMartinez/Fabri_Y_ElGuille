using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace WindowsFormsApp2.Register
{
    public class BLL_Reg
    {
        private DAL_Reg registerDAL = new DAL_Reg();

        public bool RegistrarUsuario(string usuario, string contraseña)
        {
            // Validar que el usuario no exista
            if (registerDAL.UsuarioExiste(usuario))
            {
                throw new Exception("El usuario ya existe.");
            }

            // Insertar el nuevo usuario en la base de datos sin encriptar la contraseña
            return registerDAL.InsertarUsuario(usuario, contraseña);
        }
    }
}