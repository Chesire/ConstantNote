using System;
using System.IO;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace ConstantNote.Classes.Controller
{
    static class FileController
    {
        /// <summary>
        /// Brings up a file dialog to select a file
        /// </summary>
        /// <param name="getFolder">Get whole folder instead</param>
        /// <returns>Path to the file / folder</returns>
        public static string GetFile(bool getFolder = false)
        {
            string retvalue = "";
            var dialog = new CommonOpenFileDialog
            {
                IsFolderPicker = getFolder,
                ShowHiddenItems = true,
                AllowNonFileSystemItems = true,
                ShowPlacesList = true,
                Multiselect = false,
                NavigateToShortcut = true,
                Title = "Select a File"
            };

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                retvalue = dialog.FileName;
            }

            return retvalue;
        }

        /// <summary>
        /// Reads the contents of the <paramref name="filePath"/> and returns it
        /// </summary>
        /// <param name="filePath">Path to the file to read</param>
        /// <returns></returns>
        public static string ReadFile(string filePath)
        {
            if (filePath == null) return null;
            if (!File.Exists(filePath)) return null;

            string retvalue;

            try
            {
                retvalue = File.ReadAllText(filePath);
            }
            catch
            {
                retvalue = null;
            }

            return retvalue;
        }
    }
}
