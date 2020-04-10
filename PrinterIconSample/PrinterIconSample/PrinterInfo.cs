using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PrinterIconSample
{
    /// <summary>
    /// <see cref="PrinterInfo"/> クラスは、OS にインストールされているプリンターを調べるためのクラスです。
    /// </summary>
    public class PrinterInfo
    {
        #region Fields (static and const)

        static Guid IID_IShellFolder = new Guid("000214E6-0000-0000-C000-000000000046");

        const int CSIDL_PRINTERS = 0x0004;
        const uint SHGFI_ICON = 0x100;
        const uint SHGFI_LARGEICON = 0x0;
        const uint SHGFI_SMALLICON = 0x1;
        const uint SHGFI_PIDL = 0x8;
        const uint SHGFI_DISPLAYNAME = 0x000000200;

        #endregion

        #region DllImport

        [DllImport("shell32.dll")]
        static extern int SHGetFolderLocation(IntPtr hwndOwner, int nFolder, IntPtr hToken, uint dwReserved, out IntPtr ppidl);

        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        static extern Int32 SHBindToObject(IntPtr shell, IntPtr pidl, IntPtr pbc, ref Guid riid, out IntPtr ppvOut);

        [DllImport("shlwapi.dll", CharSet = CharSet.Auto)]
        static extern void StrRetToBuf([In, Out] ref STRRET pstr, [In] IntPtr pidl, [Out, MarshalAs(UnmanagedType.LPTStr)] StringBuilder pszBuf, uint cchBuf);

        [DllImport("shlwapi.dll")]
        static extern void StrRetToBSTR([In, Out] ref STRRET pstr, [In] IntPtr pidl, [Out, MarshalAs(UnmanagedType.BStr)] out string pbstr);

        [DllImport("shell32.dll")]
        static extern IntPtr SHGetFileInfo(IntPtr pIDL, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbSizeFileInfo, uint uFlags);

        [DllImport("user32.dll")]
        static extern int DestroyIcon(IntPtr hIcon);

        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr ILCombine(IntPtr pIDLParent, IntPtr pIDLChild);

        #endregion

        #region Import structs, interface and enum

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private struct SHFILEINFO
        {
            public IntPtr hIcon;
            public int iIcon;
            public uint dwAttributes;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;
        }

        [ComImport]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [Guid("000214F2-0000-0000-C000-000000000046")]
        private interface IEnumIDList
        {
            [PreserveSig]
            uint Next(uint celt, out IntPtr rgelt, out uint pceltFetched);
            [PreserveSig]
            uint Skip(uint celt);
            [PreserveSig]
            uint Reset();
            [PreserveSig]
            uint Clone(out IntPtr ppenum);
        }

        [ComImport]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [Guid("000214E6-0000-0000-C000-000000000046")]
        private interface IShellFolder
        {
            void ParseDisplayName(IntPtr hwnd, IntPtr pbc, String pszDisplayName, UInt32 pchEaten, out IntPtr ppidl, UInt32 pdwAttributes);
            void EnumObjects(IntPtr hwnd, ESHCONTF grfFlags, out IntPtr ppenumIDList);
            void BindToObject(IntPtr pidl, IntPtr pbc, [In]ref Guid riid, out IntPtr ppv);
            void BindToStorage(IntPtr pidl, IntPtr pbc, [In]ref Guid riid, out IntPtr ppv);
            Int32 CompareIDs(Int32 lParam, IntPtr pidl1, IntPtr pidl2);
            void CreateViewObject(IntPtr hwndOwner, [In] ref Guid riid, out IntPtr ppv);
            void GetAttributesOf(UInt32 cidl, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)]IntPtr[] apidl, ref ESFGAO rgfInOut);
            void GetUIObjectOf(IntPtr hwndOwner, UInt32 cidl, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)]IntPtr[] apidl, [In] ref Guid riid, UInt32 rgfReserved, out IntPtr ppv);
            void GetDisplayNameOf(IntPtr pidl, ESHGDN uFlags, out STRRET pName);
            void SetNameOf(IntPtr hwnd, IntPtr pidl, String pszName, ESHCONTF uFlags, out IntPtr ppidlOut);

        }

        private enum ESFGAO : uint
        {
            SFGAO_CANCOPY = 0x00000001,
            SFGAO_CANMOVE = 0x00000002,
            SFGAO_CANLINK = 0x00000004,
            SFGAO_LINK = 0x00010000,
            SFGAO_SHARE = 0x00020000,
            SFGAO_READONLY = 0x00040000,
            SFGAO_HIDDEN = 0x00080000,
            SFGAO_FOLDER = 0x20000000,
            SFGAO_FILESYSTEM = 0x40000000,
            SFGAO_HASSUBFOLDER = 0x80000000,
        }

        private enum ESHCONTF
        {
            SHCONTF_FOLDERS = 0x0020,
            SHCONTF_NONFOLDERS = 0x0040,
            SHCONTF_INCLUDEHIDDEN = 0x0080,
            SHCONTF_INIT_ON_FIRST_NEXT = 0x0100,
            SHCONTF_NETPRINTERSRCH = 0x0200,
            SHCONTF_SHAREABLE = 0x0400,
            SHCONTF_STORAGE = 0x0800
        }

        private enum ESHGDN
        {
            SHGDN_NORMAL = 0x0000,
            SHGDN_INFOLDER = 0x0001,
            SHGDN_FOREDITING = 0x1000,
            SHGDN_FORADDRESSBAR = 0x4000,
            SHGDN_FORPARSING = 0x8000,
        }

        private enum STRRETType : int
        {
            WideString = 0x0000,
            Offset = 0x0001,
            CString = 0x0002
        }

        [StructLayout(LayoutKind.Explicit, Size = 264)]
        private struct STRRET
        {
            [FieldOffset(0)]
            public uint uType;

            [FieldOffset(4)]
            public IntPtr pOleStr;

            [FieldOffset(4)]
            public uint uOffset;

            [FieldOffset(4)]
            public IntPtr cStr;
        }

        #endregion

        #region Initializes

        /// <summary>
        /// <see cref="PrinterInfo"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        public PrinterInfo() { }

        #endregion

        #region Public Methods

        /// <summary>
        /// OS にインストールされているプリンター名とアイコンを取得します。
        /// </summary>
        /// <returns>プリンター名とアイコンのペア。</returns>
        public static IEnumerable<Tuple<string, BitmapSource>> GetInstalledPrinters()
        {
            List<Tuple<string, BitmapSource>> icons = new List<Tuple<string, BitmapSource>>();

            IntPtr hwnd = IntPtr.Zero;
            IntPtr pPrinters;
            IntPtr pPointer1;
            IntPtr pPointer2;
            int s_ok;

            s_ok = SHGetFolderLocation(hwnd, CSIDL_PRINTERS, IntPtr.Zero, 0, out pPrinters);

            if (s_ok != 0) return null; // 取得に失敗

            s_ok = SHBindToObject(IntPtr.Zero, pPrinters, IntPtr.Zero, ref IID_IShellFolder, out pPointer1);

            if (s_ok != 0) return null; // 取得に失敗

            var shell = (IShellFolder)Marshal.GetObjectForIUnknown(pPointer1);

            shell.EnumObjects(hwnd, ESHCONTF.SHCONTF_NONFOLDERS, out pPointer2);

            var enumIDList = (IEnumIDList)Marshal.GetObjectForIUnknown(pPointer2);
            IntPtr idPointer;

            while (enumIDList.Next(1, out idPointer, out _) == 0)
            {
                STRRET strret;
                shell.GetDisplayNameOf(idPointer, ESHGDN.SHGDN_NORMAL, out strret);

                // プリンター名の取得
                string printerName;
                StrRetToBSTR(ref strret, idPointer, out printerName);

                var pPrinter = ILCombine(pPrinters, idPointer);
                SHFILEINFO info = new SHFILEINFO();
                var infoResult = SHGetFileInfo(pPrinter, 0, ref info, (uint)Marshal.SizeOf(info), SHGFI_ICON | SHGFI_LARGEICON | SHGFI_PIDL);

                // 0 以外のとき取得に成功している
                if (infoResult != IntPtr.Zero)
                {
                    BitmapSource iconSource;

                    try
                    {
                        iconSource = Imaging.CreateBitmapSourceFromHIcon(info.hIcon, Int32Rect.Empty, null);
                        iconSource.Freeze();
                    }
                    finally
                    {
                        DestroyIcon(info.hIcon);
                    }

                    icons.Add(Tuple.Create(printerName, iconSource));
                }
            }

            Marshal.FinalReleaseComObject(enumIDList);
            Marshal.FinalReleaseComObject(shell);

            return icons;
        }

        #endregion
    }
}
