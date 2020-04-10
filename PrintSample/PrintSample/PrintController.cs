using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace PrintSample
{
    /// <summary>
    /// <see cref="PrintController"/> クラスは、<see cref="LocalPrintServer"/> のワークをアダプトし簡易な操作として提供します。
    /// </summary>
    public class PrintController
    {
        #region Fields

        private LocalPrintServer _PrintServer = new LocalPrintServer();

        #endregion

        #region Properties

        /// <summary>
        /// 印刷ジョブを取得します。
        /// </summary>
        public PrintQueue PrintQueue { get; private set; }

        /// <summary>
        /// 印刷ジョブ設定を取得します。
        /// </summary>
        public PrintTicket PrintTicket { get; private set; }

        /// <summary>
        /// 印刷機能の定義を取得します。
        /// </summary>
        public PrintCapabilities PrintCapabilities { get; private set; }

        /// <summary>
        /// 印刷キューの準備ができているかどうかを示す値を取得します。
        /// </summary>
        public bool IsEnabled
        {
            get => (_PrintServer != null && PrintQueue != null && PrintTicket != null);
        }

        /// <summary>
        /// 印刷領域における入力位置を取得します。
        /// </summary>
        public Point PritablePoint
        {
            get => new Point(PrintCapabilities.PageImageableArea.OriginWidth, PrintCapabilities.PageImageableArea.OriginHeight);
        }

        /// <summary>
        /// 印刷領域の大きさを取得します。
        /// </summary>
        public Size PrintableArea
        {
            get => new Size
                (
                    PrintCapabilities.PageImageableArea.ExtentWidth,
                    PrintCapabilities.PageImageableArea.ExtentHeight
                );
        }

        #endregion

        #region Initializes

        /// <summary>
        /// <see cref="PrintController"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        public PrintController() { }

        #endregion

        #region Events

        #endregion

        #region Public Methods

        /// <summary>
        /// 指定した <see cref="FixedDocument"/> を印刷します。
        /// </summary>
        /// <exception cref="PrintQueueException">印刷準備ができていません。</exception>
        /// <param name="doc">印刷ドキュメント。</param>
        public void Print(FixedDocument doc)
        {
            if (IsEnabled == false)
            {
                throw new PrintQueueException("印刷準備ができていません。");
            }

            var xpsWriter = PrintQueue.CreateXpsDocumentWriter(PrintQueue);
            xpsWriter.WritingPrintTicketRequired += (sender, e) => { e.CurrentPrintTicket = PrintTicket; };

            xpsWriter.Write(doc, PrintTicket);
        }

        /// <summary>
        /// 印刷ジョブを更新します。
        /// </summary>
        /// <param name="name">印刷するプリンター名。</param>
        public void UpdateQueue(string name)
        {
            var selectedQueue = _PrintServer.GetPrintQueues().FirstOrDefault(p => p.FullName == name);

            if (selectedQueue == null)
            {
                throw new PrintQueueException("指定したプリンターが取得できません。");
            }

            PrintQueue = selectedQueue;
            UpdateTicketAndCapabilities();
        }

        /// <summary>
        /// 印刷ジョブをデフォルトのプリンターに更新します。
        /// </summary>
        public void UpdateQueueToDefault()
        {
            PrintQueue = _PrintServer.DefaultPrintQueue;
            UpdateTicketAndCapabilities();
        }

        #endregion

        #region Private Methods

        private void UpdateTicketAndCapabilities()
        {
            PrintTicket = PrintQueue.DefaultPrintTicket;

            if (PrintTicket == null)
            {
                throw new PrintQueueException("プリンターオプションが取得できません。");
            }

            PrintTicket.PageMediaSize = new PageMediaSize(PageMediaSizeName.ISOA4);
            PrintCapabilities = PrintQueue.GetPrintCapabilities(PrintTicket);
        }

        #endregion
    }
}
