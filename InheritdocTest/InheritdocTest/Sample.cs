using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InheritdocTest
{
    /// <summary>
    /// <see cref="Sample"/> クラスは、<see cref="ISample"/> インターフェースで定義したコメントを
    /// 継承するかテストするためのクラスです。
    /// </summary>
    public class Sample : ISample
    {
        /// <inheritdoc />
        public string Property1 { get; set; }

        /// <inheritdoc />
        public event EventHandler Occurred;

        /// <inheritdoc />
        public void Method1()
        {
            Occurred?.Invoke(this, EventArgs.Empty);
        }
    }}
