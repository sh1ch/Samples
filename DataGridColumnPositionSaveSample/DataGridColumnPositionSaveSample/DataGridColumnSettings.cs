using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DataGridColumnPositionSaveSample
{
    /// <summary>
    /// <see cref="DataGridColumnSettings"/> クラスは、<see cref="System.Windows.Controls.DataGridColumn"/> の設定を管理するクラスです。
    /// </summary>
    public static class DataGridColumnSettings
    {
        #region Public Methods

        public static void Load(string filePath, IEnumerable<DataGridColumn> columns)
        {
            var text = "";

            if (!File.Exists(filePath)) throw new IOException("指定されたファイルパスにファイルが存在しません。");

            using (var reader = new StreamReader(filePath, Encoding.UTF8))
            {
                text = reader.ReadToEnd();
            }

            if (string.IsNullOrEmpty(text)) throw new IOException("指定されたファイルのデータが空白です。");

            var jsonParams = JsonSerializer.Deserialize<IEnumerable<DataGridParameter>>(text);

            if ((jsonParams?.Count() ?? 0) <= 0) return;

            // 左端（小さい番号）から順番に設定していく
            foreach (DataGridParameter param in jsonParams.OrderBy(p => p.DisplayIndex))
            {
                var column = columns.SingleOrDefault(p => p.Header.ToString() == param.Header);

                if (column != null)
                {
                    column.DisplayIndex = param.DisplayIndex;
                    column.Width = param.Width;
                }
            }
        }

        public static void Save(string filePath, IEnumerable<DataGridColumn> columns)
        {
            if (string.IsNullOrEmpty(filePath)) throw new IOException("指定されたファイルパスに異常があります。");
            if (columns == null) throw new ArgumentNullException("設定するカラムが null です。");

            var columnParams = columns.Select(p => new DataGridParameter { Header = p.Header.ToString(), DisplayIndex = p.DisplayIndex, Width = p.ActualWidth });
            var text = JsonSerializer.Serialize(columnParams);

            using (var writer = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                writer.Write(text);
            }
        }

        #endregion
    }
}
