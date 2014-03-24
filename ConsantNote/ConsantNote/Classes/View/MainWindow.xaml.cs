using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

namespace ConstantNote.Classes.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region SysMenu Constants
        private const Int32 _AddNewFileSysMenuID = 1000;
        private const Int32 _AddNewFolderSysMenuID = 1001;
        private const Int32 _AddInfoTabSysMenuID = 1002;
        #endregion

        public MainWindow()
        {
            InitializeComponent();
            Vm.UiDispatcher = Dispatcher;
        }

        private void UIElement_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            SetupMenuItems();
        }

        private void SetupMenuItems()
        {
            IntPtr systemMenuHandle = NativeMethods.GetSystemMenu(Handle, false);

            NativeMethods.EnableMenuItem(NativeMethods.GetSystemMenu(Handle, false), NativeMethods.SC_MAXIMIZE, NativeMethods.MF_BYCOMMAND | NativeMethods.MF_GRAYED);
            NativeMethods.EnableMenuItem(NativeMethods.GetSystemMenu(Handle, false), NativeMethods.SC_MOVE, NativeMethods.MF_BYCOMMAND | NativeMethods.MF_GRAYED);
            NativeMethods.EnableMenuItem(NativeMethods.GetSystemMenu(Handle, false), NativeMethods.SC_SIZE, NativeMethods.MF_BYCOMMAND | NativeMethods.MF_GRAYED);

            // Create our new System Menu items just before the Close menu item
            NativeMethods.InsertMenu(systemMenuHandle, 5, NativeMethods.MF_BYPOSITION | NativeMethods.MF_SEPARATOR, 0, string.Empty); // <-- Add a menu seperator
            NativeMethods.InsertMenu(systemMenuHandle, 6, NativeMethods.MF_BYPOSITION | NativeMethods.MF_ENABLED, _AddNewFileSysMenuID, "Add a new file");
            NativeMethods.InsertMenu(systemMenuHandle, 7, NativeMethods.MF_BYPOSITION | NativeMethods.MF_ENABLED, _AddNewFolderSysMenuID, "Add a new folder");
            NativeMethods.InsertMenu(systemMenuHandle, 8, NativeMethods.MF_BYPOSITION | NativeMethods.MF_ENABLED, _AddInfoTabSysMenuID, "Select info tab");
        }

        #region System Menu Hooks
        private IntPtr Handle { get { return new WindowInteropHelper(this).Handle; } }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Right)
            {
                IntPtr hWnd = new WindowInteropHelper(this).Handle;
                IntPtr hMenu = NativeMethods.GetSystemMenu(hWnd, false);
                Point p = PointToScreen(e.GetPosition(this));
                int cmd = NativeMethods.TrackPopupMenu(hMenu, 0x100, (int)p.X, (int)p.Y, 0, hWnd, IntPtr.Zero);

                if (cmd > 0)
                    OnMenuSelection(cmd);
            }
        }

        private void OnMenuSelection(Int32 selection)
        {
            switch (selection)
            {
                case _AddNewFileSysMenuID:
                    Vm.SelectNewFile();
                    break;

                case _AddNewFolderSysMenuID:
                    Vm.SelectNewFolder();
                    break;

                case _AddInfoTabSysMenuID:
                    Vm.SelectedIndex = 0;
                    break;

                default:
                    NativeMethods.SendMessage(new WindowInteropHelper(this).Handle, 0x112, (IntPtr)selection, IntPtr.Zero);
                    break;
            }
        }
        #endregion
    }
}
