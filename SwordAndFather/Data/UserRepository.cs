using SwordAndFather.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SwordAndFather.Data
{
    public class UserRepository
    {
        const string ConnectionString = "Server = localhost; Database = ClinkedIn; Trusted_Connection = True;";


        public User AddUser(string username, string password)
        {

            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                var insertUserCommand = connection.CreateCommand();
                insertUserCommand.CommandText = $@"insert into Users (username, password) 
                                              output inserted.*
                                              values (@username,@password)";

                insertUserCommand.Parameters.AddWithValue("username", username);
                insertUserCommand.Parameters.AddWithValue("username", password);

                var reader = insertUserCommand.ExecuteReader();

                if (reader.Read())
                {
                    var insertedUsername = reader["username"].ToString();
                    var insertedPassword = reader["password"].ToString();
                    var insertedId = (int)reader["id"];

                    var newUser = new User(username, password) { Id = insertedId };

                    //newUser.Id = insertedId; this does the same thing as having it in curlys above

                    connection.Close();

                    return newUser;
                }
            }            


                throw new Exception("No user found");

            
        }
        public List<User> GetAll()
        {
            var users = new List<User>();
            var connection = new SqlConnection(ConnectionString);
            connection.Open();

            var getAllUsersCommand = connection.CreateCommand();
            getAllUsersCommand.CommandText = @"select * 
                                               from users";

            var reader = getAllUsersCommand.ExecuteReader();

            while (reader.Read())
            {
                
                var id = (int)reader["id"];
                var username = reader["username"].ToString();
                var password = reader["password"].ToString();
                var user = new User(username, password) { Id = id };

                users.Add(user);
            }

            connection.Close();
            return users;
        }
    }
}
