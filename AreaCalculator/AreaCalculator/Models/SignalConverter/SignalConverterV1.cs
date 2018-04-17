using HeritageFramework;
using HeritageFramework.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AreaCalculator.Models.SignalConverter
{
    /// <summary>
    /// <see cref="SignalConverterV1"/> クラスは、信号強度 (0-100,000) を表示値 (0-1250mV) に変換するクラスです。
    /// </summary>
    public class SignalConverterV1 : ISignalConverter
    {
        #region Public Methods

        public double Parse(double signal)
        {
            if (signal > 100000) signal = 100000;
            else if (signal < 0) signal = 0;

            return signal * 0.0125;
        }

        #endregion
    }
}
