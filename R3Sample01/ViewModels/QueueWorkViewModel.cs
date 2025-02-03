using R3;
using R3Sample01.Common;
using System.Collections.ObjectModel;

namespace R3Sample01.ViewModels
{
    public class QueueWorkViewModel : DisposableBase
    {
        public BindableReactiveProperty<string?> Text { get; } = new(string.Empty);
        public ObservableCollection<string> Results { get; } = new();

        public QueueWorkViewModel()
        {
            var items = new List<char>();

            this.Text
                .Where(x => !string.IsNullOrEmpty(x))
                .Do(x => items.Add(x![^1]))
                .Select(_ => false)
                .Merge(this.Text.Debounce(TimeSpan.FromSeconds(2)).Select(_ => true))
                .Select(force =>
                {
                    if (force || items.Count >= 3)
                    {
                        var arr = items.ToArray();
                        items.Clear();
                        return arr;
                    }
                    return [];
                })
                .Where(x => x.Length > 0)
                .Select(x => new string(x))
                .Subscribe(this.Results.Add)
                .AddTo(ref this.disposables);
        }
    }
}
