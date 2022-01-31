using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanetscaleConnectionTest;


public class DbContextBase : DbContext
{
    public DbSet<Users> Users => Set<Users>();

    //public DbSet<Users> Users { get; set; } = null!;

    public DbContextBase(DbConnection connection) : base (connection, true)
    {

    }
}
