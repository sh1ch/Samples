using HeritageFramework;
using HeritageFramework.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AreaCalculator.Models.SignalSelection
{
    /// <summary>
    /// <see cref="SimpleSignalSelection"/> クラスは、先頭から順番に単純な取得をするクラスです。
    /// </summary>
    public class SimpleSignalSelection : ISignalSelection
    {
        #region Initializes

        /// <summary>
        /// <see cref="SimpleSignalSelection"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        public SimpleSignalSelection()
        {

        }

        #endregion

        #region Public Methods

        public int[] Select(IEnumerable<string> signalTexts, int count)
        {
            var selectedSignalTexts = signalTexts.Take(count); // 10 個取得 (0.1秒刻み)

            if (selectedSignalTexts?.Count() != count)
            {
                throw new ArgumentOutOfRangeException($"指定した列挙子から、要素数({count})を取得できませんでした。");
            }

            var signals = selectedSignalTexts.Select(p => int.Parse(p)).ToArray();

            return signals;
        }

        #endregion
    }
}
