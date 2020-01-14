using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InheritdocTest
{
    /// <summary>
    /// <see cref="ISample"/> インターフェースは、コメントのテストを定義します。
    /// </summary>
    public interface ISample
    {
        /// <summary>
        /// <see cref="ISample"/> インターフェースで定義したプロパティのコメントです。
        /// </summary>
        string Property1 { get; set; }

        /// <summary>
        /// <see cref="ISample"/> インターフェースで定義したイベントのコメントです。
        /// </summary>
        event EventHandler Occurred;

        /// <summary>
        /// <see cref="ISample"/> インターフェースで定義したメソッドのコメントです。
        /// </summary>
        void Method1();
    }
}
