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
    /// This class represents an asynchronous operation invoked from the <see cref="Database"/> class methods.
    /// </summary>
    public class DaabAsyncResult : IAsyncResult
    {
        readonly IAsyncResult innerAsyncResult;
        readonly DbCommand command;
        readonly bool disposeCommand;
        readonly bool closeConnection;
        readonly DateTime startTime;

        /// <summary>
        /// Construct a new <see cref="DaabAsyncResult"/> instance.
        /// </summary>
        /// <param name="innerAsyncResult">The <see cref='IAsyncResult'/> object returned from the underlying
        /// async operation.</param>
        /// <param name="command">Command that was executed.</param>
        /// <param name="disposeCommand">Should the command be disposed at EndInvoke time?</param>
        /// <param name="closeConnection">Should this connection be closed at EndInvoke time?</param>
        /// <param name="startTime">Time operation was invoked.</param>
        public DaabAsyncResult(
            IAsyncResult innerAsyncResult,
            DbCommand command,
            bool disposeCommand,
            bool closeConnection,
            DateTime startTime)
        {
            this.innerAsyncResult = innerAsyncResult;
            this.command = command;
            this.disposeCommand = disposeCommand;
            this.closeConnection = closeConnection;
            this.startTime = startTime;
        }

        /// <summary>
        /// The state object passed to the callback.
        /// </summary>
        public object AsyncState
        {
            get { return innerAsyncResult.AsyncState; }
        }

        /// <summary>
        /// Wait handle to use to wait synchronously for completion.
        /// </summary>
        public WaitHandle AsyncWaitHandle
        {
            get { return innerAsyncResult.AsyncWaitHandle; }
        }

        /// <summary>
        /// True if begin operation completed synchronously.
        /// </summary>
        public bool CompletedSynchronously
        {
            get { return innerAsyncResult.CompletedSynchronously; }
        }

        /// <summary>
        /// Has the operation finished?
        /// </summary>
        public bool IsCompleted
        {
            get { return innerAsyncResult.IsCompleted; }
        }

        /// <summary>
        /// The underlying <see cref="IAsyncResult"/> object.
        /// </summary>
        public IAsyncResult InnerAsyncResult
        {
            get { return innerAsyncResult; }
        }

        /// <summary>
        /// Should the command be disposed by the End method?
        /// </summary>
        public bool DisposeCommand
        {
            get { return disposeCommand; }
        }

        /// <summary>
        /// The command that was executed.
        /// </summary>
        public DbCommand Command
        {
            get { return command; }
        }

        /// <summary>
        /// Should the connection be closed by the End method?
        /// </summary>
        public bool CloseConnection
        {
            get { return closeConnection; }
        }

        /// <summary>
        /// Connection the operation was invoked on.
        /// </summary>
        public DbConnection Connection
        {
            get { return command.Connection; }
        }

        /// <summary>
        /// Time the operation was started.
        /// </summary>
        public DateTime StartTime
        {
            get { return startTime; }
        }
    }
}
