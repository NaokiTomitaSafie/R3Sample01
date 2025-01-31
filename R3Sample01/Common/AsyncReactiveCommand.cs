using R3;

namespace R3Sample01.Common
{
    public class AsyncReactiveCommand : AsyncReactiveCommand<Unit>
    {
        public AsyncReactiveCommand()
            : base()
        {
        }
        public AsyncReactiveCommand(Observable<bool> canExecuteSource, bool initialCanExecute)
            : base(canExecuteSource, initialCanExecute)
        {
        }
        public AsyncReactiveCommand(
            Func<Unit, CancellationToken, ValueTask> onNextAsync,
            bool configureAwait = true,
            bool cancelOnCompleted = false,
            int maxConcurrent = -1)
            : base(onNextAsync, configureAwait, cancelOnCompleted, maxConcurrent)
        {
        }
        public AsyncReactiveCommand(
            Func<ValueTask> onNextAsync,
            bool configureAwait = true,
            bool cancelOnCompleted = false,
            int maxConcurrent = -1)
            : base(onNextAsync, configureAwait, cancelOnCompleted, maxConcurrent)
        {
        }
        public AsyncReactiveCommand(
            Func<Task> onNextAsync,
            bool configureAwait = true,
            bool cancelOnCompleted = false,
            int maxConcurrent = -1)
            : base(onNextAsync, configureAwait, cancelOnCompleted, maxConcurrent)
        {
        }
    }

    public class AsyncReactiveCommand<T> : System.Windows.Input.ICommand, IDisposable
    {
        private readonly ReactiveCommand<T> command;
        private readonly ReactiveProperty<bool> isNotExecuting = new(true);
        public IReadOnlyBindableReactiveProperty<bool> IsExecuting
            => this.isNotExecuting.Select(x => !x).ToReadOnlyBindableReactiveProperty();

        private DisposableBag disposables;

        public event EventHandler? CanExecuteChanged;

        public Observable<T> Observable => this.command.AsObservable();

        public AsyncReactiveCommand()
        {
            this.command = new ReactiveCommand<T>(
                this.isNotExecuting,
                this.isNotExecuting.Value);
            this.command.CanExecuteChanged += (o, e) => this.CanExecuteChanged?.Invoke(this, e);
        }


        public AsyncReactiveCommand(Observable<bool> canExecuteSource, bool initialCanExecute)
        {
            this.command = new ReactiveCommand<T>(
                canExecuteSource.CombineLatest(this.isNotExecuting, (a, b) => a && b),
                initialCanExecute);
            this.command.CanExecuteChanged += (o, e) => this.CanExecuteChanged?.Invoke(this, e);
        }


        public AsyncReactiveCommand(
            Func<T, CancellationToken, ValueTask> onNextAsync,
            bool configureAwait = true,
            bool cancelOnCompleted = false,
            int maxConcurrent = -1) : this()
        {
            this.WithSubscribe(onNextAsync,
                configureAwait, cancelOnCompleted, maxConcurrent);
        }
        public AsyncReactiveCommand(
            Func<ValueTask> onNextAsync,
            bool configureAwait = true,
            bool cancelOnCompleted = false,
            int maxConcurrent = -1) : this()
        {
            this.WithSubscribe((a, ct) => onNextAsync(),
                configureAwait, cancelOnCompleted, maxConcurrent);
        }
        public AsyncReactiveCommand(
            Func<Task> onNextAsync,
            bool configureAwait = true,
            bool cancelOnCompleted = false,
            int maxConcurrent = -1) : this()
        {
            this.WithSubscribe((a, ct) => onNextAsync(),
                configureAwait, cancelOnCompleted, maxConcurrent);
        }


        private AsyncReactiveCommand<T> WithSubscribeCore(
            Func<T, CancellationToken, ValueTask> onNextAsync,
            bool configureAwait,
            bool cancelOnCompleted,
            int maxConcurrent)
        {
            this.command
                .SubscribeAwait(async (a, ct) =>
                {
                    this.isNotExecuting.Value = false;
                    try
                    {
                        await onNextAsync(a, ct);
                    }
                    finally
                    {
                        if (!this.isNotExecuting.IsDisposed)
                        {
                            this.isNotExecuting.Value = true;
                        }
                    }
                }, AwaitOperation.Drop, configureAwait, cancelOnCompleted, maxConcurrent)
                .AddTo(ref this.disposables);
            return this;
        }
        internal AsyncReactiveCommand<T> WithSubscribe(
            Func<T, CancellationToken, ValueTask> onNextAsync,
            bool configureAwait = true,
            bool cancelOnCompleted = false,
            int maxConcurrent = -1)
        {
            return this.WithSubscribeCore(onNextAsync,
                configureAwait, cancelOnCompleted, maxConcurrent);
        }
        internal AsyncReactiveCommand<T> WithSubscribe<U>(
            Func<T, CancellationToken, ValueTask<U>> onNextAsync,
            bool configureAwait = true,
            bool cancelOnCompleted = false,
            int maxConcurrent = -1)
        {
            return this.WithSubscribeCore(async (a, ct) => await onNextAsync(a, ct),
                configureAwait, cancelOnCompleted, maxConcurrent);
        }

        internal AsyncReactiveCommand<T> WithSubscribe(
            Func<T, CancellationToken, Task> onNextAsync,
            bool configureAwait = true,
            bool cancelOnCompleted = false,
            int maxConcurrent = -1)
        {
            return this.WithSubscribeCore(async (a, ct) => await onNextAsync(a, ct),
                configureAwait, cancelOnCompleted, maxConcurrent);
        }

        public bool CanExecute(object? parameter)
            => ((System.Windows.Input.ICommand)this.command).CanExecute(parameter);


        public void Dispose()
        {
            this.command.Dispose();
            this.disposables.Dispose();
            this.isNotExecuting.Dispose();
        }

        public void Execute(object? parameter)
        {
            ((System.Windows.Input.ICommand)this.command).Execute(parameter);
        }

    }
    public static class AsyncReactiveCommandExtensions
    {
        public static AsyncReactiveCommand<T> ToAsyncReactiveCommand<T>(
            this Observable<bool> canExecuteSource, bool initialCanExecute = true)
        {
            return new AsyncReactiveCommand<T>(canExecuteSource, initialCanExecute);
        }
        public static AsyncReactiveCommand<T> ToAsyncReactiveCommand<T>(
            this Observable<bool> canExecuteSource,
            Func<T, CancellationToken, ValueTask> executeAsync,
            bool initialCanExecute = true,
            bool configureAwait = true,
            bool cancelOnCompleted = false,
            int maxSequential = -1)
        {
            var rc = new AsyncReactiveCommand<T>(canExecuteSource, initialCanExecute);
            return rc.WithSubscribe(executeAsync, configureAwait, cancelOnCompleted, maxSequential);
        }

        public static AsyncReactiveCommand<T> ToAsyncReactiveCommand<T, U>(
            this Observable<bool> canExecuteSource,
            Func<T, CancellationToken, ValueTask<U>> convertAsync,
            bool initialCanExecute = true,
            bool configureAwait = true,
            bool cancelOnCompleted = false,
            int maxSequential = -1)
        {
            var rc = new AsyncReactiveCommand<T>(canExecuteSource, initialCanExecute);
            return rc.WithSubscribe(convertAsync, configureAwait, cancelOnCompleted, maxSequential);
        }
        public static AsyncReactiveCommand<T> ToAsyncReactiveCommand<T, U>(
            this Observable<bool> canExecuteSource,
            Func<T, CancellationToken, Task<U>> convertAsync,
            bool initialCanExecute = true,
            bool configureAwait = true,
            bool cancelOnCompleted = false,
            int maxSequential = -1)
        {
            var rc = new AsyncReactiveCommand<T>(canExecuteSource, initialCanExecute);
            return rc.WithSubscribe(convertAsync, configureAwait, cancelOnCompleted, maxSequential);
        }
        public static AsyncReactiveCommand ToAsyncReactiveCommand(
            this Observable<bool> canExecuteSource,
            Func<Unit, CancellationToken, ValueTask> executeAsync,
            bool initialCanExecute = true,
            bool configureAwait = true,
            bool cancelOnCompleted = false,
            int maxSequential = -1)
        {
            var rc = new AsyncReactiveCommand(canExecuteSource, initialCanExecute);
            return (AsyncReactiveCommand)rc.WithSubscribe(executeAsync, configureAwait, cancelOnCompleted, maxSequential);
        }
        public static AsyncReactiveCommand ToAsyncReactiveCommand<U>(
            this Observable<bool> canExecuteSource,
            Func<Unit, CancellationToken, Task<U>> executeAsync,
            bool initialCanExecute = true,
            bool configureAwait = true,
            bool cancelOnCompleted = false,
            int maxSequential = -1)
        {
            var rc = new AsyncReactiveCommand(canExecuteSource, initialCanExecute);
            return (AsyncReactiveCommand)rc.WithSubscribe(executeAsync, configureAwait, cancelOnCompleted, maxSequential);
        }
    }
}
