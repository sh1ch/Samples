using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prism.DialogParameterTest;

/// <summary>
/// <see cref="IParameters"/> インターフェースは、データの引数（パラメーター）を定義します。
/// </summary>
/// <remarks>
/// <see cref="IParameters"/> インターフェースの実装クラスは、引数（パラメーター）となるデータの取得/設定をサポートします。
/// </remarks>
public interface IParameters
{
    /// <summary>
    /// コレクションにキーと値のペアを追加します。
    /// </summary>
    /// <param name="key">コレクションの中でパラメーターの値を参照するためのキー。</param>
    /// <param name="value">格納するパラメーターの値。</param>
    void Add(string key, object value);

    /// <summary>
    /// コレクションに指定したキーの名前が存在しているかどうかを示す値を取得します。
    /// </summary>
    /// <param name="key">確認をするキーの名前。</param>
    /// <returns>キーが存在するとき <c>true</c>、それ以外のときは <c>false</c> を返却します。</returns>
    bool ContainsKey(string key);

    /// <summary>
    /// コレクションに含まれるパラメーターの数を取得します。
    /// </summary>
    int Count { get; }

    /// <summary>
    /// キーによって参照されるパラメーターの値を取得します。
    /// </summary>
    /// <typeparam name="T">パラメーターの型。</typeparam>
    /// <param name="key">パラメーターの値を参照するキー。</param>
    /// <returns><typeparamref name="T"/> 型の（キーに一致する）パラメーターの値。</returns>
    T? GetValue<T>(string key);

    /// <summary>
    /// キーによって参照されるすべてのパラメーターの値を取得します。
    /// </summary>
    /// <typeparam name="T">パラメーターの型。</typeparam>
    /// <param name="key">パラメーターの値を参照するキー。</param>
    /// <returns><typeparamref name="T"/> 型の（キーに一致する）すべてのパラメーターの値。</returns>
    IEnumerable<T?> GetValues<T>(string key);

    /// <summary>
    /// コレクションに参照するキーが存在する場合、パラメーターの値を取得します。
    /// </summary>
    /// <typeparam name="T">パラメーターの型。</typeparam>
    /// <param name="key">パラメーターの値を参照するキー。</param>
    /// <param name="value">
    /// このメソッドから制御がもどるとき、参照するキーが見つかった場合、指定した（キーに一致する）値が格納されます。
    /// それ以外のときは、<typeparamref name="T"/> 型に対応する既定の値。このパラメーターは初期化されずに渡されます。
    /// </param>
    /// <returns>キーに一致するパラメーターが存在するときは <c>true</c>、それ以外のときは <c>false</c> を返却します。</returns>
    bool TryGetValue<T>(string key, out T? value);
}
