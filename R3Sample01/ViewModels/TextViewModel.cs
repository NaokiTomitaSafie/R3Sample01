using R3;
using R3Sample01.Common;

namespace R3Sample01.ViewModels
{
    public class TextViewModel : DisposableBase
    {
        public BindableReactiveProperty<string?> Text { get; } = new(string.Empty);
        public IReadOnlyBindableReactiveProperty<string?> ConvertedTextRealtime { get; }
        public IReadOnlyBindableReactiveProperty<string?> ConvertedTextDelay { get; }
        public IReadOnlyBindableReactiveProperty<string?> ConvertedTextDebounce { get; }

        public TextViewModel()
        {
            this.ConvertedTextRealtime = this.Text
                .Select(x => x?.ToUpper())
                .ToReadOnlyBindableReactiveProperty()
                .AddTo(ref this.disposables);

            this.ConvertedTextDelay = this.Text
                .Delay(TimeSpan.FromSeconds(1), TimeProvider.System)
                .Select(x => x?.ToUpper())
                .ToReadOnlyBindableReactiveProperty()
                .AddTo(ref this.disposables);

            this.ConvertedTextDebounce = this.Text
                .Debounce(TimeSpan.FromSeconds(1))
                .Select(x => x?.ToUpper())
                .ToReadOnlyBindableReactiveProperty()
                .AddTo(ref this.disposables);
        }
    }
}
