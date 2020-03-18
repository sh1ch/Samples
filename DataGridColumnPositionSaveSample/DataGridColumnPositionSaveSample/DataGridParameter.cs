using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DataGridColumnPositionSaveSample
{
    /// <summary>
    /// <see cref="DataGridParameter"/> クラスは、<see cref="DataGrid"/> のアプリケーション終了時に保存するパラメーター クラスです。
    /// </summary>
    public class DataGridParameter
    {
        #region Properties

        /// <summary>
        /// 列を区別するテキストを取得または設定します。
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// 列の表示位置を決める値を取得または設定します。
        /// </summary>
        public int DisplayIndex { get; set; }

        /// <summary>
        /// 列の横幅を決める値を取得または設定します。
        /// </summary>
        public double Width { get; set; }

        #endregion

        #region Initializes

        /// <summary>
        /// <see cref="DataGridParameter"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        public DataGridParameter() { }

        #endregion
    }
}
