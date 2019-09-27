using DAO;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MdbTest
{
    /// <summary>
    /// <see cref="Mdb"/> クラスは、(.mdb) 形式のデータベースファイルの操作をサポートします。
    /// </summary>
    class Mdb
    {
        /// <summary>
        /// サンプルテーブルのデータを選択します。(Provider=Microsoft.Jet.OLEDB.4.0)
        /// </summary>
        /// <returns>選択したデータのコレクション。</returns>
        public IEnumerable<Charactor> SelectFromOLEDB_JET()
        {
            var connection = new OleDbConnection();
            var command = new OleDbCommand();

            var source = @"DB\Charactors.mdb";
            connection.ConnectionString = $@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={source}";

            connection.Open();

            command.CommandText = "SELECT * FROM サンプルテーブル";
            command.Connection = connection;

            var reader = command.ExecuteReader();
            var charactors = new List<Charactor>();

            while (reader.Read())
            {
                var chara = new Charactor();

                chara.Id = (int)reader[0];
                chara.Name = !reader.IsDBNull(1) ? (string)reader[1] : "";
                chara.Hp = !reader.IsDBNull(2) ? (int?)reader[2] : null;
                chara.Mp = !reader.IsDBNull(3) ? (int?)reader[3] : null;
                chara.IsDead = (bool)reader[4];

                charactors.Add(chara);
            }

            return charactors;
        }

        /// <summary>
        /// サンプルテーブルのデータを選択します。(Provider=Microsoft.ACE.OLEDB.12.0)
        /// </summary>
        /// <returns>選択したデータのコレクション。</returns>
        public IEnumerable<Charactor> SelectFromOLEDB()
        {
            var connection = new OleDbConnection();
            var command = new OleDbCommand();

            var source = @"DB\Charactors.mdb";
            connection.ConnectionString = $@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={source}";

            connection.Open();

            command.CommandText = "SELECT * FROM サンプルテーブル";
            command.Connection = connection;

            var reader = command.ExecuteReader();
            var charactors = new List<Charactor>();

            while (reader.Read())
            {
                var chara = new Charactor();

                chara.Id = (int)reader[0];
                chara.Name = !reader.IsDBNull(1) ? (string)reader[1] : "";
                chara.Hp = !reader.IsDBNull(2) ? (int?)reader[2] : null;
                chara.Mp = !reader.IsDBNull(3) ? (int?)reader[3] : null;
                chara.IsDead = (bool)reader[4];

                charactors.Add(chara);
            }

            return charactors;
        }

        public string GetEngineType()
        {
            var connection = new ADODB.Connection();
            var source = @"DB\Charactors.mdb";

            connection.ConnectionString = $@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={source}";
            connection.Open();

            ADODB.Property prop = connection.Properties["Jet OLEDB:Engine Type"];

            return prop?.Value.ToString() ?? "-1";
        }

        /// <summary>
        /// データベースファイルのプロパティ AccessVersion の値を取得します。
        /// </summary>
        /// <returns>AccessVersion の値。存在しない場合は "" を返却。</returns>
        public string GetAccessVersion()
        {
            var engine = new DAO.DBEngine();
            var source = @"DB\Charactors.mdb";

            var db = engine.OpenDatabase(source);
            var props = db.Properties;

            bool HasProperty(string name)
            {
                foreach (DAO.Property p in props)
                {
                    if (p.Name == name) return true;
                }
                return false;
            };

            var version = HasProperty("AccessVersion") ? (string)props["AccessVersion"].Value.ToString() : "";
            // var majorVer = version.Length > 2 ? version.Substring(0, 2) : "";
            // var projVer = HasProperty("ProjVer") ? (string)props["ProjVer"].Value.ToString() : "";

            return version;
        }

        /// <summary>
        /// データベースファイルを作成した Access の種別を取得します。
        /// <para>
        /// DAO や ADO を利用しコード上で作成したデータベースファイルは識別することができません。
        /// </para>
        /// </summary>
        /// <returns>Access の種別。</returns>
        public AccessType GetAccessType()
        {
            var engine = new DAO.DBEngine();
            var source = @"DB\Charactors.mdb";

            var db = engine.OpenDatabase(source);
            var props = db.Properties;

            bool HasProperty(string name)
            {
                foreach (DAO.Property p in props)
                {
                    if (p.Name == name) return true;
                }
                return false;
            };

            var version = HasProperty("AccessVersion") ? (string)props["AccessVersion"].Value.ToString() : "";
            var majorVer = version.Length > 2 ? version.Substring(0, 2) : "";
            var projVer = HasProperty("ProjVer") ? (string)props["ProjVer"].Value.ToString() : "";

            var type = AccessType.UnDefined;

            AccessType GetAfterType()
            {
                switch (projVer)
                {
                    case "24": return AccessType.Access2002;
                    case "35": return AccessType.Access2003;
                }
                return AccessType.UnDefined;
            };

            switch (majorVer)
            {
                case "02": type = AccessType.Access2; break;
                case "06": type = AccessType.Access7_95; break;
                case "07": type = AccessType.Access97; break;
                case "08":
                    type = projVer != "" ? GetAfterType() : AccessType.Access2000;
                    break;
                case "09":
                    type = projVer != "" ? GetAfterType() : AccessType.Access2002;
                    break;
                default:
                    break;
            }

            return type;
        }

    }
}
