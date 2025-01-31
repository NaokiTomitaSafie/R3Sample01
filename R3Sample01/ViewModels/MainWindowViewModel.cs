using R3;
using System.Windows.Controls;

namespace R3Sample01.ViewModels
{
    public class MainWindowViewModel
    {
        public BindableReactiveProperty<int> PageIndex { get; } = new(0);
        public BindableReactiveProperty<int> PageMax { get; } = new(0);
        public IReadOnlyBindableReactiveProperty<string?> PageName { get; }
        public BindableReactiveProperty<object?> SelectedItem { get; } = new();
        public ReactiveCommand PrevCommand { get; }
        public ReactiveCommand NextCommand { get; }
        public ReactiveCommand<object> TabLoadedCommand { get; }



        public MainWindowViewModel()
        {
            this.PageName = this.SelectedItem
                .Select(x => (x as TabItem)?.Content?.GetType().Name)
                .ToReadOnlyBindableReactiveProperty();

            this.PrevCommand = this.PageIndex
                .Select(x => x > 0)
                .ToReactiveCommand(_ => this.PageIndex.Value--);

            this.NextCommand = this.PageIndex
                .CombineLatest(this.PageMax, (index, max) => index < max - 1)
                .ToReactiveCommand(_ => this.PageIndex.Value++);

            this.TabLoadedCommand = new ReactiveCommand<object>(x =>
            {
                if (x is ItemsControl ic)
                {
                    this.PageMax.Value = ic.Items.Count;
                }
            });
        }
    }
}
