using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanetscaleConnectionTest;

[DebuggerDisplay("接続文字列:{ConnectionString}")]
public class MySqlContext : DbContextBase
{
    public static string ConnectionString { get; set; } = "";

    public MySqlContext() : base(new MySqlConnection(ConnectionString))
    {
        
    }
}
