using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DataGridColumnPositionSaveSample
{
    /// <summary>
    /// <see cref="DataGridColumnSettings"/> クラスは、<see cref="System.Windows.Controls.DataGridColumn"/> の設定を管理するクラスです。
    /// </summary>
    public class DataGridColumnSettings
    {
        #region Properties

        #endregion

        #region Initializes

        /// <summary>
        /// <see cref="DataGridColumnSettings"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        public DataGridColumnSettings() { }

        #endregion

        #region Public Methods

        public void Save(string filePath, IEnumerable<DataGridColumn> columns)
        {
            if (string.IsNullOrEmpty(filePath)) throw new IOException("指定されたファイルパスに異常があります。");
            if (columns == null) throw new ArgumentNullException("設定するカラムが null です。");


        }

        #endregion

        #region Private Methods

        #endregion
    }
}
