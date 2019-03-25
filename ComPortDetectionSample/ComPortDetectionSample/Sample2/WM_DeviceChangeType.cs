using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComPortDetectionSample
{
    /// <summary>
    /// <see cref="DeviceChangeType"/> クラスは、WM_DEVICECHANGE の種類を表す列挙型です。
    /// </summary>
    public enum WM_DeviceChangeType : int
    {
        /// <summary>
        /// デバイスの接続を表す種別です。
        /// </summary>
        Arrival = 2,
        /// <summary>
        /// デバイスの除去を表す種別です。
        /// </summary>
        Removal = 3,
        /// <summary>
        /// デバイスの除去中を表す種別です。
        /// </summary>
        Removing = 4,
        /// <summary>
        /// 不明な種別です。
        /// </summary>
        Unknown = 0,
    }
}
