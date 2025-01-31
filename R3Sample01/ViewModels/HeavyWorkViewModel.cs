using R3Sample01.Common;

namespace R3Sample01.ViewModels
{
    public class HeavyWorkViewModel
    {
        public AsyncReactiveCommand WorkCommand { get; }
        public HeavyWorkViewModel()
        {
            this.WorkCommand = new AsyncReactiveCommand((_, ct) => this.WorkAsync(ct));
        }
        private async ValueTask WorkAsync(CancellationToken cancellationToken)
        {
            await Task.Delay(TimeSpan.FromSeconds(3), cancellationToken);
        }
    }
}
