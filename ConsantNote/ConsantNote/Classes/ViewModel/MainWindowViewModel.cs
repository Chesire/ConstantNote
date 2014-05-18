using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Windows;
using ConstantNote.Classes.Controller;
using ConstantNote.Classes.View;

namespace ConstantNote.Classes.ViewModel
{
    [Serializable]
    public class MainWindowViewModel : ViewModelBase, ISerializable
    {
        #region Members
        private ObservableCollection<TabItemView> _tabsCollection;
        private int _selectedIndex;
        #endregion

        #region Constructor
        public MainWindowViewModel()
        {
            TabsCollection = new ObservableCollection<TabItemView> { new TabItemView("") { Header = "" } };
        }

        public MainWindowViewModel(SerializationInfo info, StreamingContext ctxt)
        {
            TabsCollection = (ObservableCollection<TabItemView>)info.GetValue("Items", typeof(ObservableCollection<TabItemView>));
            SelectedIndex = (int) info.GetValue("SelectedIndex", typeof (int));
        }
        #endregion

        #region Properties
        public ObservableCollection<TabItemView> TabsCollection
        {
            get { return _tabsCollection; }
            set
            {
                _tabsCollection = value;
                OnPropertyChanged("TabsCollection");
            }
        }

        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set
            {
                _selectedIndex = value;
                OnPropertyChanged("SelectedIndex");
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Serialization Method
        /// </summary>
        /// <param name="info"></param>
        /// <param name="ctxt"></param>
        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("Items", TabsCollection);
            info.AddValue("SelectedIndex", SelectedIndex);
        }
        /// <summary>
        /// Select a new file to put into the list of tabs
        /// </summary>
        public void SelectNewFile()
        {
            string newFilePath = FileController.GetFile();
            if (!string.IsNullOrEmpty(newFilePath))
            {
                AddNewFile(newFilePath);
            }
        }

        /// <summary>
        /// Select a folder of files to put into the list of tabs
        /// </summary>
        public void SelectNewFolder()
        {
            string newFolderPath = FileController.GetFile(true);
            if (!string.IsNullOrEmpty(newFolderPath))
            {
                foreach (var item in Directory.GetFiles(newFolderPath))
                {
                    AddNewFile(item);
                }
            }
        }

        /// <summary>
        /// Save all notes
        /// </summary>
        public void SaveAll()
        {
            if (TabsCollection.Count < 2) return;
            foreach (var item in TabsCollection)
            {
                item.SaveItem();
            }
        }

        private void AddNewFile(object filePath)
        {
            if (!(filePath is string)) return;

            string sFilePath = filePath as string;
            bool add = true;
            int index = 0;
            foreach (TabItemView view in TabsCollection.Where(view => view.FilePath.Equals(sFilePath)))
            {
                add = false;
                index = TabsCollection.IndexOf(view);
                break;
            }

            if (add)
            {
                string readFileText = FileController.ReadFile(sFilePath);
                if (readFileText == null)
                {
                    MessageBox.Show("Error occured trying to add requested item - {0}", sFilePath);
                    return;
                }

                TabItemView newTabItem = new TabItemView(sFilePath);
                newTabItem.CloseTabItem += NewTabItemOnCloseTabItem;
                TabsCollection.Add(newTabItem);
                SelectedIndex = TabsCollection.Count;
            }
            else
            {
                SelectedIndex = index;
            }
        }

        private void NewTabItemOnCloseTabItem(object sender, EventArgs eventArgs)
        {
            if (!(sender is TabItemView)) return;

            TabItemView tabView = sender as TabItemView;
            tabView.CloseTabItem -= NewTabItemOnCloseTabItem;
            TabsCollection.Remove(tabView);
        }
        #endregion
    }
}
