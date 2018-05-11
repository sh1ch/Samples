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
    /// <see cref="ChangingSignalSelection"/> クラスは、データを比較して差があるデータを優先して取得をするクラスです。
    /// </summary>
    public class ChangingSignalSelection : ISignalSelection
    {
        #region Properties

        /// <summary>
        /// 優先する比較差の基準値を取得または設定します。
        /// </summary>
        public int PreferredReference { get; set; } = 10;

        #endregion

        #region Initializes

        /// <summary>
        /// <see cref="SimpleSignalSelection"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        public ChangingSignalSelection()
        {

        }

        #endregion

        #region Public Methods

        public int[] Select(IEnumerable<string> signalTexts, int count)
        {
            if ((signalTexts?.Count() ?? 0) <= 0)
            {
                throw new ArgumentOutOfRangeException($"指定した列挙子はデータがありません。");
            }

            var signals = signalTexts.Select(p => int.Parse(p)).Where(p => p > -1000).ToArray();
            var checkPoints = new bool[signals.Count()];

            int p1 = signals[0];

            // 変化の差を持つポイントにチェックをつける
            for (var i = 1; i < signals.Count(); i++)
            {
                if ((Math.Abs(p1 - signals[i])) >= PreferredReference)
                {
                    checkPoints[i] = true;
                }

                p1 = signals[i];
            }

            var checkCount = checkPoints.Where(p => p == true)?.Count() ?? 0;

            if (checkCount < count)
            {
                // 指定数以下の点だったとき
                for (var i = 0; i < checkPoints.Count(); i++)
                {
                    if (checkPoints[i] == false)
                    {
                        checkPoints[i] = true;
                        checkCount += 1;
                        if (checkCount == count) break;
                    }
                }
            }
            else if (checkCount > count)
            {
                // 指定数以上の点で差が発生していたとき
                for (var i = 0; i < checkPoints.Count(); i++)
                {
                    if (checkPoints[i] == true)
                    {
                        checkPoints[i] = false;
                        checkCount -= 1;
                        if (checkCount == count) break;
                    }
                }
            }

            var selectedSignals = new List<int>();

            for (var i = 0; i < checkPoints.Count(); i++)
            {
                if (checkPoints[i])
                {
                    selectedSignals.Add(signals[i]);
                }
            }

            return selectedSignals.ToArray();
        }

        #endregion
    }
}
