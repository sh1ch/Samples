using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComPortDetectionSample
{
    /// <summary>
    /// <see cref="SerialPortObserver"/> クラスは、シリアルポートの状態を観察するクラスです。
    /// </summary>
    public class SerialPortObserver : WindowMessageObserver
    {
        #region Fields

        private const int WM_DEVICECHANGE = 0x0219;
        private const int DBT_DEVICEARRIVAL = 0x8000;
        private const int DBT_DEVICEQUERYREMOVE = 0x8001;
        private const int DBT_DEVICEREMOVECOMPLETE = 0x8004;

        private IEnumerable<string> _OldPortNames;

        #endregion

        #region Properties

        /// <summary>
        /// 不明なシリアルポートの状態変化の通知を無視するかどうかを示す値です。
        /// </summary>
        public bool IsIgnoredUnknown { get; set; }

        /// <summary>
        /// 現在のコンピューターの COM ポート名のデータ コレクションを取得します。
        /// </summary>
        public IEnumerable<string> ComPortNames { get; private set; }

        #endregion

        #region Initializes

        /// <summary>
        /// <see cref="SerialPortObserver"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        public SerialPortObserver()
        {

        }

        #endregion

        #region Events

        /// <summary>
        /// シリアルポートの変更によって発生するイベントです。
        /// </summary>
        public event EventHandler<WM_DeviceChangedEventArgs> Changed;

        #endregion

        #region Public Methods

        /// <summary>
        /// シリアルポートの状態に関するイベントをサブスクライブします。
        /// </summary>
        public void Start()
        {
            ComPortNames = SerialPort.GetPortNames();

            Receiver.RegisterMessage(WM_DEVICECHANGE);

            Receiver.RegisterEvent((msg) =>
            {
                var wparam = msg.WParam.ToInt64();
                var type = WM_DeviceChangeType.Unknown;
                var newPorts = SerialPort.GetPortNames();

                switch (wparam)
                {
                    case DBT_DEVICEARRIVAL: type = WM_DeviceChangeType.Arrival; break;
                    case DBT_DEVICEQUERYREMOVE: type = WM_DeviceChangeType.Removing; break;
                    case DBT_DEVICEREMOVECOMPLETE: type = WM_DeviceChangeType.Removal; break;
                }

                // 不明な変更通知を無視する例外
                if (IsIgnoredUnknown && type == WM_DeviceChangeType.Unknown) return;

                // 接続 or 削除のときにポートを更新
                if (type == WM_DeviceChangeType.Arrival || type == WM_DeviceChangeType.Removal)
                {
                    _OldPortNames = ComPortNames;
                    ComPortNames = newPorts;
                }

                Changed?.Invoke(this, new WM_DeviceChangedEventArgs(type, ComPortNames, _OldPortNames, msg));
            });
        }

        #endregion

        #region Private Methods

        #endregion
    }
}
