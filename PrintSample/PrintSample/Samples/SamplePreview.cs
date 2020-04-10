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
    /// <see cref="SamplePreview"/> クラスは、サンプルの印刷プレビュークラスです。
    /// </summary>
    public class SamplePreview : PreviewBase
    {
        #region Initializes

        /// <summary>
        /// <see cref="SamplePreview"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        public SamplePreview() { }

        #endregion

        #region Public Methods

        public void UpdateDocument(SampleOption option)
        {
            List<FrameworkElement> vCollection = new List<FrameworkElement>();

            for (var i = 0; i < option.Pages; i++)
            { 
                var v = new SamplePage();
                var vm = option;
               
                v.DataContext = vm;
                vCollection.Add(v);
            }

            UpdateTemplateDocuments(vCollection, Stretch.Uniform);
        }

        [Obsolete]
        public void SelfUpdateDocument(SampleOption option)
        {
            var page = CreatePage();
            var size = page.PrintableSize;

            var viewbox = new Viewbox
            {
                Width = size.Width,
                Height = size.Height,
                Stretch = System.Windows.Media.Stretch.Uniform, // 縦横比を維持してサイズの最終調整
            };
            var v = new SamplePage();
            var vm = option;

            viewbox.Child = v;
            v.DataContext = vm;

            viewbox.Arrange(new Rect(new Point(0, 0), size));
            viewbox.Measure(size);
            viewbox.UpdateLayout();

            page.FinalizePage(viewbox);

            // ドキュメントの作成
            var fixedDocument = new FixedDocument();
            fixedDocument.Pages.Add(page.PageContent);

            Document = fixedDocument;
        }

        #endregion
    }
}
