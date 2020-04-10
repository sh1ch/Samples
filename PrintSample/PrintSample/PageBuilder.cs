using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace PrintSample
{
    /// <summary>
    /// <see cref="PageBuilder"/> クラスは、<see cref="Page"/> の生成をサポートするためのクラスです。
    /// </summary>
    public class PageBuilder
    {
        #region Fields

        private readonly int _DPI;
        private readonly Thickness _Margin;
        private readonly Size _PageSize;
        private readonly Point _OriginPoint;

        #endregion

        #region Initializes

        private PageBuilder(int dpi, Thickness margin, Size pageSize, Point originPoint) =>
            (_DPI, _Margin, _PageSize, _OriginPoint) = (dpi, margin, pageSize, originPoint);

        #endregion

        #region Public Methods

        /// <summary>
        /// <see cref="Page"/> インスタンスの生成を開始します。
        /// </summary>
        /// <param name="dpi">印刷するドット密度の単位。</param>
        /// <returns></returns>
        public static PageBuilder Create(int dpi = 72) => 
            new PageBuilder(dpi, default(Thickness), Size.Empty, default(Point));

        /// <summary>
        /// ユーザーカスタムで追加された余白を設定します。
        /// </summary>
        /// <param name="margin">余白の大きさ。</param>
        /// <returns>ビルダー インスタンス。</returns>
        public PageBuilder Margin(Thickness margin) => new PageBuilder(_DPI, margin, _PageSize, _OriginPoint);

        /// <summary>
        /// 用紙の基本サイズを設定します。
        /// </summary>
        /// <param name="pageSize">ページの大きさ。</param>
        /// <returns>ビルダー インスタンス。</returns>
        public PageBuilder PageSize(Size pageSize) => new PageBuilder(_DPI, _Margin, pageSize, _OriginPoint);

        /// <summary>
        /// プリンター性能による余白を設定します。
        /// </summary>
        /// <param name="originPoint">余白の大きさ。</param>
        /// <returns>ビルダー インスタンス。</returns>
        public PageBuilder OriginPoint(Point originPoint) => new PageBuilder(_DPI, _Margin, _PageSize, originPoint);

        /// <summary>
        /// 必要な初期化条件を設定した <see cref="Page"/> のインスタンスを生成します。
        /// </summary>
        /// <exception cref="InvalidOperationException">必須のパラメーターが初期化されていません。</exception>
        /// <returns>初期化済のインスタンス。</returns>
        public Page Build()
        {
            var page = new Page();

            if (_PageSize == null || _PageSize.IsEmpty || _PageSize.Width == 0 || _PageSize.Height == 0)
            {
                throw new InvalidOperationException("必須のパラメーターが初期化されていません。");
            }

            page.BuildSize(_PageSize, _OriginPoint, _Margin);
            page.BuildContent(_PageSize.Width, _PageSize.Height, _OriginPoint);
            page.BuildFixedPage(_DPI, _PageSize.Width, _PageSize.Height, _OriginPoint, _Margin);

            return page;
        }

        #endregion

        #region Private Methods

        #endregion
    }
}
