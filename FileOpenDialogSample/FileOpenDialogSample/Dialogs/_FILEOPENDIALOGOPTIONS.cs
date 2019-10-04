using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileOpenDialogSample.Dialogs
{
    /// <summary>
    /// <see cref="_FILEOPENDIALOGOPTIONS"/> 列挙型は、[開く] または [保存] ダイアログで使用できるオプションのセットを定義します。
    /// </summary>
    [Flags]
    public enum _FILEOPENDIALOGOPTIONS : uint
    {
        FOS_OVERWRITEPROMPT = 0x00000002,
        FOS_STRICTFILETYPES = 0x00000004,
        FOS_NOCHANGEDIR = 0x00000008,
        /// <summary>
        /// ファイルではなくフォルダを選択できる [開く] ダイアログボックスを表示します。
        /// </summary>
        FOS_PICKFOLDERS = 0x00000020,
        /// <summary>
        /// ファイルシステムのアイテムを返却します。
        /// </summary>
        FOS_FORCEFILESYSTEM = 0x00000040,
        FOS_ALLNONSTORAGEITEMS = 0x00000080,
        FOS_NOVALIDATE = 0x00000100,
        FOS_ALLOWMULTISELECT = 0x00000200,
        FOS_PATHMUSTEXIST = 0x00000800,
        FOS_FILEMUSTEXIST = 0x00001000,
        FOS_CREATEPROMPT = 0x00002000,
        FOS_SHAREAWARE = 0x00004000,
        FOS_NOREADONLYRETURN = 0x00008000,
        FOS_NOTESTFILECREATE = 0x00010000,
        FOS_HIDEMRUPLACES = 0x00020000,
        FOS_HIDEPINNEDPLACES = 0x00040000,
        FOS_NODEREFERENCELINKS = 0x00100000,
        FOS_DONTADDTORECENT = 0x02000000,
        FOS_FORCESHOWHIDDEN = 0x10000000,
        FOS_DEFAULTNOMINIMODE = 0x20000000,
        FOS_FORCEPREVIEWPANEON = 0x40000000,
        FOS_SUPPORTSTREAMABLEITEMS = 0x80000000
    }

}
