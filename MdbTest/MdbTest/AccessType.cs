using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MdbTest
{
    /// <summary>
    /// <see cref="Access"/> 列挙型は、DB ファイルを作成した Access 種別を表します。
    /// </summary>
    public enum AccessType : int
    {
        /// <summary>
        /// 未定義
        /// </summary>
        UnDefined = -1,
        /// <summary>
        /// 不明
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// Access 2.0
        /// </summary>
        Access2 = 2,
        /// <summary>
        /// Access 7.0 or (Access 95)
        /// </summary>
        Access7_95 = 6,
        /// <summary>
        /// Access 97
        /// </summary>
        Access97 = 7,
        /// <summary>
        /// Access 2000
        /// </summary>
        Access2000 = 800,
        /// <summary>
        /// Access 2002
        /// </summary>
        Access2002 = 802,
        /// <summary>
        /// Access 2003
        /// </summary>
        Access2003 = 803,
    }
}
