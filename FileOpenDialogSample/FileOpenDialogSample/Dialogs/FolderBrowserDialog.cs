using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;

namespace FileOpenDialogSample.Dialogs
{
    /// <summary>
    /// FolderBrowserDialog クラスは、フォルダーを選択する機能を提供するクラスです。
    /// <para>
    /// <see cref="Microsoft.WindowsAPICodePack.Dialogs.CommonFileDialog"/> クラスを利用したフォルダーの選択に近い機能を提供します。
    /// </para>
    /// </summary>
    public class FolderBrowserDialog
    {
        #region DllImports

        [DllImport("shell32.dll")]
        private static extern int SHILCreateFromPath([MarshalAs(UnmanagedType.LPWStr)] string pszPath, out IntPtr ppIdl, ref uint rgflnOut);

        [DllImport("shell32.dll")]
        private static extern int SHCreateShellItem(IntPtr pidlParent, IntPtr psfParent, IntPtr pidl, out IShellItem ppsi);

        #endregion

        #region Private Classes & Interfaces

        [ComImport]
        [Guid("DC1C5A9C-E88A-4dde-A5A1-60F82A20AEF7")]
        private class FileOpenDialogInternal { }

        [ComImport]
        [Guid("43826D1E-E718-42EE-BC55-A1E261C37BFE")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IShellItem
        {
            void BindToHandler(); // 省略宣言
            void GetParent(); // 省略宣言
            void GetDisplayName([In] SIGDN sigdnName, [MarshalAs(UnmanagedType.LPWStr)] out string ppszName);
            void GetAttributes();  // 省略宣言
            void Compare();  // 省略宣言
        }

        [ComImport]
        [Guid("42f85136-db7e-439c-85f1-e4075d135fc8")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IFileOpenDialog
        {
            [PreserveSig]
            uint Show([In] IntPtr parent); // IModalWindow
            void SetFileTypes();  // 省略宣言
            void SetFileTypeIndex([In] uint iFileType);
            void GetFileTypeIndex(out uint piFileType);
            void Advise(); // 省略宣言
            void Unadvise();
            void SetOptions([In] _FILEOPENDIALOGOPTIONS fos);
            void GetOptions(out _FILEOPENDIALOGOPTIONS pfos);
            void SetDefaultFolder(IShellItem psi);
            void SetFolder(IShellItem psi);
            void GetFolder(out IShellItem ppsi);
            void GetCurrentSelection(out IShellItem ppsi);
            void SetFileName([In, MarshalAs(UnmanagedType.LPWStr)] string pszName);
            void GetFileName([MarshalAs(UnmanagedType.LPWStr)] out string pszName);
            void SetTitle([In, MarshalAs(UnmanagedType.LPWStr)] string pszTitle);
            void SetOkButtonLabel([In, MarshalAs(UnmanagedType.LPWStr)] string pszText);
            void SetFileNameLabel([In, MarshalAs(UnmanagedType.LPWStr)] string pszLabel);
            void GetResult(out IShellItem ppsi);
            void AddPlace(IShellItem psi, int alignment);
            void SetDefaultExtension([In, MarshalAs(UnmanagedType.LPWStr)] string pszDefaultExtension);
            void Close(int hr);
            void SetClientGuid();  // 省略宣言
            void ClearClientData();
            void SetFilter([MarshalAs(UnmanagedType.Interface)] IntPtr pFilter);
            void GetResults([MarshalAs(UnmanagedType.Interface)] out IntPtr ppenum); // 省略宣言
            void GetSelectedItems([MarshalAs(UnmanagedType.Interface)] out IntPtr ppsai); // 省略宣言
        }

        #endregion

        #region Fields

        private const uint ERROR_CANCELLED = 0x800704C7;

        #endregion

        #region Properties

        /// <summary>
        /// ユーザーによって選択されたフォルダーのパスを取得または設定します。
        /// </summary>
        public string SelectedPath { get; set; }

        /// <summary>
        /// ダイアログ上に表示されるタイトルのテキストを取得または設定します。
        /// </summary>
        public string Title { get; set; }

        #endregion

        #region Initializes

        /// <summary>
        /// <see cref="FolderBrowserDialog"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        public FolderBrowserDialog() { }

        #endregion

        #region Events

        #endregion

        #region Public Methods

        public DialogResult ShowDialog()
        {
            return ShowDialog(IntPtr.Zero);
        }

        public DialogResult ShowDialog(Window owner)
        {
            if (owner == null)
            {
                throw new ArgumentNullException("指定したウィンドウは null です。オーナーを正しく設定できません。");
            }

            var handle = new WindowInteropHelper(owner).Handle;

            return ShowDialog(handle);
        }

        public DialogResult ShowDialog(IntPtr owner)
        {
            var dialog = new FileOpenDialogInternal() as IFileOpenDialog;

            try
            {
                IShellItem item;
                string selectedPath;

                dialog.SetOptions(_FILEOPENDIALOGOPTIONS.FOS_PICKFOLDERS | _FILEOPENDIALOGOPTIONS.FOS_FORCEFILESYSTEM);

                if (!string.IsNullOrEmpty(SelectedPath))
                {
                    IntPtr idl = IntPtr.Zero; // path の intptr
                    uint attributes = 0;

                    if (SHILCreateFromPath(SelectedPath, out idl, ref attributes) == 0)
                    {
                        if (SHCreateShellItem(IntPtr.Zero, IntPtr.Zero, idl, out item) == 0)
                        {
                            dialog.SetFolder(item);
                        }

                        if (idl != IntPtr.Zero)
                        {
                            Marshal.FreeCoTaskMem(idl);
                        }
                    }
                }

                if (!string.IsNullOrEmpty(Title))
                {
                    dialog.SetTitle(Title);
                }

                var hr = dialog.Show(owner);

                // 選択のキャンセルまたは例外
                if (hr == ERROR_CANCELLED) return DialogResult.Cancel;
                if (hr != 0) return DialogResult.Abort;

                dialog.GetResult(out item);

                if (item != null)
                {
                    item.GetDisplayName(SIGDN.SIGDN_FILESYSPATH, out selectedPath);
                    SelectedPath = selectedPath;
                }
                else
                {
                    return DialogResult.Abort;
                }

                return DialogResult.OK;
            }
            finally
            {
                Marshal.FinalReleaseComObject(dialog);
            }
        }

        #endregion
    }
}
