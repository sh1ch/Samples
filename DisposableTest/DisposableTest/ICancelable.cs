using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisposableTest
{
    /// <summary>
    /// <see cref="ICancelable"/> インターフェースは、廃棄状態を調べるプロパティを定義します。
    /// </summary>
    public interface ICancelable : IDisposable
    {
        /// <summary>
        /// オブジェクトが破棄されているかどうかを示す値を取得します。
        /// </summary>
        bool IsDisposed { get; }
    }
}
