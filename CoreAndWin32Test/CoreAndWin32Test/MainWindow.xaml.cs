using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Windows.Sdk;



namespace CoreAndWin32Test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // var hWnd = PInvoke.FindWindow("UnityWndClass", "umamusume");
            var hWnd = PInvoke.FindWindow("Notepad", "*無題 - メモ帳");

            var bitmap = GetCaptureBitmap(hWnd);

            bitmap?.Save("save.png", ImageFormat.Png);
        }

        private unsafe Bitmap GetCaptureBitmap(HWND hWnd)
        {
            Bitmap bitmap = null;
            HWND zero = new HWND(0);
            HDC desktopDC = new HDC(IntPtr.Zero);
            HDC memoryDC = new HDC(IntPtr.Zero);

            try
            {
                var result = PInvoke.GetClientRect(hWnd, out RECT rect);

                if (!result) throw new NullReferenceException($"指定したハンドルのウィンドウ({hWnd})を発見することができませんでした。");

                POINT point = new POINT();
                var mapResult = PInvoke.MapWindowPoints(hWnd, zero, &point, 2);

                if (mapResult == 0) throw new NullReferenceException($"指定したハンドルのウィンドウ({hWnd})の座標空間の変換に失敗しました。");

                rect.left = point.x;
                rect.top = point.y;
                rect.right = rect.right + point.x;
                rect.bottom = rect.bottom + point.y;

                var tempRect = rect;

                desktopDC = PInvoke.GetWindowDC(zero); // デスクトップの HDC を取得

                var header = new BITMAPINFOHEADER()
                {
                    biSize = (uint)Marshal.SizeOf(typeof(BITMAPINFOHEADER)),
                    biWidth = tempRect.right - rect.left,
                    biHeight = tempRect.bottom - rect.top,
                    biPlanes = 1,
                    biCompression = 0, // BitmapCompressionMode.BI_RGB = 0
                    biBitCount = 24,
                };

                var info = new BITMAPINFO
                {
                    bmiHeader = header,
                };

                void** bits = null;

                HBITMAP hBitmap = PInvoke.CreateDIBSection(desktopDC, &info, DIB_USAGE.DIB_RGB_COLORS, bits, new HANDLE(IntPtr.Zero), 0);

                memoryDC = PInvoke.CreateCompatibleDC(desktopDC);

                var phBitMap = PInvoke.SelectObject(memoryDC, new HGDIOBJ(hBitmap));

                PInvoke.BitBlt(memoryDC, 0, 0, header.biWidth, header.biHeight, desktopDC, rect.left, rect.top, ROP_CODE.SRCCOPY);
                PInvoke.SelectObject(memoryDC, phBitMap);

                bitmap = Bitmap.FromHbitmap(hBitmap, IntPtr.Zero);
            }
            finally
            {
                if (desktopDC.Value != IntPtr.Zero)
                {
                    PInvoke.ReleaseDC(zero, desktopDC);
                }

                if (memoryDC.Value != IntPtr.Zero)
                {
                    PInvoke.ReleaseDC(hWnd, memoryDC);
                }
            }

            return bitmap;
        }

    }
}
