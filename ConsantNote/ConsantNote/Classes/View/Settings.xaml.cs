using System;
using System.Windows;
using ConstantNote.Classes.Controller;

namespace ConstantNote.Classes.View
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        private readonly int _initialHeight;
        private readonly int _initialWidth;
        private readonly string _initialLocation;

        public Settings()
        {
            InitializeComponent();
            _initialHeight = SettingsController.ApplicationHeight;
            _initialWidth = SettingsController.ApplicationWidth;
            HeightBox.Text =_initialHeight.ToString();
            WidthBox.Text = _initialWidth.ToString();

            FileDirTextBlock.Text = _initialLocation = SettingsController.StateLocation;
        }

        private void SelectSaveDir_OnClick(object sender, RoutedEventArgs e)
        {
            FileDirTextBlock.Text = FileController.GetFile(true);
        }

        private void SaveSettings_OnClick(object sender, RoutedEventArgs e)
        {
            SaveStateLocation();
            if (SaveHeight() && SaveWidth())
            {
                Close();
            }
        }

        private bool SaveHeight()
        {
            int value;
            try
            {
                value = Convert.ToInt32(HeightBox.Text);
                if (value == _initialHeight) return true;
            }
            catch (Exception)
            {
                // Not a valid 32 num
                MessageBox.Show(string.Format(@"{0} is not a valid number", HeightBox.Text));
                return false;
            }
            

            SettingsController.ApplicationHeight = value;
            return true;
        }

        private bool SaveWidth()
        {
            int value;
            try
            {
                value = Convert.ToInt32(WidthBox.Text);
                if (value == _initialWidth) return true;
            }
            catch (Exception)
            {
                // Not a valid 32 num
                MessageBox.Show(string.Format(@"{0} is not a valid number", WidthBox.Text));
                return false;
            }
            
            SettingsController.ApplicationWidth = value;
            return true;
        }

        private void SaveStateLocation()
        {
            if (_initialLocation != FileDirTextBlock.Text)
                SettingsController.StateLocation = FileDirTextBlock.Text;
        }
    }
}
