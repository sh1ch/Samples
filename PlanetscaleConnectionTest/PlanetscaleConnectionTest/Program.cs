using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PlanetscaleConnectionTest;

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
        
        // デフォルト
        /*
        Now(connectionString);
        Insert(connectionString);
        InsertUsingTransaction(connectionString, true);
        Update(connectionString);
        Select(connectionString);
        */

        // EF
        MySqlContext.ConnectionString = connectionString;

        //InsertEF();
        InsertUsingTransaction(true);
        UpdateEF();
        SelectEF();
    }

    private void Select(string connectionString)
    {
        var data = new List<(int, string, string)>();

        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();

            var command = new MySqlCommand("SELECT * FROM users;", connection);
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

    private void Insert(string connectionString)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();

            var sql1 = "INSERT INTO users (email, first_name, last_name) VALUES ('aa', 'bb', 'cc');";
            var sql2 = "SELECT LAST_INSERT_ID() FROM users;";

            var command = new MySqlCommand(sql1 + sql2, connection);

            // 挿入したカラムの ID を取得
            long id1 = (long)command.ExecuteScalar();
            long id2 = command.LastInsertedId;

            Console.WriteLine($"inserted id={id1}={id2}");
        }
    }

    private void Update(string connectionString)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();

            var command = new MySqlCommand("UPDATE users SET email = 'rance@rudras.wld', first_name = 'あてな', last_name = '２号' WHERE id = 1;", connection);

            command.ExecuteNonQuery();
        }
    }

    private void InsertUsingTransaction(string connectionString, bool raiseException)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();

            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    var sql1 = "INSERT INTO users (email, first_name, last_name) VALUES ('test', 'using', 'transaction');";
                    var sql2 = "SELECT LAST_INSERT_ID() FROM users;";

                    var command1 = new MySqlCommand(sql1 + sql2, connection);

                    // 挿入したカラムの ID を取得
                    long id1 = (long)command1.ExecuteScalar();
                    long id2 = command1.LastInsertedId;

                    var sql3 = $"INSERT INTO sample (data, data2) VALUES ('{id1}', 'related value');";
                    var sql4 = "SELECT LAST_INSERT_ID() FROM users;";

                    var command2 = new MySqlCommand(sql3 + sql4, connection);

                    // 挿入したカラムの ID を取得
                    long id3 = (long)command2.ExecuteScalar();
                    long id4 = command2.LastInsertedId;

                    // 例外を発生させるサンプル
                    if (raiseException)
                    {
                        throw new Exception();
                    }

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                }
            }
        }
    }

    private void Now(string connectionString)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();

            var command = new MySqlCommand("SELECT NOW();", connection);
            var result = (DateTime)command.ExecuteScalar();

            Console.WriteLine(result);
        }
    }

    private void SelectEF()
    {
        using (var context = new MySqlContext())
        {
            foreach (var u in context.Users)
            {
                Console.WriteLine($"id={u.Id}, email={u.Email}");
            }
        }
    }

    private void InsertEF()
    {
        var newUser = new Users
        {
            Email = "EF@aa.bb",
            FirstName = "EF_firstname",
            LastName = "EF_lastname",
        };

        using (var context = new MySqlContext())
        {
            context.Users.Add(newUser);
            context.SaveChanges();
        }
    }

    private void InsertUsingTransaction(bool raiseException)
    {
        using (var context = new MySqlContext())
        {
            using var transaction = context.Database.BeginTransaction();

            try
            {
                var newUser1 = new Users
                {
                    Email = "EF@cc",
                    FirstName = "cc",
                    LastName = "cc",
                };

                context.Users.Add(newUser1);
                context.SaveChanges();

                if (raiseException) throw new Exception();

                var newUser2 = new Users
                {
                    Email = "EF@dd",
                    FirstName = "dd",
                    LastName = "dd",
                };

                context.Users.Add(newUser2);
                context.SaveChanges();
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
            }
        }
    }

    private void UpdateEF()
    {
        using (var context = new MySqlContext())
        {
            var user = context.Users.SingleOrDefault(p => p.Id == 1);

            if (user == null) return;

            user.Email = "update@mail.com";
            user.FirstName = "update.name1";
            user.LastName = "update.name2";

            context.SaveChanges();
        }
    }
}
