using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Prism.DialogParameterTest;

/// <summary>
/// <see cref="ParameterBase"/> クラスは、汎用パラメーターのベースです。
/// </summary>
public abstract class ParameterBase : IParameters, IEnumerable<KeyValuePair<string, object>>
{
    private readonly List<KeyValuePair<string, object>> _Entries = new List<KeyValuePair<string, object>>();

    /// <summary>
    /// <see cref="ParameterBase"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    protected ParameterBase()
    {
    }

    /// <summary>
    /// パラメーターのコレクションを検索し、コレクションにキーが含まれている場合はキーに対応する値を返却します。
    /// それ以外のときは <c>null</c> を返却します。
    /// </summary>
    /// <param name="key">コレクションの中でパラメーターを参照するキー。</param>
    /// <returns>キーが含まれるときは対応する値、それ以外のときは <c>null</c> を返却します。</returns>
    public object? this[string key]
    {
        get
        {
            foreach (var entry in _Entries)
            {
                if (string.Compare(entry.Key, key, StringComparison.Ordinal) == 0)
                {
                    return entry.Value;
                }
            }

            return null;
        }
    }

    public int Count => _Entries.Count;

    /// <summary>
    /// コレクションに含まれるすべてキーの <see cref="IEnumerable{string}"/> を取得します。
    /// </summary>
    public IEnumerable<string> Keys => _Entries.Select(p => p.Key);

    public void Add(string key, object value) => _Entries.Add(new (key, value));

    public bool ContainsKey(string key) => Keys.Contains(key);

    public IEnumerator<KeyValuePair<string, object>> GetEnumerator() => _Entries.GetEnumerator();

    public T? GetValue<T>(string key) => GetValue<T?>(_Entries, key);

    public IEnumerable<T?> GetValues<T>(string key) => GetValues<T>(_Entries, key);

    public bool TryGetValue<T>(string key, out T? value) => TryGetValue(_Entries, key, out value);

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();


    private T? GetValue<T>(IEnumerable<KeyValuePair<string, object>> parameters, string key)
        => (T?)GetValue(parameters, key, typeof(T));

    private object? GetValue(IEnumerable<KeyValuePair<string, object>> parameters, string key, Type type)
    {
        foreach (var parameter in parameters)
        {
            if (string.Compare(parameter.Key, key, StringComparison.Ordinal) == 0)
            {
                if (TryGetValue(parameter, type, out var value))
                {
                    return value;
                }

                // キーが一致しても型が一致しない（型の変換に失敗した）ときは例外
                throw new InvalidCastException($"Unable to convert the value of Type '{parameter.Value.GetType().FullName}' to '{type.FullName}' for the key '{key}' ");
            }
        }

        return GetDefault(type);
    }

    private IEnumerable<T?> GetValues<T>(IEnumerable<KeyValuePair<string, object>> parameters, string key)
    {
        var values = new List<T?>();
        var type = typeof(T);

        foreach (var parameter in parameters)
        {
            if (string.Compare(parameter.Key, key, StringComparison.Ordinal) == 0)
            {
                TryGetValue(parameter, type, out var value);
                values.Add((T?)value);
            }
        }

        return values.AsEnumerable();
    }

    private bool TryGetValue<T>(IEnumerable<KeyValuePair<string, object>> parameters, string key, out T? value)
    {
        var isSuccessed = false;
        var type = typeof(T);

        foreach (var parameter in parameters)
        {
            if (string.Compare(parameter.Key, key, StringComparison.Ordinal) == 0)
            {
                isSuccessed = TryGetValue(parameter, typeof(T), out object? valueAsObject);

                value = (T?)valueAsObject;
                return isSuccessed;
            }
        }

        value = (T?)GetDefault(type);

        return isSuccessed;
    }

    private bool TryGetValue(KeyValuePair<string, object> parameter, Type type, out object? value)
    {
        var isSuccessd = false;

        value = GetDefault(type);

        if (parameter.Value == null)
        {
            isSuccessd = true;
        }
        else if (parameter.Value.GetType() == type)
        {
            // 基本的な設定
            isSuccessd = true;
            value = parameter.Value;
        }

        // 例外的な設定
        if (!isSuccessd && parameter.Value != null)
        {
            if (type.IsAssignableFrom(parameter.Value.GetType()))
            {
                isSuccessd = true;
                value = parameter.Value;
            }
            else if (type.IsEnum)
            {
                var valueAsString = parameter.Value.ToString() ?? "";

                if (Enum.IsDefined(type, valueAsString))
                {
                    isSuccessd = true;
                    value = Enum.Parse(type, valueAsString);
                }
                else if (int.TryParse(valueAsString, out var valueAsInt))
                {
                    isSuccessd = true;
                    value = Enum.ToObject(type, valueAsInt);
                }
            }

            if (!isSuccessd && type.GetInterface(typeof(System.IConvertible).ToString()) != null)
            {
                isSuccessd = true;
                value = Convert.ChangeType(parameter.Value, type);
            }
        }

        return isSuccessd;
    }

    private object? GetDefault(Type type)
    {
        if (type.IsValueType)
        {
            return Activator.CreateInstance(type);
        }

        return null;
    }
}
