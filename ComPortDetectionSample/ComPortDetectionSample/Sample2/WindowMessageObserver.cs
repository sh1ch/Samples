using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ComPortDetectionSample
{
    /// <summary>
    /// <see cref="WindowMessageObserver"/> クラスは、WndProc のメッセージの観察をするクラスです。
    /// </summary>
    public class WindowMessageObserver : IDisposable
    {
        #region Fields

        private object _LockObject = new object();
        private Thread _ReceiverThread = null;

        #endregion

        #region Properties

        /// <summary>
        /// WndProc のメッセージの転送をサポートする <see cref="WindowMessageReceiver"/> オブジェクトを取得します。
        /// </summary>
        public WindowMessageReceiver Receiver { get; private set; }
        
        /// <summary>
        /// <see cref="Receiver"/> が実行中かどうかを示す値を取得します。
        /// </summary>
        public bool IsRunning { get; private set; }

        /// <summary>
        /// WndProc のメッセージの転送をサポートする <see cref="WindowMessageReceiver"/> オブジェクトのハンドルを取得します。
        /// </summary>
        protected IntPtr Handle
        {
            get
            {
                if (Receiver == null) throw new NullReferenceException($"{Receiver} オブジェクトは null です。");
                if (IsRunning == false) throw new InvalidOperationException($"{Receiver} オブジェクトは、実行されていません。");

                return Receiver.Handle;
            }
        }

        #endregion

        #region Initializes

        /// <summary>
        /// <see cref="WindowMessageObserver"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        public WindowMessageObserver()
        {
            var name = Thread.CurrentThread.Name;

            CreateReceiver($"{name}->win32 msg subthread", true);
        }

        #endregion

        #region Events

        #endregion

        #region Public Methods

        /// <summary>
        /// 使用したすべてのリソースを開放します。
        /// </summary>
        public void Dispose()
        {
            if (IsRunning)
            {
                Receiver.Dispose();

                _ReceiverThread.Abort();
                _ReceiverThread = null;

                IsRunning = false;
            }
        }

        /// <summary>
        /// <see cref="Receiver"/> にメッセージを送信します。
        /// </summary>
        /// <param name="message">送信するメッセージ。</param>
        [Obsolete]
        public void Post(int message)
        {
            WIN32API.PostMessage(Handle, message, System.IntPtr.Zero, 0);
        }

        #endregion

        #region Private Methods

        private void CreateReceiver(string threadName, bool isBackground)
        {
            lock (_LockObject)
            {
                if (IsRunning) throw new InvalidOperationException($"{Receiver} オブジェクトは、すでに実行されています。");

                using (var e = new ManualResetEvent(false))
                {
                    _ReceiverThread = new Thread(() =>
                    {
                        Receiver = new WindowMessageReceiver();
                        e.Set();
                        System.Windows.Forms.Application.Run();
                    });

                    _ReceiverThread.Name = threadName;
                    _ReceiverThread.IsBackground = isBackground;
                    _ReceiverThread.Start();

                    e.WaitOne();

                    IsRunning = true;
                }

            }
        }

        [Obsolete]
        private static int SendMessage(System.IntPtr hWnd, int Msg, System.IntPtr wParam, int lParam)
        {
            return WIN32API.SendMessage(hWnd, Msg, wParam, lParam);
        }

        [Obsolete]
        private static int SendMessage(System.IntPtr hWnd, int Msg, System.IntPtr wParam, ref COPYDATASTRUCT lParam)
        {
            return WIN32API.SendMessage(hWnd, Msg, wParam, ref lParam);
        }

        [Obsolete]
        private static int PostMessage(System.IntPtr hWnd, int Msg, System.IntPtr wParam, int lParam)
        {
            return WIN32API.PostMessage(hWnd, Msg, wParam, lParam);
        }

        #endregion

        #region Private Objects

        private class WIN32API
        {
            [System.Runtime.InteropServices.DllImport("User32.dll", EntryPoint = "SendMessageA", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
            public static extern int SendMessage(System.IntPtr hWnd, int Msg, System.IntPtr wParam, int lParam);

            [System.Runtime.InteropServices.DllImport("user32.dll", EntryPoint = "SendMessageA", CharSet = System.Runtime.InteropServices.CharSet.Unicode)]
            public static extern int SendMessage(System.IntPtr hWnd, int Msg, System.IntPtr wParam, ref COPYDATASTRUCT lParam);

            [System.Runtime.InteropServices.DllImport("User32.dll", EntryPoint = "PostMessageA", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
            public static extern int PostMessage(System.IntPtr hWnd, int Msg, System.IntPtr wParam, int lParam);
        }

        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential, CharSet = System.Runtime.InteropServices.CharSet.Unicode)]
        private struct COPYDATASTRUCT
        {
            public System.IntPtr dwData;
            public int cbData;
            [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPWStr)]
            public string lpData;
        }

        [Obsolete]
        private enum Messages
        {
            WM_COPYDATA = 0x004A,
            WM_USER = 0x0400,
            WM_APP = 0x8000
        }

        #endregion
    }
}
