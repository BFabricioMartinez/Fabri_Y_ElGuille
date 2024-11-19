using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public class DataAccessLayer
    {
        private SqlConnection conn = new SqlConnection("Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=WindowsFormMariano;Data Source=DESKTOP-MO43P3P");
       
        public void InsertContact(Contact contact)
        {
            try
            {
                conn.Open();
                string query = @"
                                    insert into Contacts (FirstName, LastName, Phone, Address)
                                    values (@FirstName, @LastName, @Phone, @Address)";

                SqlParameter firstName = new SqlParameter();
                firstName.ParameterName = "@FirstName";
                firstName.Value = contact.FirstName;
                firstName.DbType = System.Data.DbType.String;

                SqlParameter lastName = new SqlParameter("@LastName", contact.LastName);
                SqlParameter phone = new SqlParameter("@Phone", contact.Phone);
                SqlParameter address = new SqlParameter("@Address", contact.Address);

                SqlCommand command = new SqlCommand(query, conn);

                command.Parameters.Add(firstName);
                command.Parameters.Add(lastName);
                command.Parameters.Add(phone);
                command.Parameters.Add(address);

                command.ExecuteNonQuery();


            }
            catch (Exception)
            {
                throw;
            }
            finally 
            { 
                conn.Close();
            }
        }

        public List<Contact> GetContacts(string searchTxt = null)
        {
            List<Contact> contacts = new List<Contact>();
           
            try
            {
                conn.Open();

                string query = "select * from Contacts";

                SqlCommand command = new SqlCommand();

                if(!string.IsNullOrEmpty(searchTxt))
                {
                    query += @" WHERE FirstName LIKE @search 
                                    OR LastName LIKE @search 
                                    OR Phone    LIKE @search
                                    OR Address  LIKE @search";

                    command.Parameters.Add(new SqlParameter("@search",$"%{searchTxt}%"));
                }

                command.CommandText = query;
                command.Connection = conn;

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read()) 
                { 
                    Contact contact = new Contact();

                    contact.Id = int.Parse(reader["Id"].ToString());
                    contact.FirstName = reader["FirstName"].ToString();
                    contact.LastName = reader["LastName"].ToString();
                    contact.Phone = reader["Phone"].ToString();
                    contact.Address = reader["Address"].ToString();

                    contacts.Add(contact);

                }

            }
            catch (Exception)
            {

                throw;
            }
            finally { conn.Close(); }
            return contacts;
        }
    
        public void UpdateContact(Contact contact)
        {
            try
            {
                conn.Open();

                string query = @"UPDATE Contacts  
                                                    SET FirstName = @FirstName, 
                                                         LastName = @LastName,
                                                            Phone = @Phone,
                                                          Address = @Address
                                                         WHERE Id = @Id";

                SqlParameter id = new SqlParameter("@Id", contact.Id);
                SqlParameter firstName = new SqlParameter("@FirstName", contact.FirstName);
                SqlParameter lastName = new SqlParameter("@LastName", contact.LastName);
                SqlParameter phone = new SqlParameter("@Phone", contact.Phone);
                SqlParameter address = new SqlParameter("@Address", contact.Address);

                SqlCommand command = new SqlCommand(query, conn);

                command.Parameters.Add(id);
                command.Parameters.Add(firstName);
                command.Parameters.Add(lastName);
                command.Parameters.Add(phone);
                command.Parameters.Add(address);

                command.ExecuteNonQuery();

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                conn.Close();
            }
        }

        public void DeleteContact(int id)
        {

            try
            {
                //conectar
                conn.Open();

                //def query sql
                string query = "DELETE FROM Contacts WHERE Id = @Id";

                //tratar parametros
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.Add(new SqlParameter("@Id", id));

                //ejecutar query
                command.ExecuteNonQuery();
               
            }
            catch (Exception)
            {

                throw;
            }
            finally { conn.Close(); }
        }


        //DAL PRODUCTOS
        public void AgregarProducto(string usuariosProd, decimal costoProd, decimal precioProd)
        {
            try
            {
                conn.Open();
                string query = @"
                        insert into Productos (UsuariosProd, CostoProd, PrecioProd)
                        values (@UsuariosProd, @CostoProd, @PrecioProd)";

                SqlCommand command = new SqlCommand(query, conn);

                // Asignación de parámetros
                command.Parameters.Add(new SqlParameter("@UsuariosProd", System.Data.DbType.String) { Value = usuariosProd });
                command.Parameters.Add(new SqlParameter("@CostoProd", System.Data.DbType.Decimal) { Value = costoProd });
                command.Parameters.Add(new SqlParameter("@PrecioProd", System.Data.DbType.Decimal) { Value = precioProd });

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al agregar producto", ex);
            }
            finally
            {
                conn.Close();
            }
        }

        public List<Producto> ObtenerProductos()
        {
            List<Producto> productos = new List<Producto>();

            try
            {
                conn.Open();

                string query = "SELECT * FROM Productos";
                SqlCommand command = new SqlCommand(query, conn);

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Producto producto = new Producto
                    {
                        Id = int.Parse(reader["ID"].ToString()),
                        UsuariosProd = reader["UsuariosProd"].ToString(),
                        CostoProd = decimal.Parse(reader["CostoProd"].ToString()),
                        PrecioProd = decimal.Parse(reader["PrecioProd"].ToString())
                    };

                    productos.Add(producto);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener productos", ex);
            }
            finally
            {
                conn.Close();
            }

            return productos;
        }

        public void BorrarProducto(int id)
        {

            try
            {
                //conectar
                conn.Open();

                //def query sql
                string query = "DELETE FROM Productos WHERE Id = @Id";

                //tratar parametros
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.Add(new SqlParameter("@Id", id));

                //ejecutar query
                command.ExecuteNonQuery();

            }
            catch (Exception)
            {

                throw;
            }
            finally { conn.Close(); }
        }
        //PEDIDOOOS
        public DataTable ObtenerUsuariosPedidos()
        {
            DataTable dt = new DataTable();
            try
            {
                conn.Open();
                string query = "SELECT ID, FirstName FROM contacts ORDER BY FirstName";
                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                adapter.Fill(dt);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }
            return dt;
        }

        public int AgregarPedido(int idCliente, decimal precioTotal)
        {
            try
            {
                conn.Open();
                string query = @"INSERT INTO pedidos (idCliente, total) 
                         VALUES (@idCliente, @precioTotal);
                         SELECT SCOPE_IDENTITY();";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@idCliente", idCliente);
                cmd.Parameters.AddWithValue("@precioTotal", precioTotal);
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }
        }

        public void AgregarPedidoItem(int idPedido, int idProducto, int cantidad, decimal precio)
        {
            try
            {
                conn.Open();
                string query = @"INSERT INTO pedidos_items (idPedido, idProducto, cantidad, precio) 
                   VALUES (@idPedido, @idProducto, @cantidad, @precio)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@idPedido", idPedido);
                cmd.Parameters.AddWithValue("@idProducto", idProducto);
                cmd.Parameters.AddWithValue("@cantidad", cantidad);
                cmd.Parameters.AddWithValue("@precio", precio);
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }
        }

        public DataTable ObtenerPedidos()
        {
            DataTable dt = new DataTable();
            try
            {
                conn.Open();
                string query = @"
            SELECT 
                p.id,
                u.id as idCliente,
                u.FirstName as nombreCliente,
                p.total
            FROM pedidos p
            INNER JOIN Contacts u ON p.idCliente = u.id";

                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                adapter.Fill(dt);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }
            return dt;
        }
        public DataTable ObtenerPedidoItems(int idPedido)
        {
            DataTable dt = new DataTable();
            try
            {
                conn.Open();
                string query = @"
            SELECT 
                pi.id,
                pr.UsuariosProd as producto,
                pi.cantidad,
                pr.PrecioProd as precio,
                (pi.cantidad * pr.PrecioProd) as subtotal
            FROM pedidos_items pi
            INNER JOIN productos pr ON pi.idproducto = pr.id
            WHERE pi.idpedido = @idPedido";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@idPedido", idPedido);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }
            return dt;
        }

        public void ActualizarPrecioTotalPedido(int idPedido, decimal total)
        {
            try
            {
                conn.Open();
                string query = "UPDATE pedidos SET total = @total WHERE id = @idPedido";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@total", total);
                cmd.Parameters.AddWithValue("@idPedido", idPedido);
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }
        }

        public void ActualizarEstadoPedido(int idPedido, string estado)
        {
            try
            {
                conn.Open();
                string query = "UPDATE pedidos SET estado = @estado WHERE id = @idPedido";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@estado", estado);
                cmd.Parameters.AddWithValue("@idPedido", idPedido);
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }
        }
        public bool EliminarPedido(int idPedido)
        {
            try
            {
                conn.Open();
                using (SqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        string deleteItemsQuery = "DELETE FROM pedidos_items WHERE idPedido = @idPedido";
                        SqlCommand cmdItems = new SqlCommand(deleteItemsQuery, conn, transaction);
                        cmdItems.Parameters.AddWithValue("@idPedido", idPedido);
                        cmdItems.ExecuteNonQuery();

                        string deletePedidoQuery = "DELETE FROM pedidos WHERE id = @idPedido";
                        SqlCommand cmdPedido = new SqlCommand(deletePedidoQuery, conn, transaction);
                        cmdPedido.Parameters.AddWithValue("@idPedido", idPedido);
                        int rowsAffected = cmdPedido.ExecuteNonQuery();

                        transaction.Commit();
                        return rowsAffected > 0;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }
        }

        public void EliminarPedidoItem(int idPedidoItem)
        {
            try
            {
                conn.Open();
                using (SqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        string query = "DELETE FROM pedidos_Items WHERE ID = @IdPedidoItem";
                        SqlCommand command = new SqlCommand(query, conn, transaction);
                        command.Parameters.AddWithValue("@IdPedidoItem", idPedidoItem);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected == 0)
                        {
                            throw new Exception("No se encontró el ítem de pedido para eliminar.");
                        }

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }
        }

        public PedidoDetalle ObtenerPedidoPorId(int idPedido)
        {
            try
            {
                conn.Open();
                string query = "SELECT id, idCliente, total FROM Pedidos WHERE id = @IdPedido";
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@IdPedido", idPedido);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new PedidoDetalle
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("id")),
                            IdCliente = reader.GetInt32(reader.GetOrdinal("idCliente")),
                            total = reader.GetDecimal(reader.GetOrdinal("total"))
                        };
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }
            return null;
        }

        public DataTable ObtenerUsuarios()
        {
            DataTable dt = new DataTable();
            try
            {
                conn.Open();
                string query = "SELECT ID, FirstName FROM Contacts ORDER BY FirstName";
                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                adapter.Fill(dt);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }
            return dt;
        }

        public class PedidoDetalle
        {
            public int Id { get; set; }
            public int IdCliente { get; set; }
            public decimal total { get; set; }
        }

        public class Producto
        {
            public int Id { get; set; }
            public string UsuariosProd { get; set; }
            public decimal CostoProd { get; set; }
            public decimal PrecioProd { get; set; }
        }
    }
}
