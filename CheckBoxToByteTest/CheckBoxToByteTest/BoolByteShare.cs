using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckBoxToByteTest;

/// <summary>
/// <see cref="BoolByteShare"/> クラスは、<see cref="bool[]"/> 型と <see cref="byte"/> 型のデータで値を共有するクラスです。
/// </summary>
public class BoolByteShare
{
    private bool[] _Bools = new bool[8];

    /// <summary>
    /// <see cref="byte"/> 型のデータを取得または設定します。
    /// </summary>
    public byte Byte
    {
        get
        {
            var bools = Bools;

            return ToByte(bools);
        }
        set
        {
            var bools = ToBools(value);

            Bools = bools;
        }
    }

    /// <summary>
    /// <see cref="Byte"/> と同じデータを表す <see cref="bool"/> 型の配列を取得または設定します。
    /// </summary>
    public bool[] Bools
    {
        get
        {
            var bools = new bool[8];

            bools[0] = Bool1;
            bools[1] = Bool2;
            bools[2] = Bool3;
            bools[3] = Bool4;
            bools[4] = Bool5;
            bools[5] = Bool6;
            bools[6] = Bool7;
            bools[7] = Bool8;

            return bools;
        }
        set
        {
            if (value == null)
            {
                throw new NullReferenceException();
            }

            if (value.Length != 8)
            {
                throw new ArgumentOutOfRangeException();
            }

            var bools = value;

            Bool1 = bools[0];
            Bool2 = bools[1];
            Bool3 = bools[2];
            Bool4 = bools[3];
            Bool5 = bools[4];
            Bool6 = bools[5];
            Bool7 = bools[6];
            Bool8 = bools[7];
        }
    }

    public bool Bool1
    {
        get => _Bools[0];
        set => _Bools[0] = value;
    }

    public bool Bool2
    {
        get => _Bools[1];
        set => _Bools[1] = value;
    }

    public bool Bool3
    {
        get => _Bools[2];
        set => _Bools[2] = value;
    }

    public bool Bool4
    {
        get => _Bools[3];
        set => _Bools[3] = value;
    }

    public bool Bool5
    {
        get => _Bools[4];
        set => _Bools[4] = value;
    }

    public bool Bool6
    {
        get => _Bools[5];
        set => _Bools[5] = value;
    }

    public bool Bool7
    {
        get => _Bools[6];
        set => _Bools[6] = value;
    }

    public bool Bool8
    {
        get => _Bools[7];
        set => _Bools[7] = value;
    }

    /// <summary>
    /// <see cref="BoolByteShare"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    public BoolByteShare()
    {
    }

    /// <summary>
    /// 指定した 16 進数のテキストの変換を試みて、変換に成功したとき <see cref="Byte"/> の値を更新します。
    /// </summary>
    /// <param name="hex">16 進数のテキスト。</param>
    /// <returns>変換に成功したとき <c>true</c>、それ以外のとき <c>false</c> を返却します。</returns>
    public bool TryParseUsingHex(string hex)
    {
        if (string.IsNullOrEmpty(hex))
        {
            return false;
        }

        if (hex.StartsWith("0x"))
        {
            hex = hex.Substring(2);
        }

        var canParse = byte.TryParse(hex, NumberStyles.HexNumber, NumberFormatInfo.CurrentInfo, out byte result);

        if (canParse)
        {
            Byte = result;
        }

        return canParse;
    }

    private bool[] ToBools(byte data)
    {
        var bools = new bool[8];

        for (int i = 0; i < 8; i++)
        {
            bools[i] = (data & (1 << i)) != 0;
        }

        Array.Reverse(bools);

        return bools;
    }

    private byte ToByte(bool[] sources)
    {
        byte result = 0x00;
        int index = 8 - sources.Length;

        if (sources.Length <= 0)
        {
            return result;
        }

        foreach (var source in sources)
        {
            if (source)
            {
                result |= (byte)(1 << (7 - index));
            }

            index++;
        }

        return result;
    }
}
