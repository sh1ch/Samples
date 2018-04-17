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
    /// <see cref="ISignalConverter"/> インターフェースは、信号値を表示値に変換するインターフェースです。
    /// </summary>
    public interface ISignalConverter
    {
        double Parse(double signal);
    }
}
