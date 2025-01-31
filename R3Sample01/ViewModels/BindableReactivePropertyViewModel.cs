using R3;

namespace R3Sample01.ViewModels
{
    public class BindableReactivePropertyViewModel
    {
        public BindableReactiveProperty<bool> IsChecked { get; } = new(false);
    }
}
