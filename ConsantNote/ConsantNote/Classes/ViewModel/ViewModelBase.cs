using System.ComponentModel;
using System.Windows.Threading;
using ConsantNote.Annotations;

namespace ConstantNote.Classes.ViewModel
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        /// <summary>
        /// The dispatcher to access the ui thread
        /// </summary>
        public Dispatcher UiDispatcher { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
