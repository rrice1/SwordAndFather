using SwordAndFather.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace SwordAndFather.Data
{
    public class UserRepository
    {
        const string ConnectionString = "Server = localhost; Database = ClinkedIn; Trusted_Connection = True;";


        public User AddUser(string username, string password)
        {

            using (var db = new SqlConnection(ConnectionString))
            {

                var newUser = db.QueryFirstOrDefault<User>(@"insert into users (username, password)
                                               output inserted.*
                                               values(@username,@password)", 
                                               new { username, password});

                if (newUser != null)
                {
                    return newUser;
                }

                //connection.Open();

                //var insertUserCommand = connection.CreateCommand();
                //insertUserCommand.CommandText = $@"insert into Users (username, password) 
                //                              output inserted.*
                //                              values (@username,@password)";

                //insertUserCommand.Parameters.AddWithValue("username", username);
                //insertUserCommand.Parameters.AddWithValue("username", password);

                //var reader = insertUserCommand.ExecuteReader();

                //if (reader.Read())
                //{
                //    var insertedUsername = reader["username"].ToString();
                //    var insertedPassword = reader["password"].ToString();
                //    var insertedId = (int)reader["id"];

                //    var newUser = new User(username, password) { Id = insertedId };

                //    //newUser.Id = insertedId; this does the same thing as having it in curlys above

                //    connection.Close();

                //    return newUser;
                //}
            }            


                throw new Exception("No user found");

            
        }

        public void DeleteUser(int id)
        {
            using (var db = new SqlConnection(ConnectionString))
            {
                var rowsAffected = db.Execute("delete from users where Id = @id", new {id});

                if(rowsAffected != 1)
                {
                    throw new Exception("Didn't delete");
                }
            }
        }

        public User UpdateUser(User userToUpdate)
        {
            using (var db = new SqlConnection(ConnectionString))
            {
               var rowsAffected = db.Execute(@"update users 
                             set username = @username,
                                 password = @password
                             where id = @id", userToUpdate);
                if (rowsAffected == 1)
                return userToUpdate;
            }
            throw new Exception("Could not update user");
            
        }

            public IEnumerable<User> GetAll()
        {            
            using (var db = new SqlConnection(ConnectionString))
            {
               var users = db.Query<User>("select * from users").ToList();

                return users;
                //var getAllUsersCommand = connection.CreateCommand();
                //getAllUsersCommand.CommandText = @"select * 
                //                               from users";

                //var reader = getAllUsersCommand.ExecuteReader();

                //while (reader.Read())
                //{

                //    var id = (int)reader["id"];
                //    var username = reader["username"].ToString();
                //    var password = reader["password"].ToString();
                //    var user = new User(username, password) { Id = id };

                //    users.Add(user);

            }

            
            

        }
    }
}
