using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Navigation;

namespace FixedDecimalPointTextBoxSample
{
    /// <summary>
    /// <see cref="SimpleCommand"/> クラスは、シンプルな <see cref="ICommand"/> に対応するクラスです。
    /// </summary>
    public class SimpleCommand : ICommand
    {
        #region Fields

        private Action _Execute;
        private Func<bool> _CanExecute;

        #endregion

        #region Initializes

        /// <summary>
        /// <see cref="SimpleCommand"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        public SimpleCommand() { }

        /// <summary>
        /// <see cref="SimpleCommand"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="execute">コマンドが呼び出されたときに実行するメソッド。</param>
        public SimpleCommand(Action execute) : this(execute, null)
        {

        }

        /// <summary>
        /// <see cref="SimpleCommand"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="execute">コマンドが呼び出されたときに実行するメソッド。</param>
        /// <param name="canExecute">実行メソッドを実施してよいかどうかをしめすメソッド。</param>
        public SimpleCommand(Action execute, Func<bool> canExecute)
        {
            if (execute == null) throw new ArgumentException();

            _Execute = execute;
            _CanExecute = canExecute;
        }

        #endregion

        #region Events

        public event EventHandler CanExecuteChanged;

        #endregion

        #region Public Methods

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter) => _Execute();

        #endregion
    }
}
