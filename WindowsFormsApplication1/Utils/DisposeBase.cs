using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MsAccessRestrictor.Utils {
    // Design pattern for a base class.
    internal abstract class DisposeBase : IDisposable {
        private bool disposed = false;

        //Implement IDisposable.
        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing) {
            if (!disposed)
            {
                if (disposing)
                {
                    // Free other state (managed objects).
                }
                // Free your own state (unmanaged objects).
                // Set large fields to null.
                disposed = true;
            }
        }

        // Use C# destructor syntax for finalization code.
        ~DisposeBase() {
            // Simply call Dispose(false).
            Dispose(false);
        }
    }
}
