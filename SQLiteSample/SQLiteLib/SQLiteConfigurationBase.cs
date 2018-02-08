using System;
using System.Data.Entity;
using System.Data.Entity.Core.Common;
using System.Data.SQLite;
using System.Data.SQLite.EF6;

namespace SQLiteLib
{
    /// <summary>
    /// <see cref="SQLiteConfigurationBase"/> クラスは、読み取り専用のDB設定の初期化クラスです。
    /// </summary>
    public class SQLiteConfigurationBase : DbConfiguration
    {
        #region Initializes

        /// <summary>
        /// <see cref="SQLiteConfigurationBase"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        public SQLiteConfigurationBase()
        {
            SetProviderFactory("System.Data.SQLite", SQLiteFactory.Instance);
            SetProviderFactory("System.Data.SQLite.EF6", SQLiteProviderFactory.Instance);
            SetProviderServices("System.Data.SQLite", (DbProviderServices)SQLiteProviderFactory.Instance.GetService(typeof(DbProviderServices)));

            Console.WriteLine("SQLite Providers の初期化を実行しました。");
        }

        #endregion
    }
}
