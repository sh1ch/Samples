using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;

namespace SaveWindowPlacementStateSample
{
    /// <summary>
    /// <see cref="WindowPlacementStateSetting"/> クラスは、画面上の配置情報の保存と読込をサポートします。
    /// </summary>
    public class WindowPlacementStateSetting
    {
        /// <summary>
        /// 画面上の位置情報構造体を取得または設定します。
        /// </summary>
        public WINDOWPLACEMENT? Placement { get; set; }

        /// <summary>
        /// 指定した画面上のウィンドウに <see cref="Placement"/> 情報を適用します。
        /// </summary>
        /// <param name="window">設定するウィンドウ。</param>
        public void Attach(Window window)
        {
            if (window == null) throw new ArgumentNullException(nameof(window));
            if (Placement == null) throw new NullReferenceException(nameof(Placement));

            var hwnd = new WindowInteropHelper(window).Handle;
            var placement = Placement.Value;

            placement.length = Marshal.SizeOf(typeof(WINDOWPLACEMENT));
            placement.flags = 0;
            placement.showCmd = (placement.showCmd == SW.SHOWMINIMIZED) ? SW.SHOWNORMAL : placement.showCmd;

            NativeMethod.SetWindowPlacement(hwnd, ref placement);
        }

        /// <summary>
        /// 指定した画面上のウィンドウの情報を読込します。 
        /// </summary>
        /// <param name="window">保存するウィンドウ。</param>
        public void Read(Window window)
        {
            if (window == null) throw new ArgumentNullException(nameof(window));

            WINDOWPLACEMENT placement;
            var hwnd = new WindowInteropHelper(window).Handle;
            NativeMethod.GetWindowPlacement(hwnd, out placement);

            Placement = placement;
        }
    }
}
