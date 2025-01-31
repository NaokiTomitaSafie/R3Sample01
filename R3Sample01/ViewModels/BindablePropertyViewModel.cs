using R3Sample01.Common;

namespace R3Sample01.ViewModels
{
    public class BindablePropertyViewModel
    {
        public BindableProperty<bool> IsChecked { get; } = new(false);
    }
}
