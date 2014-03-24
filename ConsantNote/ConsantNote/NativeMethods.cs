using System;
using System.Runtime.InteropServices;

namespace ConstantNote
{
    class NativeMethods
    {
        #region Constants
        internal const Int32 WM_SYSCOMMAND = 0x112;
        internal const Int32 MF_ENABLED = 0x000;
        internal const Int32 MF_GRAYED = 0x001;
        internal const Int32 MF_SEPARATOR = 0x800;
        internal const Int32 MF_BYPOSITION = 0x400;
        internal const Int32 MF_BYCOMMAND = 0x00000;
        internal const Int32 SC_CLOSE = 0xF060;
        internal const Int32 SC_SIZE = 0xF000;
        internal const Int32 SC_MOVE = 0xF010;
        internal const Int32 SC_MAXIMIZE = 0xF030;
        #endregion

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        internal static extern bool ModifyMenu(IntPtr hMenu, Int32 wPosition, Int32 wFlags, Int32 wIDNewItem, string lpNewItem);

        [DllImport("user32.dll")]
        internal static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        internal static extern bool InsertMenu(IntPtr hMenu, Int32 wPosition, Int32 wFlags, Int32 wIDNewItem, string lpNewItem);

        [DllImport("user32.dll")]
        internal static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);

        [DllImport("user32.dll")]
        internal static extern int TrackPopupMenu(IntPtr hMenu, uint uFlags, int x, int y,
           int nReserved, IntPtr hWnd, IntPtr prcRect);
        [DllImport("user32.dll")]
        internal static extern bool EnableMenuItem(IntPtr Hmenu, uint Uposition, uint UEnable);
    }
}
