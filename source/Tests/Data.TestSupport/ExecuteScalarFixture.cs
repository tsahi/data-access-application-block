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
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Data.TestSupport
{
    public class ExecuteScalarFixture
    {
        DbCommand command;
        Database db;

        public ExecuteScalarFixture(Database db,
                                    DbCommand command)
        {
            this.db = db;
            this.command = command;
        }

        public void CanExecuteScalarDoAnInsertion()
        {
            string insertCommand = "Insert into Region values (99, 'Midwest')";
            DbCommand command = db.GetSqlStringCommand(insertCommand);
            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                using (RollbackTransactionWrapper transaction = new RollbackTransactionWrapper(connection.BeginTransaction()))
                {
                    db.ExecuteScalar(command, transaction.Transaction);

                    DbCommand rowCountCommand = db.GetSqlStringCommand("select count(*) from Region");
                    int count = Convert.ToInt32(db.ExecuteScalar(rowCountCommand, transaction.Transaction));
                    Assert.AreEqual(5, count);
                }
            }
        }

        public void ExecuteScalarWithCommandTextAndTypeInTransaction()
        {
            int count = -1;
            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                using (DbTransaction transaction = connection.BeginTransaction())
                {
                    count = Convert.ToInt32(db.ExecuteScalar(transaction, command.CommandType, command.CommandText));
                    transaction.Commit();
                }
            }

            Assert.AreEqual(4, count);
        }

        public void ExecuteScalarWithIDbCommand()
        {
            int count = Convert.ToInt32(db.ExecuteScalar(command));

            Assert.AreEqual(4, count);
        }

        public void ExecuteScalarWithIDbTransaction()
        {
            int count = -1;
            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                using (DbTransaction transaction = connection.BeginTransaction())
                {
                    count = Convert.ToInt32(db.ExecuteScalar(command, transaction));
                    transaction.Commit();
                }
            }

            Assert.AreEqual(4, count);
        }

        public void ExecuteScalarWithNullIDbCommand()
        {
            int count = Convert.ToInt32(db.ExecuteScalar((DbCommand)null));
        }

        public void ExecuteScalarWithNullIDbCommandAndNullTransaction()
        {
            int count = Convert.ToInt32(db.ExecuteScalar(null, (string)null));
        }

        public void ExecuteScalarWithNullIDbTransaction()
        {
            int count = Convert.ToInt32(db.ExecuteScalar(command, null));
        }
    }
}
