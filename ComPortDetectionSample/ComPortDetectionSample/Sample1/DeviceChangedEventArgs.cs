using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace ComPortDetectionSample
{
    /// <summary>
    /// <see cref="DeviceChangedEventArgs"/> クラスは、シリアルポートの変更を表現したイベントデータを提供するクラスです。
    /// </summary>
    public class DeviceChangedEventArgs : EventArgs
    {
        #region Properties

        /// <summary>
        /// 発生したイベントの種別を取得します。
        /// </summary>
        public DeviceChangeType Type { get; }

        /// <summary>
        /// 現在に存在しているシリアルポートの列挙子を取得します。
        /// </summary>
        public IEnumerable<string> NewPortNames { get; }

        /// <summary>
        /// 発生した <see cref="EventArrivedEventArgs"/> イベントデータを取得します。
        /// </summary>
        public EventArrivedEventArgs Args { get; }

        #endregion

        #region Initializes

        /// <summary>
        /// <see cref="DeviceChangedEventArgs"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="type">発生したイベントの種別。</param>
        /// <param name="newPorts">現在に存在しているシリアルポートの列挙子。</param>
        /// <param name="args">発生したイベントの詳細。</param>
        public DeviceChangedEventArgs(DeviceChangeType type, IEnumerable<string> newPorts, EventArrivedEventArgs args)
        {
            Type = type;
            NewPortNames = newPorts;
            Args = args;
        }

        #endregion
    }
}
