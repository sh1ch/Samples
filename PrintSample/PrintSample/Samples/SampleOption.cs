using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintSample
{
    /// <summary>
    /// <see cref="SampleOption"/> クラスは、サンプルのオプションを表すクラスです。
    /// </summary>
    public class SampleOption
    {
        #region Properties

        private bool _Option1;

        public bool Option1
        {
            get { return _Option1; }
            set
            {
                _Option1 = value;
                Changed?.Invoke(this, EventArgs.Empty);
            }
        }

        private bool _Option2;

        public bool Option2
        {
            get { return _Option2; }
            set
            {
                _Option2 = value;
                Changed?.Invoke(this, EventArgs.Empty);
            }
        }

        private int _Pages = 1;

        public int Pages
        {
            get { return _Pages; }
            set
            {
                _Pages = value;
                Changed?.Invoke(this, EventArgs.Empty);
            }
        }

        #endregion

        #region Initializes

        /// <summary>
        /// <see cref="SampleOption"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        public SampleOption() { }

        #endregion

        #region Events

        public event EventHandler Changed;

        #endregion
    }
}
