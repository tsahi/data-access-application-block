/*
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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Data.BVT.DatabaseFixtures
{
    [TestClass]
    public class ConnectionStringsValidConfigurationFixture : EntLibFixtureBase
    {
        public ConnectionStringsValidConfigurationFixture()
            : base("ConfigFiles.ConnectionStringsOnly.config")
        {
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ExceptionIsThrownWhenDefaultDatabaseIsResolved()
        {
            new DatabaseProviderFactory(base.ConfigurationSource).CreateDefault();
        }

        [TestMethod]
        public void CorrectDatabaseTypeIsReturnedWhenNamedDatabaseIsResolved()
        {
            var database = new DatabaseProviderFactory(base.ConfigurationSource).Create("DefaultSql");

            Assert.IsInstanceOfType(database, typeof(SqlDatabase));
        }
    }
}

