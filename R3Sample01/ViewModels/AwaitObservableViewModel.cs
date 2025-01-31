using R3;
using R3Sample01.Common;

namespace R3Sample01.ViewModels
{
    public class AwaitObservableViewModel : DisposableBase
    {
        public BindableReactiveProperty<string> Status { get; } = new();

        public AsyncReactiveCommand StartCommand { get; }
        public ReactiveCommand CountCommand { get; }

        public BindableReactiveProperty<int> Count { get; } = new();

        public AwaitObservableViewModel()
        {
            this.StartCommand = new AsyncReactiveCommand(async (_, ct) =>
            {
                this.Status.Value = $"start count={this.Count.Value}";
                await this.Count.Skip(3).FirstAsync();
                this.Status.Value = $"completed count={this.Count.Value}";
            });
            this.CountCommand = new ReactiveCommand(_ => this.Count.Value++);
        }
    }
}
