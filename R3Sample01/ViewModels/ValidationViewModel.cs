using R3;
using R3Sample01.Common;

namespace R3Sample01.ViewModels
{
    public class ValidationViewModel : DisposableBase
    {
        public BindableReactiveProperty<string?> Address { get; }
        public BindableReactiveProperty<string?> Pass { get; }

        public ReactiveCommand Command { get; }

        public ValidationViewModel()
        {
            this.Address = new BindableReactiveProperty<string?>()
                .EnableValidation(x => IsAddressValid(x) ? null : new Exception("invalid address"))
                .AddTo(ref this.disposables);

            this.Pass = new BindableReactiveProperty<string?>()
                .EnableValidation(x => IsPassValid(x) ? null : new Exception("invalid pass"))
                .AddTo(ref this.disposables);

            this.Command = this.Address
                .CombineLatest(this.Pass, (a, p) => IsAddressValid(a) && IsPassValid(p))
                .ToReactiveCommand(_ => { })
                .AddTo(ref this.disposables);
        }

        private static bool IsAddressValid(string? str)
        {
            return !string.IsNullOrEmpty(str) && str.Contains('@');
        }
        private static bool IsPassValid(string? str)
        {
            return !string.IsNullOrEmpty(str);
        }
    }
}
