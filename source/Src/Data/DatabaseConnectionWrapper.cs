﻿/*
Copyright 2013 Microsoft Corporation
Licensed under the Apache License, Version 2.0 (the "License");

you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

using System;
using System.Data.Common;
using System.Threading;

namespace Microsoft.Practices.EnterpriseLibrary.Data
{
    /// <summary>
    /// This is a small helper class used to manage closing a connection 
    /// in the presence of transaction pooling. We can't actually
    /// close the connection until everyone using it is done, thus, we
    /// need reference counting.
    /// </summary>
    /// <remarks>
    /// User code should not use this class directly - it's used internally
    /// by the authors of DAAB providers to manage connections when using
    /// the DAAB methods.
    /// </remarks>
    public class DatabaseConnectionWrapper : IDisposable
    {
        private int refCount;

        /// <summary>
        /// Create a new <see cref="DatabaseConnectionWrapper"/> that wraps
        /// the given <paramref name="connection"/>.
        /// </summary>
        /// <param name="connection">Database connection to manage the lifetime of.</param>
        public DatabaseConnectionWrapper(DbConnection connection)
        {
            Connection = connection;
            refCount = 1;
        }

        /// <summary>
        /// The underlying <see cref="DbConnection"/> we're managing.
        /// </summary>
        public DbConnection Connection { get; private set; }

        /// <summary>
        /// Has this wrapper disposed the underlying connection?
        /// </summary>
        public bool IsDisposed
        {
            get { return refCount == 0; }
        }

        #region IDisposable Members

        /// <summary>
        /// Decrement the reference count and, if refcount is 0, close the underlying connection.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1816:CallGCSuppressFinalizeCorrectly", Justification = "As designed. This is a reference counting disposable.")]
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Usual Dispose pattern folderal to shut up FxCop.
        /// </summary>
        /// <param name="disposing">true if called via <see cref="DatabaseConnectionWrapper.Dispose()"/> method, false
        /// if called from finalizer. Of course, since we have no finalizer this will never
        /// be false.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1816:CallGCSuppressFinalizeCorrectly", Justification = "As designed. This is a reference counting disposable.")]
        protected virtual void Dispose(bool disposing)
        {
            if(disposing)
            {
                int count = Interlocked.Decrement(ref refCount);
                if (count == 0)
                {
                    Connection.Dispose();
                    Connection = null;
                    GC.SuppressFinalize(this);
                }
            }            
        }

        #endregion

        /// <summary>
        /// Increment the reference count for the wrapped connection.
        /// </summary>
        public DatabaseConnectionWrapper AddRef()
        {
            Interlocked.Increment(ref refCount);
            return this;
        }
    }
}
