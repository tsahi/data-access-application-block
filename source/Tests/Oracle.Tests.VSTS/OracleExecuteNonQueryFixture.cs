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
using Microsoft.Practices.EnterpriseLibrary.Data.Oracle.Tests.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Data.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Oracle.Tests
{
    [TestClass]
    public class OracleExecuteNonQueryFixture
    {
        ExecuteNonQueryFixture baseFixture;
        Database db;
        const string insertString = "insert into Region values (77, 'Elbonia')";
        const string countQuery = "select count(*) from Region";

        [TestInitialize]
        public void SetUp()
        {
            EnvironmentHelper.AssertOracleClientIsInstalled();
            DatabaseProviderFactory factory = new DatabaseProviderFactory(OracleTestConfigurationSource.CreateConfigurationSource());
            db = factory.Create("OracleTest");

            DbCommand insertionCommand = db.GetSqlStringCommand(insertString);
            DbCommand countCommand = db.GetSqlStringCommand(countQuery);

            baseFixture = new ExecuteNonQueryFixture(db, insertString, countQuery, insertionCommand, countCommand);
        }

        [TestMethod]
        public void CanExecuteNonQueryWithCommandTextWithDefinedTypeAndTransaction()
        {
            baseFixture.CanExecuteNonQueryWithCommandTextWithDefinedTypeAndTransaction();
        }

        [TestMethod]
        public void CanExecuteNonQueryWithDbCommand()
        {
            baseFixture.CanExecuteNonQueryWithDbCommand();
        }

        [TestMethod]
        public void CanExecuteNonQueryThroughTransaction()
        {
            baseFixture.CanExecuteNonQueryThroughTransaction();
        }

        [TestMethod]
        public void TransactionActuallyRollsBack()
        {
            baseFixture.TransactionActuallyRollsBack();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExecuteNonQueryWithNullDbTransaction()
        {
            baseFixture.ExecuteNonQueryWithNullDbTransaction();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ExecuteNonQueryWithNullDbCommandAndTransaction()
        {
            baseFixture.ExecuteNonQueryWithNullDbCommandAndTransaction();
        }
    }
}
