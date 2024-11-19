using System;
using System.Data.SqlClient;

public class DAL_Reg
{
    private SqlConnection conn = new SqlConnection("Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=WindowsFormMariano;Data Source=DESKTOP-MO43P3P");

    public bool UsuarioExiste(string usuarioR)
    {
        try
        {
            conn.Open();
            string query = "SELECT COUNT(*) FROM Register WHERE UsuarioR = @UsuarioR";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@UsuarioR", usuarioR);
                int count = (int)cmd.ExecuteScalar();
                return count > 0;
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error al verificar existencia de usuario: " + ex.Message);
        }
        finally
        {
            if (conn.State == System.Data.ConnectionState.Open)
                conn.Close();
        }
    }

    public bool InsertarUsuario(string usuarioR, string contraseñaR)
    {
        try
        {
            conn.Open();
            string query = "INSERT INTO Register (UsuarioR, ContraseñaR) VALUES (@UsuarioR, @ContraseñaR)";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@UsuarioR", usuarioR);
                cmd.Parameters.AddWithValue("@ContraseñaR", contraseñaR);
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error al insertar usuario: " + ex.Message);
        }
        finally
        {
            if (conn.State == System.Data.ConnectionState.Open)
                conn.Close();
        }
    }
}
