using R3;

namespace R3Sample01.Common
{
    public class DisposableBase : IDisposable
    {
        protected DisposableBag disposables;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.disposables.Dispose();
            }
        }
    }
}
