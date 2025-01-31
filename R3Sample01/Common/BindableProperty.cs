using System;
using System.ComponentModel;

namespace R3Sample01.Common
{
    public class BindableProperty<T> : INotifyPropertyChanged where T : struct, IEquatable<T>
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
            => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private T value;
        public T Value
        {
            get => this.value;
            set
            {
                if (!this.value.Equals(value))
                {
                    this.value = value;
                    RaisePropertyChanged(nameof(this.Value));
                }
            }
        }

        public BindableProperty()
        {
        }

        public BindableProperty(T initialValue)
        {
            this.value = initialValue;
        }
    }
}
