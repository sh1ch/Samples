using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DeviceCheckSample
{
    /// <summary>
    /// <see cref="AudioDevice"/> クラスは、オーディオデバイスの情報を表現するクラスです。
    /// </summary>
    public class AudioDevice
    {
        #region DLL extern

        [DllImport("winmm.dll", SetLastError = true)]
        private static extern uint waveOutGetNumDevs();

        [DllImport("winmm.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern int waveOutGetDevCaps(int uDeviceID, ref WaveOutCaps pwoc, int cbwoc);

        private enum MMRESULT : int
        {
            MMSYSERR_NOERROR = 0,
            MMSYSERR_ERROR = 1,
            MMSYSERR_BADDEVICEID = 2,
            MMSYSERR_NOTENABLED = 3,
            MMSYSERR_ALLOCATED = 4,
            MMSYSERR_INVALHANDLE = 5,
            MMSYSERR_NODRIVER = 6,
            MMSYSERR_NOMEM = 7,
            MMSYSERR_NOTSUPPORTED = 8,
            MMSYSERR_BADERRNUM = 9,
            MMSYSERR_INVALFLAG = 10,
            MMSYSERR_INVALPARAM = 11,
            MMSYSERR_HANDLEBUSY = 12,
            MMSYSERR_INVALIDALIAS = 13,
            MMSYSERR_BADDB = 14,
            MMSYSERR_KEYNOTFOUND = 15,
            MMSYSERR_READERROR = 16,
            MMSYSERR_WRITEERROR = 17,
            MMSYSERR_DELETEERROR = 18,
            MMSYSERR_VALNOTFOUND = 19,
            MMSYSERR_NODRIVERCB = 20,
            WAVERR_BADFORMAT = 32,
            WAVERR_STILLPLAYING = 33,
            WAVERR_UNPREPARED = 34
        }

        [StructLayout(LayoutKind.Sequential, Pack = 2, CharSet = CharSet.Auto)]
        private struct WaveOutCaps
        {
            public short wMid;
            public short wPid;
            public uint vDriverVersion;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string szPname;
            public int dwFormats;
            public short wChannels;
            public short wReserved1;
            public int dwSupport;
        }

        #endregion

        #region Properties

        /// <summary>
        /// デバイスの名前を取得します。
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// デバイスの ID を表す値を取得します。
        /// </summary>
        public int Id { get; private set; }

        #endregion

        #region Initializes

        /// <summary>
        /// <see cref="AudioDevice"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        public AudioDevice()
        {

        }

        #endregion

        #region Public Methods

        /// <summary>
        /// 現在、コンピューターに存在しているオーディオデバイスを取得します。
        /// </summary>
        /// <returns>オーディオデバイスの列挙子。存在しないときは０件の列挙子を返却します。</returns>
        /// <exception cref="IndexOutOfRangeException">オーディオデバイスのインデックス例外。</exception>
        /// <exception cref="InvalidOperationException">再生デバイスの提供または、不明な例外。</exception>
        /// <exception cref="OutOfMemoryException">メモリーの割当、ロックの例外。</exception>
        public static IEnumerable<AudioDevice> Find()
        {
            var devices = new List<AudioDevice>();
            var waveOutCaps = new WaveOutCaps();
            var deviceCount = waveOutGetNumDevs();

            for (int id = 0; id < deviceCount; id++)
            {
                var codeValue = (MMRESULT)waveOutGetDevCaps(id, ref waveOutCaps, Marshal.SizeOf(typeof(WaveOutCaps)));
                var device = new AudioDevice();

                if (codeValue == MMRESULT.MMSYSERR_NOERROR)
                {
                    device.Id = id;
                    device.Name = waveOutCaps.szPname;
                    devices.Add(device);
                }
                else
                {
                    // エラー処理
                    switch (codeValue)
                    {
                        case MMRESULT.MMSYSERR_BADDEVICEID:
                            throw new IndexOutOfRangeException($"指定されたインデックス {id} は領域外の値です。");
                        case MMRESULT.MMSYSERR_NODRIVER:
                            throw new InvalidOperationException($"再生デバイスの提供がありませんでした。");
                        case MMRESULT.MMSYSERR_NOMEM:
                            throw new OutOfMemoryException("メモリーの割当またはロックができませんでした。");
                        default:
                            throw new InvalidOperationException($"インデックス {id} は、特定できないエラー ({codeValue}) が発生しました。");
                    }
                }
            }

            return devices;
        }

        #endregion
    }
}
