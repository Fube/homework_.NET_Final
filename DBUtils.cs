using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContactLibrary;

namespace TermProject
{
    sealed class DBUtils
    {
        // Not compile-time constant so it can't be "const"
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;

        private const string createQuery =
            "INSERT INTO dbo.contacts (first_name, last_name, phone_number) VALUES (@fName, @lName, @phoneNumber) SET @ID = @@IDENTITY";

        private const string nullCreateQuery = "INSERT INTO dbo.contacts (first_name, last_name, phone_number) VALUES (@fName, @lName, NULL)";

        private const string readAllQuery = "SELECT * FROM dbo.contacts c ORDER BY c.id";

        private const string updateQuery =
            "UPDATE dbo.contacts SET first_name=@fName, last_name=@lName, phone_number=@phoneNumber WHERE id=@id";

        private const string deleteQuery = "DELETE FROM dbo.contacts WHERE id=@id";

        private const string identityQuery = "SELECT @@IDENTITY as newID FROM dbo.contacts";

        private static readonly Lazy<DBUtils> LazyInstance = new Lazy<DBUtils>(() => new DBUtils());

        public static DBUtils Instance => LazyInstance.Value;

        DBUtils(){}

        private void _compileCommand(out SqlCommand command, SqlConnection connection, string query) => command = new SqlCommand(query, connection);

        private void _tryExecute(SqlConnection conn, SqlCommand comm)
        {
            try
            {
                conn.Open();
                comm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
                Trace.WriteLine(ex.StackTrace);
            }
            finally
            {
                conn.Close();
            }
        }

        public long Create(Contact contact)
        {

            var connection = new SqlConnection(_connectionString);

            _compileCommand(out var command, connection, createQuery);
            _compileCommand(out var nullCommand, connection, nullCreateQuery);

            var (id, fName, lName, phoneNumber) = contact;

            if (id != null)
            {
                throw new DirtyFieldException("ID", "DB");
            }


            command.Parameters.AddWithValue("@fName", fName);
            command.Parameters.AddWithValue("@lName", lName);
            command.Parameters.AddWithValue("@phoneNumber", phoneNumber);
            command.Parameters.Add("@ID", SqlDbType.BigInt).Direction = ParameterDirection.Output;

            _tryExecute(connection, command);

            return long.Parse(command.Parameters["@ID"].Value.ToString());
        }

        public List<Contact> CreateMany(List<Contact> contacts)
        {

            List<Contact> toReturn = new List<Contact>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                foreach (var contact in contacts)
                {
                    try
                    {
                        _compileCommand(out var command, connection, createQuery);

                        var (id, fName, lName, phoneNumber) = contact;

                        if (id != null)
                        {
                            throw new DirtyFieldException("ID", "DB");
                        }

                        command.Parameters.AddWithValue("@fName", fName);
                        command.Parameters.AddWithValue("@lName", lName);
                        command.Parameters.Add("@ID", SqlDbType.BigInt).Direction = ParameterDirection.Output;


                        // Can't use ternary. Couldn't figure out. Didn't bother looking into it
                        if (string.IsNullOrEmpty(phoneNumber)) command.Parameters.AddWithValue("@phoneNumber", DBNull.Value);
                        else command.Parameters.AddWithValue("@phoneNumber", phoneNumber);

                        command.ExecuteNonQuery();

                        long newID = long.Parse(command.Parameters["@ID"].Value.ToString());

                        toReturn.Add(new Contact(
                            newID, fName, lName, phoneNumber    
                        ));
                    }
                    catch (Exception ex)
                    {
                        Trace.WriteLine("Failed to add contact");
                        Trace.WriteLine(ex.Message);
                    }
                }
            }

            return toReturn;
        }

        public List<Contact> ReadAll()
        {
            List<Contact> toReturn = new List<Contact>();

            using (var connection = new SqlConnection(_connectionString))
            {
                _compileCommand(out var command, connection, readAllQuery);
                connection.Open();

                var reader = command.ExecuteReader();
                while (reader.Read()) 
                    toReturn.Add(new Contact((long)reader[0],reader[1].ToString(), reader[2].ToString(), reader[3].ToString()));
                
            }

            return toReturn;
        }
        public List<Contact> ReadOne(long id)
        {
            List<Contact> toReturn = new List<Contact>();

            using (var connection = new SqlConnection(_connectionString))
            {
                _compileCommand(out var command, connection, $"SELECT * FROM dbo.contacts c where c.id={id}");
                connection.Open();

                var reader = command.ExecuteReader();
                toReturn.Add(new Contact((long)reader[0], reader[1].ToString(), reader[2].ToString(), reader[3].ToString()));
            }

            return toReturn;
        }

        public void Update(Contact contact)
        {

            //TODO: use ContactManager's FindById
            var connection = new SqlConnection(_connectionString);

            _compileCommand(out var command, connection, updateQuery);

            var (id, fName, lName, phoneNumber) = contact;

            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@fName", fName);
            command.Parameters.AddWithValue("@lName", lName);
            command.Parameters.AddWithValue("@phoneNumber", phoneNumber);

            _tryExecute(connection, command);
        }

        public void RemoveOne(long idToDEL)
        {
            var connection = new SqlConnection(_connectionString);

            _compileCommand(out var command, connection, deleteQuery);

            command.Parameters.AddWithValue("@id", idToDEL);

            _tryExecute(connection, command);
        }
    }
}
