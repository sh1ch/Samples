using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DisposableTest
{
    /// <summary>
    /// <see cref="Disposable"/> クラスは、<see cref="IDisposable"/> オブジェクトを作成するための静的メソッドを提供するクラスです。
    /// </summary>
    public class Disposable
    {
        private sealed class EmptyDisposable : IDisposable
        {
            public static readonly EmptyDisposable Instance = new EmptyDisposable();

            private EmptyDisposable(){ }

            public void Dispose()
            {

            }
        }

        #region Properties

        /// <summary>
        /// 廃棄するときに何もしない <see cref="IDisposable"/> を取得します。
        /// </summary>
        public static IDisposable Empty => EmptyDisposable.Instance;

        #endregion

        #region Initializes

        /// <summary>
        /// <see cref="Disposable"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        public Disposable() { }

        #endregion

        #region Public Methods

        /// <summary>
        /// 廃棄するときに指定したアクションを呼び出す <see cref="IDisposable"/> オブジェクトを作成します。
        /// </summary>
        /// <param name="dispose"><see cref="IDisposable.Dispose"/> が最初に呼び出されたときに実行するアクション。</param>
        /// <returns>廃棄するときに与えられたアクションを実行する <see cref="IDisposable"/> オブジェクト。</returns>
        /// <exception cref="ArgumentNullException">指定した <paramref name="dispose"/> は <c>null</c> です。</exception>
        public static IDisposable Create(Action dispose)
        {
            if (dispose == null)
            {
                throw new ArgumentNullException(nameof(dispose));
            }

            return new AnonymousDisposable(dispose);
        }

        /// <summary>
        /// 廃棄するときに指定したアクションを呼び出す <see cref="IDisposable"/> オブジェクトを作成します。
        /// </summary>
        /// <param name="state"></param>
        /// <param name="dispose"><see cref="IDisposable.Dispose"/> が最初に呼び出されたときに実行するアクション。</param>
        /// <returns>廃棄するときに与えられたアクションを実行する <see cref="IDisposable"/> オブジェクト。</returns>
        /// <exception cref="ArgumentNullException">指定した <paramref name="dispose"/> は <c>null</c> です。</exception>
        public static IDisposable Create<T>(T state, Action<T> dispose)
        {
            if (dispose == null)
            {
                throw new ArgumentNullException(nameof(dispose));
            }

            return new AnonymousDisposable<T>(state, dispose);
        }

        #endregion
    }
}
