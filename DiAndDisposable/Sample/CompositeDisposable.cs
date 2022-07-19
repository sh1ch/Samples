using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Samples;

/// <summary>
/// <see cref="IDisposable"/> のオブジェクトをまとめて解放する機能を提供します。
/// </summary>
public class CompositeDisposable : IDisposable, ICollection<IDisposable>
{
    private readonly object _Lock = new object();
    private readonly List<IDisposable> _Disposables;
    private bool _Disposed;

    /// <summary>
    /// コレクションに格納されている要素の数を取得します。
    /// </summary>
    public int Count => _Disposables.Count;

    /// <summary>
    /// コレクションが読み取り専用かどうかを示す値を取得します。（常に <c>false</c> を返却）
    /// </summary>
    public bool IsReadOnly => false;

    /// <summary>
    /// <see cref="CompositeDisposable"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    public CompositeDisposable()
    {
        _Disposed = false;
        _Disposables = new List<IDisposable>();
    }

    public CompositeDisposable(IEnumerable<IDisposable> disposables)
    {
        if (disposables == null)
        {
            throw new ArgumentNullException(nameof(disposables));
        }

        _Disposables = new List<IDisposable>(disposables);
    }

    public IEnumerator<IDisposable> GetEnumerator()
    {
        ThrowExceptionIfDisposed();

        lock (_Lock)
        {
            return _Disposables.GetEnumerator();
        }
    }

    IEnumerator<IDisposable> IEnumerable<IDisposable>.GetEnumerator()
    {
        ThrowExceptionIfDisposed();

        lock (_Lock)
        {
            yield return _Disposables.GetEnumerator();
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        ThrowExceptionIfDisposed();

        lock (_Lock)
        {
            yield return _Disposables.GetEnumerator();
        }
    }

    /// <summary>
    /// リソースを解放するコレクションに要素を追加します。
    /// </summary>
    /// <param name="disposable">追加するオブジェクト。</param>
    /// <exception cref="ArgumentNullException">引数の値が <c>null</c> のときに発生する例外。</exception>
    public void Add(IDisposable disposable)
    {
        if (disposable == null)
        {
            throw new ArgumentNullException(nameof(disposable));
        }

        ThrowExceptionIfDisposed();

        lock (_Lock)
        {
            _Disposables.Add(disposable);
        }
    }

    /// <summary>
    /// リソースを解放するコレクションに要素を追加します。
    /// </summary>
    /// <param name="action">追加するアクション。</param>
    /// <exception cref="ArgumentNullException">引数の値が <c>null</c> のときに発生する例外。</exception>
    public void Add(Action action)
    {
        if (action == null)
        {
            throw new ArgumentNullException(nameof(action));
        }

        ThrowExceptionIfDisposed();

        var disposable = new AnonymousDisposable(action);

        lock (_Lock)
        {
            _Disposables.Add(disposable);
        }
    }

    /// <summary>
    /// すべての要素を削除します。
    /// </summary>
    public void Clear()
    {
        ThrowExceptionIfDisposed();

        lock (_Lock)
        {
            _Disposables.Clear();
        }
    }

    /// <summary>
    /// ある要素が、コレクション内に存在するかどうかを判断します。
    /// </summary>
    /// <param name="disposable">検索するオブジェクト。</param>
    /// <returns>存在するとき <c>true</c>、それ以外のとき <c>false</c>。</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public bool Contains(IDisposable disposable)
    {
        if (disposable == null)
        {
            throw new ArgumentNullException(nameof(disposable));
        }

        ThrowExceptionIfDisposed();

        lock (_Lock)
        {
            return _Disposables.Contains(disposable);
        }
    }

    /// <summary>
    /// ある要素が、コレクション内に存在するかどうかを判断します。
    /// </summary>
    /// <param name="action">検索するアクション。</param>
    /// <returns>存在するときは <c>true</c>、それ以外のときは <c>false</c>。</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public bool Contains(Action action)
    {
        if (action == null)
        {
            throw new ArgumentNullException(nameof(action));
        }

        ThrowExceptionIfDisposed();

        lock (_Lock)
        {
            var anonymous = _Disposables
                .Where(p => p is AnonymousDisposable)
                .Cast<AnonymousDisposable>();

            return anonymous.Any(p => p.Equals(action));
        }
    }

    /// <summary>
    /// コレクションの全体、または、その一部を配列にコピーします。
    /// </summary>
    /// <param name="copyToArray">コピー先の配列。</param>
    /// <param name="index">コピーの開始位置とする <paramref name="disposables"/> のインデックス。（０から始まる）</param>
    public void CopyTo(IDisposable[] copyToArray, int index)
    {
        ThrowExceptionIfDisposed();

        lock (_Lock)
        {
            _Disposables.CopyTo(copyToArray, index);
        }
    }

    /// <summary>
    /// 特定のオブジェクトがコレクション内にあるときに、最初に出現したものを削除します。
    /// </summary>
    /// <param name="disposable">削除するオブジェクト。</param>
    /// <returns>正常に削除されたときは <c>true</c>、それ以外のときは <c>false</c>。</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public bool Remove(IDisposable disposable)
    {
        if (disposable == null)
        {
            throw new ArgumentNullException(nameof(disposable));
        }

        ThrowExceptionIfDisposed();

        lock (_Lock)
        {
            return _Disposables.Remove(disposable);
        }
    }

    /// <summary>
    /// 特定のアクションがコレクション内にあるときに、最初に出現したものを削除します。
    /// </summary>
    /// <param name="disposable">削除するアクション。</param>
    /// <returns>正常に削除されたときは <c>true</c>、それ以外のときは <c>false</c>。</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public bool Remove(Action action)
    {
        if (action == null)
        {
            throw new ArgumentNullException(nameof(action));
        }

        ThrowExceptionIfDisposed();

        lock (_Lock)
        {
            var anonymous = _Disposables
                .Where(p => p is AnonymousDisposable)
                .Cast<AnonymousDisposable>();

            var removingAction = anonymous.SingleOrDefault(p => p.Equals(action));

            if (removingAction == null)
            {
                return false;
            }

            return _Disposables.Remove(removingAction);
        }
    }

    /// <summary>
    /// アンマネージ リソースの解放、または、リセットに関連付けられているアプリケーション定義のタスクを実行します。
    /// </summary>
    public void Dispose()
    {
        if (_Disposed) return;

        lock (_Lock)
        {
            _Disposables.ForEach(p => p.Dispose());
        }

        _Disposed = true;
        GC.SuppressFinalize(this);
    }

    protected void ThrowExceptionIfDisposed()
    {
        if (_Disposed)
        {
            throw new ObjectDisposedException(nameof(CompositeDisposable));
        }
    }
}
