using System;
using System.Collections.Generic;
using System.Configuration;
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
            "INSERT INTO dbo.contacts (first_name, last_name, phone_number) VALUES (@fName, @lName, @phoneNumber)";

        private const string readAllQuery = "SELECT * FROM dbo.contacts c ORDER BY c.id";

        private const string readOneQuery = "SELECT * FROM dbo.contacts c where c.id=@id";

        private const string updateQuery =
            "UPDATE dbo.contacts SET first_name=@fName, last_name=@lName, phone_number=@phoneNumber WHERE c.id=@id";

        private const string deleteQuery = "DELETE FROM dbo.contacts c WHERE c.id=@id";

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



        public void Create(Contact contact)
        {

            var connection = new SqlConnection(_connectionString);

            _compileCommand(out var command, connection, createQuery);

            var (id, fName, lName, phoneNumber) = contact;

            if (id != null)
            {
                throw new DirtyFieldException("ID", "DB");
            }

            command.Parameters.AddWithValue("@fName", fName);
            command.Parameters.AddWithValue("@lName", lName);
            command.Parameters.AddWithValue("@phoneNumber", phoneNumber);

            _tryExecute(connection, command);
            
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
                    toReturn.Add(new Contact((long)reader["id"],reader["first_name"].ToString(), reader["last_name"].ToString(), reader["phone_number"].ToString()));
                
            }

            return toReturn;
        }
        public List<Contact> ReadOne(long id)
        {
            List<Contact> toReturn = new List<Contact>();

            using (var connection = new SqlConnection(_connectionString))
            {
                _compileCommand(out var command, connection, readOneQuery);

                command.Parameters.AddWithValue("@id", id);

                connection.Open();

                var reader = command.ExecuteReader();
                while (reader.Read())
                    toReturn.Add(new Contact((long)reader["id"], reader["first_name"].ToString(), reader["last_name"].ToString(), reader["phone_number"].ToString()));
            }

            return toReturn;
        }

        public void Update(long idToUPD, Contact contact)
        {
            var connection = new SqlConnection(_connectionString);

            _compileCommand(out var command, connection, updateQuery);

            var (id, fName, lName, phoneNumber) = contact;

            if (id != null)
            {
                throw new DirtyFieldException("ID", "DB");
            }
            command.Parameters.AddWithValue("@id", idToUPD);
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
