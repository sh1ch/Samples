using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileOpenDialogSample.Dialogs
{
    /// <summary>
    /// <see cref="DialogResult"/> 列挙型は、ダイアログ ボックスの戻り値を示す識別子を表します。
    /// </summary>
    public enum DialogResult : int
    {
        /// <summary>
        /// ダイアログ ボックスの戻り値は Nothing です。モーダル ダイアログ ボックスの実行が継続します。
        /// </summary>
        None = 0,
        /// <summary>
        /// ダイアログ ボックスの戻り値は OK です。
        /// </summary>
        OK = 1,
        /// <summary>
        /// ダイアログ ボックスの戻り値は Cancel です。
        /// </summary>
        Cancel = 2,
        /// <summary>
        /// ダイアログ ボックスの戻り値は Abort です。
        /// </summary>
        Abort = 3,
        /// <summary>
        /// ダイアログ ボックスの戻り値は Retry です。
        /// </summary>
        Retry = 4,
        /// <summary>
        /// ダイアログ ボックスの戻り値は Ignore です。
        /// </summary>
        Ignore = 5,
        /// <summary>
        /// ダイアログ ボックスの戻り値は Yes です。
        /// </summary>
        Yes = 6,
        /// <summary>
        /// ダイアログ ボックスの戻り値は No です。
        /// </summary>
        No = 7
    }

}
