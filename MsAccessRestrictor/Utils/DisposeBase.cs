using System;

namespace MsAccessRestrictor.Utils {
    // Design pattern for a base class.
    public abstract class DisposeBase : IDisposable {
        private bool _disposed; // false by default

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing) {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Free other state (managed objects).
                }
                // Free your own state (unmanaged objects).
                // Set large fields to null.
                _disposed = true;
            }
        }

        ~DisposeBase() {
            Dispose(false);
        }
    }
}
