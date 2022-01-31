using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PlanetscaleConnectionTest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var program = new Program();

            program.Run();
        }

        public void Run()
        {
            DotNetEnv.Env.Load(".env");

            var server = DotNetEnv.Env.GetString("SERVER");
            var user = DotNetEnv.Env.GetString("USER");
            var password = DotNetEnv.Env.GetString("PASSWORD");
            var database = DotNetEnv.Env.GetString("DATABASE");
            var port = DotNetEnv.Env.GetString("PORT");
            var ssl = DotNetEnv.Env.GetString("SSLMODE");

            var connectionString = $"server={server};user={user};database={database};port={port};password={password};SslMode={ssl}";
            var data = new List<(int, string, string)>();

            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                var command = new MySqlCommand("SELECT * FROM users", connection);
                var result = command.ExecuteReader();

                while (result.Read())
                {
                    int id = result.GetInt32("id");
                    string email = result.GetString("email");
                    string name1 = result.GetString("first_name");
                    string name2 = result.GetString("last_name");

                    data.Add((id, email, $"{name1} {name2}"));
                }
            }

            foreach (var d in data)
            {
                Console.WriteLine($"id={d.Item1}, email= {d.Item2}, name {d.Item3}");
            }
        }
    }
}