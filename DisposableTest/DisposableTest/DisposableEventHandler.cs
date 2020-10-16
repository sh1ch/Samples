using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisposableTest
{
    /// <summary>
    /// <see cref="DisposableEventHandler"/> クラスは、<see cref="EventHandler"/> を <see cref="IDisposable"/> に対応させ、イベントの登録解除をやりやすくしたクラスです。
    /// </summary>
    public class DisposableEventHandler : IDisposable
    {
        #region Properties

        private List<IDisposable> _Disposables = new List<IDisposable>();

        #endregion

        #region Initializes

        /// <summary>
        /// <see cref="DisposableEventHandler"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        public DisposableEventHandler() { }

        #endregion

        #region Events

        private event EventHandler Handler;

        #endregion

        #region Public Methods

        /// <summary>
        /// イベントの購読をすべて開放します。
        /// </summary>
        public void Dispose()
        {
            foreach (var disposable in _Disposables)
            {
                disposable?.Dispose();
            }

            _Disposables.Clear();
        }

        /// <summary>
        /// 指定した <see cref="EventHandler"/> を購読します。
        /// <para>
        /// 購読されたイベントハンドラーは、イベントを実行した際に実行されます。
        /// </para>
        /// </summary>
        /// <param name="handler"></param>
        public void SubScribe(EventHandler handler)
        {
            Handler += handler;

            // イベントの購読を終了する
            var disposable = Disposable.Create(() =>
            {
                // Console.WriteLine($"{handler} の購読を解除します。");

                Handler -= handler;
            });

            _Disposables.Add(disposable);
        }

        /// <summary>
        /// <see cref="EventHandler"/> のイベントを実行します。
        /// </summary>
        /// <param name="sender">実行オブジェクト。</param>
        /// <param name="args">イベント引数。</param>
        public void Raise(object sender, EventArgs args) => Handler?.Invoke(sender, args);

        /// <summary>
        /// <see cref="EventHandler"/> のイベントを実行します。
        /// </summary>
        /// <param name="sender">実行オブジェクト。</param>
        /// <param name="args">イベント引数。</param>
        public void Raise(object sender) => Handler?.Invoke(sender, EventArgs.Empty);

        #endregion
    }

    /// <summary>
    /// <see cref="DisposableEventHandler"/> クラスは、<see cref="EventHandler{TEventArgs}"/> を <see cref="IDisposable"/> に対応させ、イベントの登録解除をやりやすくしたクラスです。
    /// </summary>
    /// <typeparam name="TEventArgs">イベントによって生成されるイベント データの型。</typeparam>
    public class DisposableEventHandler<TEventArgs> : IDisposable
    {
        #region Properties

        private List<IDisposable> _Disposables = new List<IDisposable>();

        #endregion

        #region Initializes

        /// <summary>
        /// <see cref="DisposableEventHandler"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        public DisposableEventHandler() { }

        #endregion

        #region Events

        private event EventHandler<TEventArgs> Handler;

        #endregion

        #region Public Methods

        /// <summary>
        /// イベントの購読をすべて開放します。
        /// </summary>
        public void Dispose()
        {
            foreach (var disposable in _Disposables)
            {
                disposable?.Dispose();
            }

            _Disposables.Clear();
        }

        /// <summary>
        /// 指定した <see cref="EventHandler{TEventArgs}"/> を購読します。
        /// <para>
        /// 購読されたイベントハンドラーは、イベントを実行した際に実行されます。
        /// </para>
        /// </summary>
        /// <param name="handler">イベントによって生成されるイベント データの型。</param>
        public void SubScribe(EventHandler<TEventArgs> handler)
        {
            Handler += handler;

            // イベントの購読を終了する
            var disposable = Disposable.Create(() =>
            {
                // Console.WriteLine($"{handler} の購読を解除します。");
                
                Handler -= handler;
            });

            _Disposables.Add(disposable);
        }

        /// <summary>
        /// <see cref="EventHandler{TEventArgs}"/> のイベントを実行します。
        /// </summary>
        /// <param name="sender">実行オブジェクト。</param>
        /// <param name="args">イベント引数。</param>
        public void Raise(object sender, TEventArgs args) => Handler?.Invoke(sender, args);

        #endregion
    }
}
