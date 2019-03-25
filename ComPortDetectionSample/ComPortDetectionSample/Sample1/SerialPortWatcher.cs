using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace ComPortDetectionSample
{
    /// <summary>
    /// <see cref="SerialPortWatcher"/> クラスは、シリアルポートの状態を観察するクラスです。
    /// </summary>
    public class SerialPortWatcher : IDisposable
    {
        #region Fields

        private ManagementEventWatcher _Watcher;

        private EventArrivedEventHandler _Changed;
        private EventHandler _Disposed;

        #endregion

        #region Properties

        private static readonly SerialPortWatcher _Instance = new SerialPortWatcher();

        /// <summary>
        /// <see cref="SerialPortWatcher"/> クラスのインスタンスを取得します。
        /// </summary>
        public static SerialPortWatcher Instance
        {
            get { return _Instance; }
        }

        /// <summary>
        /// 現在のコンピューターの COM ポート名のデータ コレクションを取得します。
        /// </summary>
        public IEnumerable<string> ComPortNames { get; private set; }

        #endregion

        #region Initializes

        /// <summary>
        /// <see cref="SerialPortWatcher"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        private SerialPortWatcher()
        {
            _Changed = (sender, e) =>
            {
                int value = 0;
                var valueText = e.NewEvent.GetPropertyValue("EventType").ToString();

                UpdateComPortNames();
                int.TryParse(valueText, out value);

                var type = (DeviceChangeType)value;
                var newPorts = SerialPort.GetPortNames();

                Changed?.Invoke(this, new DeviceChangedEventArgs(type, newPorts, e));
            };

            _Disposed += (sender, e) =>
            {
                Disposed?.Invoke(this, e);
            };

            UpdateComPortNames();
        }

        #endregion

        #region Events

        /// <summary>
        /// シリアルポートの変更によって発生するイベントです。
        /// </summary>
        public event EventHandler<DeviceChangedEventArgs> Changed;

        /// <summary>
        /// <see cref="Component.Dispose"/> メソッドの呼び出しによって発生するイベントです。
        /// </summary>
        public event EventHandler Disposed;

        #endregion

        #region Public Methods

        /// <summary>
        /// 使用したすべてのリソースを開放します。
        /// </summary>
        public void Dispose()
        {
            if (_Watcher != null)
            {
                _Watcher.EventArrived -= _Changed;
                _Watcher.Disposed -= _Disposed;

                _Watcher.Stop();
                _Watcher = null;
            }
        }

        /// <summary>
        /// シリアルポートの状態に関するイベントをサブスクライブします。
        /// </summary>
        public void Start()
        {
            var query = new WqlEventQuery("SELECT * FROM Win32_DeviceChangeEvent");

            _Watcher = new ManagementEventWatcher(query);

            _Watcher.EventArrived += _Changed;
            _Watcher.Disposed += _Disposed;

            _Watcher.Start();
        }

        #endregion

        #region Private Methods

        private void UpdateComPortNames()
        {
            var com = SerialPort.GetPortNames();
            
            ComPortNames = new List<string>(com);
        }

        #endregion

    }
}
