using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DisposableTest
{
    /// <summary>
    /// <see cref="AnonymousDisposable"/> クラスは、<see cref="Action"/> をベースにした <see cref="IDisposable"/> オブジェクトを表現したクラスです。
    /// </summary>
    public class AnonymousDisposable : ICancelable
    {
        #region Fields

        private volatile Action _Dispose;

        #endregion

        #region Properties

        /// <summary>
        /// オブジェクトが破棄されているかどうかを示す値を取得します。
        /// </summary>
        public bool IsDisposed => _Dispose == null;

        #endregion

        #region Initializes

        /// <summary>
        /// <see cref="AnonymousDisposable"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="dispose"><see cref="IDisposable.Dispose"/> の発生時に実行されるアクション。</param>
        public AnonymousDisposable(Action dispose)
        {
            System.Diagnostics.Debug.Assert(dispose != null);

            _Dispose = dispose;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// 現在のインスタンスが破棄されていないなら、設定されている匿名アクションを実行します。
        /// </summary>
        public void Dispose()
        {
            // スレッドセーフで実行する
            Interlocked.Exchange(ref _Dispose, null)?.Invoke();
        }

        #endregion
    }

    /// <summary>
    /// <see cref="AnonymousDisposable"/> クラスは、<see cref="Action"/> をベースにした <see cref="IDisposable"/> オブジェクトを表現したクラスです。
    /// </summary>
    public class AnonymousDisposable<T> : ICancelable
    {
        #region Fields

        private T _State;
        private volatile Action<T> _Dispose;

        #endregion

        #region Properties

        /// <summary>
        /// オブジェクトが破棄されているかどうかを示す値を取得します。
        /// </summary>
        public bool IsDisposed => _Dispose == null;

        #endregion

        #region Initializes

        /// <summary>
        /// <see cref="AnonymousDisposable"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="state"></param>
        /// <param name="dispose"><see cref="IDisposable.Dispose"/> の発生時に実行されるアクション。</param>
        public AnonymousDisposable(T state, Action<T> dispose)
        {
            System.Diagnostics.Debug.Assert(dispose != null);

            _State = state;
            _Dispose = dispose;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// 現在のインスタンスが破棄されていないなら、設定されている匿名アクションを実行します。
        /// </summary>
        public void Dispose()
        {
            // スレッドセーフで実行する
            Interlocked.Exchange(ref _Dispose, null)?.Invoke(_State);
            _State = default;
        }

        #endregion
    }
}
