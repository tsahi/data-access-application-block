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

using System.Data;
using System.Data.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Data.TestSupport
{
    public class ExecuteReaderFixture
    {
        Database db;
        DbCommand insertCommand;
        string insertString;
        DbCommand queryCommand;
        string queryString;

        public ExecuteReaderFixture(Database db,
                                    string insertString,
                                    DbCommand insertCommand,
                                    string queryString,
                                    DbCommand queryCommand)
        {
            this.db = db;
            this.insertString = insertString;
            this.queryString = queryString;
            this.insertCommand = insertCommand;
            this.queryCommand = queryCommand;
        }

        public void CanExecuteQueryThroughDataReaderUsingTransaction()
        {
            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                using (RollbackTransactionWrapper transaction = new RollbackTransactionWrapper(connection.BeginTransaction()))
                {
                    using (IDataReader reader = db.ExecuteReader(insertCommand, transaction.Transaction))
                    {
                        Assert.AreEqual(1, reader.RecordsAffected);
                    }
                }

                Assert.AreEqual(ConnectionState.Open, connection.State);
            }
        }

        public void CanExecuteReaderFromDbCommand()
        {
            IDataReader reader = db.ExecuteReader(queryCommand);
            DbConnection connection = queryCommand.Connection;
            string accumulator = "";
            while (reader.Read())
            {
                accumulator += ((string)reader["RegionDescription"]).Trim();
            }
            reader.Close();

            Assert.AreEqual("EasternWesternNorthernSouthern", accumulator);
            Assert.AreEqual(ConnectionState.Closed, connection.State);
        }

        public void CanExecuteReaderWithCommandText()
        {
            IDataReader reader = db.ExecuteReader(CommandType.Text, queryString);
            string accumulator = "";
            while (reader.Read())
            {
                accumulator += ((string)reader["RegionDescription"]).Trim();
            }
            reader.Close();

            Assert.AreEqual("EasternWesternNorthernSouthern", accumulator);
        }

        public void EmptyQueryStringTest()
        {
            using (DbCommand myCommand = db.GetSqlStringCommand(string.Empty))
            {
                IDataReader reader = db.ExecuteReader(myCommand);
            }
        }

        public void ExecuteQueryThroughDataReaderUsingNullCommandAndNullTransactionThrows()
        {
            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                using (IDataReader reader = db.ExecuteReader(null, (string)null)) { }
            }
        }

        public void ExecuteQueryThroughDataReaderUsingNullCommandThrows()
        {
            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                insertCommand = null;
                using (IDataReader reader = db.ExecuteReader(insertCommand, null)) { }
            }
        }

        public void ExecuteQueryThroughDataReaderUsingNullTransactionThrows()
        {
            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                using (IDataReader reader = db.ExecuteReader(insertCommand, null)) { }
            }
        }

        public void ExecuteReaderWithNullCommand()
        {
            using (IDataReader reader = db.ExecuteReader((DbCommand)null)) { }
            Assert.AreEqual(null, insertCommand);
        }

        public void NullQueryStringTest()
        {
            using (DbCommand myCommand = db.GetSqlStringCommand(null))
            {
                IDataReader reader = db.ExecuteReader(myCommand);
            }
        }

        public void WhatGetsReturnedWhenWeDoAnInsertThroughDbCommandExecute()
        {
            int count = -1;
            IDataReader reader = null;
            try
            {
                reader = db.ExecuteReader(insertCommand);
                count = reader.RecordsAffected;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }

                string deleteString = "Delete from Region where RegionId = 99";
                DbCommand cleanupCommand = db.GetSqlStringCommand(deleteString);
                db.ExecuteNonQuery(cleanupCommand);
            }

            Assert.AreEqual(1, count);
        }
    }
}
