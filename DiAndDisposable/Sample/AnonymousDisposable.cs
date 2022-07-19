using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Samples;

/// <summary>
/// <see cref="AnonymousDisposable"/> クラスは、<see cref="CompositeDisposable"/> クラスの拡張機能です。
/// </summary>
internal class AnonymousDisposable : IDisposable
{
    private readonly Action _ReleaseAction;
    private bool _Disposed;

    /// <summary>
    /// <see cref="AnonymousDisposable"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="releaseAction">リソースを解放するアクション。</param>
    public AnonymousDisposable(Action releaseAction)
    {
        _ReleaseAction = releaseAction;
        _Disposed = false;
    }

    /// <summary>
    /// ２つのインスタンスが等しいかどうかを判断します。
    /// </summary>
    /// <param name="action"></param>
    /// <returns>指定したアクションとアクションが等しいときは <c>true</c>、それ以外のときは <c>false</c>。</returns>
    public bool Equals(Action action) => _ReleaseAction == action;

    /// <summary>
    /// アンマネージ リソースの解放、または、リセットに関連付けられているアプリケーション定義のタスクを実行します。
    /// </summary>
    public void Dispose()
    {
        if (_Disposed) return;

        _ReleaseAction();
        _Disposed = true;

        GC.SuppressFinalize(this);
    }
}
