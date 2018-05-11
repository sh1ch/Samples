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
    /// <see cref="SignalSelectionType"/> クラスは、シグナル選択の種別を表す列挙型です。
    /// </summary>
    public enum SignalSelectionType : int
    {
        /// <summary>
        /// 単純な取得。
        /// </summary>
        Simple = 0,
        /// <summary>
        /// 変化点を重視した取得。
        /// </summary>
        ChangingPoint = 1,
    }
}
