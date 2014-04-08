using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using ConstantNote.Classes.Controller;
using ConstantNote.Classes.ViewModel;

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
        private const Int32 _AddSaveSysMenuID = 1002;
        private const Int32 _AddInfoTabSysMenuID = 1003;
        private const Int32 _AddSettingsSysMenuID = 1004;
        #endregion

        private readonly MainWindowViewModel _vm;

        public MainWindow()
        {
            InitializeComponent();
            Console.WriteLine(SettingsController.StateLocation);
            object tempObject = Serializer.Deserialize(GetSaveLocation());
            _vm = tempObject == null ? 
                _vm = new MainWindowViewModel { UiDispatcher = Dispatcher } :
                (MainWindowViewModel)Convert.ChangeType(tempObject, typeof(MainWindowViewModel));

            DataContext = _vm;
            ApplicationDimensionsChanged(null, null);
            SettingsController.ApplicationDimensionsChanged += ApplicationDimensionsChanged;
        }

        private void ApplicationDimensionsChanged(object sender, EventArgs eventArgs)
        {
            Height = SettingsController.ApplicationHeight;
            Width = SettingsController.ApplicationWidth;
            SettingsController.Save();
        }

        private void UIElement_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            SetupMenuItems();
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            Serializer.Serialize(GetSaveLocation(), _vm);
            SettingsController.ApplicationDimensionsChanged -= ApplicationDimensionsChanged;
            SettingsController.Save();
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
            NativeMethods.InsertMenu(systemMenuHandle, 8, NativeMethods.MF_BYPOSITION | NativeMethods.MF_ENABLED, _AddSaveSysMenuID, "Save all");
            NativeMethods.InsertMenu(systemMenuHandle, 9, NativeMethods.MF_BYPOSITION | NativeMethods.MF_ENABLED, _AddInfoTabSysMenuID, "Select info tab");
            NativeMethods.InsertMenu(systemMenuHandle, 10, NativeMethods.MF_BYPOSITION | NativeMethods.MF_ENABLED, _AddSettingsSysMenuID, "Change Settings");
        }

        private static string GetSaveLocation()
        {
            string retvalue = "notes.srl";
            string settingsLocation = SettingsController.StateLocation;
            if(!string.IsNullOrEmpty(settingsLocation))
            {
                retvalue = string.Format(@"{0}\notes.srl", settingsLocation);
            }

            return retvalue;
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
                    _vm.SelectNewFile();
                    break;

                case _AddNewFolderSysMenuID:
                    _vm.SelectNewFolder();
                    break;

                case _AddSaveSysMenuID:
                    _vm.SaveAll();
                    break;

                case _AddInfoTabSysMenuID:
                    _vm.SelectedIndex = 0;
                    break;

                case _AddSettingsSysMenuID:
                    Settings settingsWindow = new Settings();
                    settingsWindow.ShowDialog();
                    break;

                default:
                    NativeMethods.SendMessage(new WindowInteropHelper(this).Handle, 0x112, (IntPtr)selection, IntPtr.Zero);
                    break;
            }
        }
        #endregion
    }
}
