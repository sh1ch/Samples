using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteLib.Orm
{
    public class MasterContext : DbContext
    {
        #region Properties

        public DbSet<User> Users { get; set; }

        public DbSet<UserParameter> UserParameters { get; set; }

        #endregion

        #region Initializes

        /// <summary>
        /// <see cref="MasterContext"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        public MasterContext(DbConnection connection) : base(connection, true)
        {

        }

        static MasterContext()
        {
            // エラーメッセージを消す
            //SQLite error (1): no such table: __MigrationHistory
            //SQLite error (1): no such table: __MigrationHistory
            //SQLite error (1): no such table: EdmMetadata

            System.Data.Entity.Database.SetInitializer<MasterContext>(null);
        }

        #endregion

        #region Protected Methods

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            base.OnModelCreating(modelBuilder);
        }

        #endregion
    }
}
