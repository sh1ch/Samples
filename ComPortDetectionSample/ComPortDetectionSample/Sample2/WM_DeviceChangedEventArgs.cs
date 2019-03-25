using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ComPortDetectionSample
{
    /// <summary>
    /// <see cref="WM_DeviceChangedEventArgs"/> クラスは、シリアルポートの変更を表現したイベントデータを提供するクラスです。
    /// </summary>
    public class WM_DeviceChangedEventArgs : EventArgs
    {
        #region Properties

        /// <summary>
        /// 発生したイベントの種別を取得します。
        /// </summary>
        public WM_DeviceChangeType Type { get; }

        /// <summary>
        /// 現在に存在しているシリアルポートの列挙子を取得します。
        /// </summary>
        public IEnumerable<string> NewPortNames { get; }

        /// <summary>
        /// 更新の前に存在していたシリアルポートの列挙子を取得します。
        /// </summary>
        public IEnumerable<string> OldPortNames { get; }

        /// <summary>
        /// 発生した Windows メッセージを取得します。
        /// </summary>
        public Message Msg { get; }

        #endregion

        #region Initializes

        /// <summary>
        /// <see cref="WM_DeviceChangedEventArgs"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="type">発生したイベントの種別。</param>
        /// <param name="newPorts">現在に存在しているシリアルポートの列挙子。</param>
        /// <param name="oldPorts">過去に存在しているシリアルポートの列挙子。</param>
        /// <param name="msg">発生したWindows メッセージ。</param>
        public WM_DeviceChangedEventArgs(WM_DeviceChangeType type, IEnumerable<string> newPorts, IEnumerable<string> oldPorts, Message msg)
        {
            Type = type;
            NewPortNames = newPorts;
            OldPortNames = oldPorts;
            Msg = msg;
        }

        #endregion
    }
}
