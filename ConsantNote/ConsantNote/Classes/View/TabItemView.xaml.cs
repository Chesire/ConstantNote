using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ConsantNote.Annotations;
using ConstantNote.Resources;

namespace ConstantNote.Classes.View
{
    /// <summary>
    /// Interaction logic for TabItemView.xaml
    /// </summary>
    [Serializable]
    public partial class TabItemView : TabItem , INotifyPropertyChanged, ISerializable
    {
        #region Members
        private bool _hasBeenEdited;
        private bool _canClose;
        #endregion

        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler CloseTabItem;
        #endregion

        #region Properties
        public string FilePath { get; private set; }

        public bool HasBeenEdited
        {
            get { return _hasBeenEdited; }
            private set
            {
                _hasBeenEdited = value;
                OnPropertyChanged("HasBeenEdited");
            }
        }

        public bool CanClose
        {
            get { return _canClose; }
            private set
            {
                _canClose = value;
                OnPropertyChanged("CanClose");
            }
        }
        #endregion

        #region Constructor
        public TabItemView(string filePath)
        {
            Initialize(filePath);
        }

        public TabItemView(SerializationInfo info, StreamingContext ctxt)
        {
            Initialize((string)info.GetValue("FilePath", typeof(string)));
        }

        private void Initialize(string filePath)
        {
            InitializeComponent();
            FilePath = filePath;
            SetupTabItem();
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("FilePath", FilePath);
        }
        #endregion

        #region Event Handling
        private void TextFileBlock_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            HasBeenEdited = true;
        }

        private void TextFileBlock_OnLostFocus(object sender, RoutedEventArgs e)
        {
            if (!HasBeenEdited) return;
            SaveItem();
        }

        private void CrossImage_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!HasBeenEdited) return;
            SaveItem();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            if (CloseTabItem != null) 
                CloseTabItem(this, new EventArgs());
        }
        #endregion

        #region Methods
        private void SetupTabItem()
        {
            if (string.IsNullOrEmpty(FilePath))
            {
                TextFileBlock.IsHitTestVisible = false;
                TextFileBlock.Text = MainResource.Info;
                HasBeenEdited = false;
                CanClose = false;
                return;
            }
            HeaderTextBlock.Text = new FileInfo(FilePath).Name;
            TextFileBlock.Text = File.ReadAllText(FilePath);
            HasBeenEdited = false;
            CanClose = true;
        }

        internal void SaveItem()
        {
            if (string.IsNullOrEmpty(FilePath)) return;
            if (!HasBeenEdited) return;

            try
            {
                using (var sw = File.CreateText(FilePath))
                {
                    sw.Write(TextFileBlock.Text);
                }
                HasBeenEdited = false;
                Console.WriteLine(@"Saved");
            }
            catch (Exception ex)
            {
                // Need to store the file in a queue to write to, so eventually it goes through
                Console.WriteLine(ex);
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}