using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace PrintSample
{
    /// <summary>
    /// <see cref="Page"/> クラスは、印刷するページを表現するクラスです。
    /// </summary>
    public class Page
    {
        #region Properties

        /// <summary>
        /// 印刷するドット密度の単位を取得します。
        /// </summary>
        public int Dpi { get; }

        /// <summary>
        /// ページ要素の情報を取得します。
        /// </summary>
        public PageContent PageContent { get; private set; }

        /// <summary>
        /// ページ要素を取得します。
        /// </summary>
        public FixedPage FixedPage { get; private set; }

        /// <summary>
        /// ページの余白を取得します。
        /// </summary>
        public Thickness Margin { get; private set; }

        /// <summary>
        /// 印刷可能範囲の大きさを取得します。
        /// </summary>
        public Size PrintableSize { get; private set; }

        /// <summary>
        /// プリンター依存の余白を取得します。
        /// </summary>
        public Point OriginPoint { get; private set; }

        #endregion

        #region Initializes

        /// <summary>
        /// <see cref="Page"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        public Page() : this(72) { }

        /// <summary>
        /// <see cref="Page"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="dpi">印刷ドット密度の単位。</param>
        public Page(int dpi) 
        {
            Dpi = dpi;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// ページ・余白のサイズを初期化します。
        /// </summary>
        /// <param name="printableSize">印刷可能なページの大きさ。</param>
        /// <param name="originPoint">プリンター依存の余白。</param>
        /// <param name="margin">拡張余白。</param>
        public void BuildSize(Size printableSize, Point originPoint, Thickness margin)
        {
            PrintableSize = printableSize;
            OriginPoint = originPoint;
            Margin = margin;
        }

        /// <summary>
        /// <see cref="PageContent"/> を初期化します。
        /// </summary>
        /// <param name="width">要素の幅。</param>
        /// <param name="height">要素の高さ。</param>
        /// <param name="originPoint">プリンター性能による必須余白。</param>
        public void BuildContent(double width, double height, Point originPoint)
        {
            var content = new PageContent()
            {
                Width = width + originPoint.X * 2,
                Height = height + originPoint.Y * 2
            };

            OriginPoint = originPoint;
            PageContent = content;
        }

        /// <summary>
        /// <see cref="PageContent"/> を初期化します。
        /// </summary>
        /// <param name="dpi">印刷するドット密度の単位。</param>
        /// <param name="width">要素の幅。</param>
        /// <param name="height">要素の高さ。</param>
        /// <param name="originPoint">プリンター性能による必須余白。</param>
        /// <param name="margin">余白。</param>
        public void BuildFixedPage(int dpi, double width, double height, Point originPoint, Thickness margin)
        {
            var mm = ToMilliThickness(margin, dpi);

            var page = new FixedPage()
            {
                Width = width + originPoint.X * 2,
                Height = height + originPoint.Y * 2
            };

            page.Margin = new System.Windows.Thickness(originPoint.X + mm.Left, originPoint.Y + mm.Top, originPoint.X + mm.Right, originPoint.Y + mm.Bottom);
            OriginPoint = originPoint;
            Margin = margin;
            FixedPage = page;
        }

        /// <summary>
        /// 作成したページ要素を確定させます。
        /// <para>
        /// 確定した要素を <see cref="PageContentCollection"/> に追加することができます。
        /// </para>
        /// </summary>
        /// <exception cref="ArgumentNullException" />
        public void FinalizePage(FrameworkElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException();
            }

            FixedPage.Children.Add(element);
            PageContent.Child = FixedPage;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// mm で示された <see cref="Thickness"/> をドット単位に変換した <seealso cref="System.Windows.Thickness"/> を取得します。
        /// </summary>
        private System.Windows.Thickness ToMilliThickness(Thickness thickness, int dpi)
        {
            return new System.Windows.Thickness
            (
                ConvertMilliToDots(thickness.Left, dpi), ConvertMilliToDots(thickness.Top, dpi),
                ConvertMilliToDots(thickness.Right, dpi), ConvertMilliToDots(thickness.Bottom, dpi)
            );
        }

        /// <summary>
        /// inch で示された <see cref="Thickness"/> をドット単位に変換した <seealso cref="System.Windows.Thickness"/> を取得します。
        /// </summary>
        private System.Windows.Thickness ToInchThickness(Thickness thickness, int dpi)
        {
            return new System.Windows.Thickness
                (
                    ConvertInchToDots(thickness.Left, dpi), ConvertInchToDots(thickness.Top, dpi),
                    ConvertInchToDots(thickness.Right, dpi), ConvertInchToDots(thickness.Bottom, dpi)
                );
        }

        /// <summary>
        /// mm の大きさを表す値を ドット単位の大きさに変換します。
        /// </summary>
        /// <param name="value">mm の値。</param>
        /// <param name="dpi">ドット密度。</param>
        /// <returns>ドット単位の大きさ。</returns>
        private double ConvertMilliToDots(double value, int dpi)
        {
            var inch = 25.4D; // 1inch = 25.4mm
            return (value / inch) * dpi;
        }

        /// <summary>
        /// inch の大きさを表す値を ドット単位の大きさに変換します。
        /// </summary>
        /// <param name="value">inch の値。</param>
        /// <param name="dpi">ドット密度。</param>
        /// <returns>ドット単位の大きさ。</returns>
        private double ConvertInchToDots(double value, int dpi)
        {
            return value * dpi;
        }

        #endregion
    }
}
