using HeritageFramework.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace PrintSample
{
    /// <summary>
    /// <see cref="PreviewBase"/> クラスは、印刷プレビューするためのクラスです。
    /// </summary>
    public class PreviewBase : INotifyPropertyChanged
    {
        #region Properties

        private PrintController _PrintController = new PrintController();

        /// <summary>
        /// 印刷用インスタンスを取得します。
        /// </summary>
        protected PrintController PrintController
        {
            get
            {
                return _PrintController;
            }
        }

        private FixedDocument _Document;

        /// <summary>
        /// 印刷するドキュメントデータを取得します。
        /// </summary>
        public FixedDocument Document
        {
            get { return _Document; }
            protected set
            {
                if (Document == value) return;

                _Document = value;
                PropertyChanged.Raise(this, nameof(Document));
            }
        }

        private bool _IsValid;

        /// <summary>
        /// 設定の状態が正常かどうかを示す値を取得します。
        /// </summary>
        public bool IsValid
        {
            get { return _IsValid; }
            set
            {
                _IsValid = value;
                PropertyChanged.Raise(this, nameof(IsValid));
            }
        }

        /// <summary>
        /// プレビューの拡大率を取得または設定します。
        /// </summary>
        public double Zoom { get; set; } = 50;

        #endregion

        #region Initializes

        /// <summary>
        /// <see cref="PreviewBase"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        public PreviewBase() { }

        #endregion

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Public Methods

        /// <summary>
        /// プレビューしている <see cref="Document"/> を印刷します。
        /// </summary>
        /// <param name="settings">印刷設定。</param>
        /// <exception cref="NullReferenceException">印刷するドキュメントは null です。</exception>
        /// <exception cref="PrintQueueException">印刷準備ができていません。</exception>
        public void Print(PrintSettings settings = null)
        {
            if (Document == null)
            {
                throw new NullReferenceException("印刷するドキュメントは null です。");
            }

            if (settings != null)
            {
                UpdateSettings(settings);
            }

            PrintController.Print(Document);
        }

        /// <summary>
        /// プリンター性能に影響する印刷設定を更新します。
        /// </summary>
        /// <param name="settings">印刷設定。</param>
        public void UpdateCapabilities(PrintSettings settings)
        {
            PrintController.UpdateQueue(settings.SelectedPrinter);

            var caps = PrintController.PrintCapabilities.DuplexingCapability;

            if (caps.Contains(settings.Duplexing))
            {
                PrintController.PrintTicket.Duplexing = settings.Duplexing;
            }
            else
            {
                PrintController.PrintTicket.Duplexing = Duplexing.OneSided;
            }
        }

        /// <summary>
        /// 基本的な設定を利用して <see cref="FrameworkElement"/> の <see cref="FixedDocument"/> を作成します。
        /// </summary>
        /// <param name="v">ドキュメントに変換する要素。</param>
        /// <param name="stretch">ドキュメントの伸縮。</param>
        public void UpdateTemplateDocument(FrameworkElement v, Stretch stretch)
        {
            UpdateTemplateDocuments(Enumerable.Repeat(v, 1), stretch);
        }

        /// <summary>
        /// 基本的な設定を利用して <see cref="FrameworkElement"/> の <see cref="FixedDocument"/> を作成します。
        /// </summary>
        /// <param name="v">ドキュメントに変換する要素のコレクション。</param>
        /// <param name="stretch">ドキュメントの伸縮。</param>
        public void UpdateTemplateDocuments(IEnumerable<FrameworkElement> v, Stretch stretch)
        {
            var fixedDocument = new FixedDocument();

            foreach (var sv in v)
            {
                var page = CreatePage();
                var size = page.PrintableSize;

                // 印刷データ
                var viewbox = new Viewbox
                {
                    Width = size.Width,
                    Height = size.Height,
                    Stretch = stretch,
                };

                viewbox.Child = sv;

                viewbox.Arrange(new Rect(new Point(0, 0), size));
                viewbox.Measure(size);
                viewbox.UpdateLayout();

                page.FinalizePage(viewbox);

                fixedDocument.Pages.Add(page.PageContent);
            }

            Document = fixedDocument;
        }

        /// <summary>
        /// 現在の印刷ジョブの状態を表すコレクションを取得します。
        /// </summary>
        /// <returns>印刷ジョブの状態を表すコレクション。</returns>
        public PrintJobInfoCollection GetPrintJobInfo()
        {
            return PrintController.PrintQueue?.GetPrintJobInfoCollection();
        }

        #endregion

        #region Protected Methods

        protected Page CreatePage()
        {
            var point = _PrintController.PritablePoint;
            var area = _PrintController.PrintableArea;

            var page = PageBuilder.Create()
                                  .PageSize(area)
                                  .OriginPoint(point)
                                  .Margin(new Thickness(0, 0, 0, 0))
                                  .Build();

            return page;
        }

        protected void UpdateSettings(PrintSettings settings)
        {
            PrintController.PrintTicket.CopyCount = settings.Copies;
        }

        #endregion
    }
}
