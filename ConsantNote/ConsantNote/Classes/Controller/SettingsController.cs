using System;
using ConstantNote.Properties;

namespace ConstantNote.Classes.Controller
{
    static class SettingsController
    {
        public static EventHandler ApplicationDimensionsChanged;

        static internal string StateLocation
        {
            get { return Settings.Default.StateLocation; }
            set { Settings.Default.StateLocation = value; }
        }

        static internal int ApplicationHeight
        {
            get { return Settings.Default.ApplicationHeight; }
            set
            {
                Settings.Default.ApplicationHeight = value;
                ApplicationDimensionsChanged(null, null);
            }
        }

        static internal int ApplicationWidth
        {
            get { return Settings.Default.ApplicationWidth; }
            set
            {
                Settings.Default.ApplicationWidth = value;
                ApplicationDimensionsChanged(null, null);
            }
        }

        static internal void Save()
        {
            Settings.Default.Save();
        }
    }
}
