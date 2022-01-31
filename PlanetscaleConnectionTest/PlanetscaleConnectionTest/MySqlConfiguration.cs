using MySql.Data.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanetscaleConnectionTest;

public class MySqlConfiguration : DbConfiguration
{
    /// <summary>
    /// <see cref="MySqlConfiguration"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    public MySqlConfiguration()
    {
        SetDefaultConnectionFactory(new MySqlConnectionFactory());

        // MySql.Data.MySqlClient
        string name = MySql.Data.EntityFramework.MySqlProviderInvariantName.ProviderName;

        SetProviderFactory(name, new MySql.Data.MySqlClient.MySqlClientFactory());
        SetProviderServices(name, new MySql.Data.MySqlClient.MySqlProviderServices());

        Console.WriteLine("MySQL Configuration の初期化を実行しました。");
    }
}
