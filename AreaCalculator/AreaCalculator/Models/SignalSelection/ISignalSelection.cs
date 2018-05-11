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
    /// <see cref="ISignalSelection"/> クラスは、１秒間の信号の列挙子から必要なデータを選択するクラスです。
    /// </summary>
    public interface ISignalSelection
    {
        int[] Select(IEnumerable<string> signals, int count);
    }
}
