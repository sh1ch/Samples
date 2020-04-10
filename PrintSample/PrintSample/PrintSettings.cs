using HeritageFramework.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Printing;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PrintSample
{
    /// <summary>
    /// <see cref="PrintSettings"/> クラスは、印刷設定をするクラスです。
    /// </summary>
    public class PrintSettings : INotifyPropertyChanged
    {
        #region Fields

        private LocalPrintServer _LocalPrintServer = new LocalPrintServer();

        #endregion

        #region Properties

        /// <summary>
        /// 使用できるプリンター名のコレクションを取得します。
        /// </summary>
        public IEnumerable<string> Printers
        {
            get
            {
                var printers = new List<string>();

                foreach (var queue in _LocalPrintServer.GetPrintQueues())
                {
                    printers.Add(queue.FullName);
                }

                return printers.Count > 0 ? printers : null;
            }
        }

        private string _SelectedPrinter;

        /// <summary>
        /// 選択されているプリンター名を取得または設定します。
        /// </summary>
        public string SelectedPrinter
        {
            get { return _SelectedPrinter; }
            set
            {
                if (SelectedPrinter == value) return;

                _SelectedPrinter = value;

                if (!CanDuplexing(SelectedPrinter, Duplexing))
                {
                    Duplexing = Duplexing.OneSided;
                }

                PropertyChanged.Raise(this, nameof(SelectedPrinter));
                PropertyChanged.Raise(this, nameof(IsSupportedDuplexing));
                PrinterChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private Duplexing _Duplexing = Duplexing.OneSided;

        /// <summary>
        /// 片面、両面印刷の設定値を取得または設定します。
        /// </summary>
        public Duplexing Duplexing
        {
            get { return _Duplexing; }
            set
            {
                if (Duplexing == value) return;

                if (CanDuplexing(SelectedPrinter, value))
                {
                    _Duplexing = value;
                }
                else
                {
                    _Duplexing = Duplexing.OneSided;
                }
                
                PropertyChanged.Raise(this, nameof(Duplexing));
                PropertyChanged.Raise(this, nameof(IsSupportedDuplexing));
            }
        }

        /// <summary>
        /// 両面印刷に対応しているかどうかを示す値を取得します。
        /// </summary>
        public bool IsSupportedDuplexing
        {
            get 
            {
                if (!Printers.Contains(SelectedPrinter)) return false;

                var queue = _LocalPrintServer.GetPrintQueue(SelectedPrinter);

                var duplexings = queue.GetPrintCapabilities()?.DuplexingCapability;

                if (duplexings == null || duplexings.Count() <= 0) return false;

                return duplexings.Contains(Duplexing.OneSided) && 
                       duplexings.Contains(Duplexing.TwoSidedLongEdge) &&
                       duplexings.Contains(Duplexing.TwoSidedShortEdge);
            }
        }

        /// <summary>
        /// 部数を取得または設定します。
        /// </summary>
        public int Copies { get; set; } = 1;

        #endregion

        #region Initializes

        /// <summary>
        /// <see cref="PrintSettings"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        public PrintSettings() 
        {
            // デフォルトのプリンターを設定
            SelectedPrinter = GetDefaultPrinter();
        }

        #endregion

        #region Events

        public event EventHandler PrinterChanged;
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Public Methods

        /// <summary>
        /// プリンターの OS 設定を表示します。
        /// </summary>
        /// <exception cref="System.ComponentModel.Win32Exception">関連付けられているファイルを開いているときにエラーが発生しました。</exception>
        public void OpenMsSettings()
        {
            try
            {
                // Windows 10 の設定で表示する
                Process.Start("ms-settings:printers");
            }
            catch
            {
                try
                {
                    // コントロールパネルで再度試行
                    Process.Start("control.exe", "/name Microsoft.DevicesAndPrinters");
                }
                catch
                {
                    // すべて失敗したとき例外
                    throw;
                }
            }
        }

        #endregion

        #region Private Methods

        private string GetDefaultPrinter()
        {
            var localPrinter = _LocalPrintServer.DefaultPrintQueue;

            return Printers.FirstOrDefault(p => p == localPrinter.FullName) ?? "";
        }

        private bool CanDuplexing(string printerName, Duplexing duplexing)
        {
            if (!Printers.Contains(printerName)) return false;

            var queue = _LocalPrintServer.GetPrintQueue(printerName);
            var capabilities = queue.GetPrintCapabilities();

            return capabilities.DuplexingCapability.Contains(duplexing);
        }

        #endregion
    }
}
