using System;
using System.Data.SqlClient;

namespace WindowsFormsApp2.Login
{
    public class DAL
    {
        private SqlConnection conn = new SqlConnection("Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=WindowsFormMariano;Data Source=DESKTOP-MO43P3P");

        public bool ValidarUsuario(string usuarioR, string contraseñaR)
        {
            try
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM Register WHERE UsuarioR = @UsuarioR AND ContraseñaR = @ContraseñaR";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UsuarioR", usuarioR);
                    cmd.Parameters.AddWithValue("@ContraseñaR", contraseñaR);
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al validar usuario: " + ex.Message);
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
            }
        }

        // Método opcional para obtener el ID del usuario
        public int ObtenerIdUsuario(string usuarioR)
        {
            try
            {
                conn.Open();
                string query = "SELECT Id FROM Register WHERE UsuarioR = @UsuarioR";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UsuarioR", usuarioR);
                    object result = cmd.ExecuteScalar();
                    return result != null ? Convert.ToInt32(result) : -1;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener ID de usuario: " + ex.Message);
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
            }
        }
    }
}