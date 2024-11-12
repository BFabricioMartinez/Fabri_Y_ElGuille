using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public class DataAccessLayer
    {
        private SqlConnection conn = new SqlConnection("Integrated Security=SSPI;Persist Security Info=False;User ID=sa;Initial Catalog=WinFormsContacts;Data Source=DESKTOP-L2H4HG4\\SQLEXPRESS");
       
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

    }

   
}
