using System.ComponentModel;

namespace R3Sample01.ViewModels
{
    public class SimpleBindingViewModel : INotifyPropertyChanged
    {
        public bool IsChecked
        {
            get { return _fieldIsChecked; }
            set
            {
                if (_fieldIsChecked != value)
                {
                    _fieldIsChecked = value;
                    RaisePropertyChanged(nameof(IsChecked));
                }
            }
        }
        private bool _fieldIsChecked;


        public event PropertyChangedEventHandler? PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
            => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
