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
using Microsoft.Practices.EnterpriseLibrary.Data.Oracle.Tests.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Data.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Oracle.Tests
{
    /// <summary>
    /// Tests executing a batch of commands with insert, delete and update 
    /// using ExecuteUpdateDataTable
    /// </summary>
    [TestClass]
    public class OracleUpdateDataSetFixture : UpdateDataSetFixture
    {
        [TestInitialize]
        public void TestInitialize()
        {
            EnvironmentHelper.AssertOracleClientIsInstalled();
            DatabaseProviderFactory factory = new DatabaseProviderFactory(OracleTestConfigurationSource.CreateConfigurationSource());
            db = factory.Create("OracleTest");
            try
            {
                DeleteStoredProcedures();
            }
            catch { }
            CreateStoredProcedures();
            base.SetUp();
        }

        [TestCleanup]
        public void OneTimeTearDown()
        {
            base.TearDown();
            DeleteStoredProcedures();
        }

        [TestMethod]
        public void OracleModifyRowWithStoredProcedure()
        {
            base.ModifyRowWithStoredProcedure();
        }

        [TestMethod]
        public void OracleDeleteRowWithStoredProcedure()
        {
            base.DeleteRowWithStoredProcedure();
        }

        [TestMethod]
        public void OracleInsertRowWithStoredProcedure()
        {
            base.InsertRowWithStoredProcedure();
        }

        [TestMethod]
        public void OracleDeleteRowWithMissingInsertAndUpdateCommands()
        {
            base.DeleteRowWithMissingInsertAndUpdateCommands();
        }

        [TestMethod]
        public void OracleUpdateRowWithMissingInsertAndDeleteCommands()
        {
            base.UpdateRowWithMissingInsertAndDeleteCommands();
        }

        [TestMethod]
        public void OracleInsertRowWithMissingUpdateAndDeleteCommands()
        {
            base.InsertRowWithMissingUpdateAndDeleteCommands();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void OracleUpdateDataSetWithAllCommandsMissing()
        {
            base.UpdateDataSetWithAllCommandsMissing();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void OracleUpdateDataSetWithNullTable()
        {
            base.UpdateDataSetWithNullTable();
        }

        protected override void CreateDataAdapterCommands()
        {
            OracleDataSetHelper.CreateDataAdapterCommands(db, ref insertCommand, ref updateCommand, ref deleteCommand);
        }

        protected override void CreateStoredProcedures()
        {
            OracleDataSetHelper.CreateStoredProcedures(db);
        }

        protected override void DeleteStoredProcedures()
        {
            OracleDataSetHelper.DeleteStoredProcedures(db);
        }

        protected override void AddTestData()
        {
            OracleDataSetHelper.AddTestData(db);
        }
    }
}
