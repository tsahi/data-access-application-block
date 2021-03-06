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
using System.Configuration;
using System.Data.Common;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Data.OracleClient;
using System.Data.SqlClient;
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Fluent;
using Microsoft.Practices.EnterpriseLibrary.Data.Oracle.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Data.BVT
{
    [TestClass]
    public class FluentConfigurationResolveFixture : FluentConfigurationFixtureBase
    {
        [TestInitialize]
        public override void Initialize()
        {
            base.Initialize();
        }

        [TestMethod]
        public void DatabaseIsResolvedWhenSetAsDefault()
        {
            configurationStart.ForDatabaseNamed(DatabaseName)
                                .AsDefault();

            DictionaryConfigurationSource source = new DictionaryConfigurationSource();
            builder.UpdateConfigurationWithReplace(source);
            base.ConfigurationSource = source;

            var database = new DatabaseProviderFactory(base.ConfigurationSource).CreateDefault();

            Assert.IsNotNull(database);
            Assert.AreEqual(DefaultConnectionString, database.ConnectionString);
            Assert.IsInstanceOfType(database, typeof(SqlDatabase));
        }

        [TestMethod]
        public void SqlDatabaseIsResolvedWhenSetAsDefault()
        {
            configurationStart.ForDatabaseNamed(DatabaseName)
                               .AsDefault()
                               .ThatIs
                                   .ASqlDatabase()
                                   .WithConnectionString(new SqlConnectionStringBuilder() { DataSource = DataSource, InitialCatalog = InitialCatalog, IntegratedSecurity = IntegratedSecurity });
            DictionaryConfigurationSource source = new DictionaryConfigurationSource();
            builder.UpdateConfigurationWithReplace(source);
            base.ConfigurationSource = source;

            var database = new DatabaseProviderFactory(base.ConfigurationSource).CreateDefault();

            Assert.IsNotNull(database);

            var connectionBuilder = new SqlConnectionStringBuilder(database.ConnectionString);

            Assert.AreEqual(DataSource, connectionBuilder.DataSource);
            Assert.AreEqual(InitialCatalog, connectionBuilder.InitialCatalog);
            Assert.IsTrue(connectionBuilder.IntegratedSecurity);
            Assert.IsInstanceOfType(database, typeof(SqlDatabase));
        }

        [TestMethod]
        public void GenericDatabaseIsResolvedWhenSetAsDefault()
        {
            var dbConnectionBuilder = new DbConnectionStringBuilder();

            dbConnectionBuilder[UidPropertyName] = Uid;
            dbConnectionBuilder[PwdPropertyName] = Pwd;
            dbConnectionBuilder[DataSourcePropertyName] = DataSource;

            configurationStart.ForDatabaseNamed(DatabaseName)
                                .AsDefault()
                                .ThatIs
                                    .AnotherDatabaseType(DbProviderMapping.DefaultSqlProviderName)
                                    .WithConnectionString(dbConnectionBuilder);

            DictionaryConfigurationSource source = new DictionaryConfigurationSource();
            builder.UpdateConfigurationWithReplace(source);
            base.ConfigurationSource = source;

            var database = new DatabaseProviderFactory(base.ConfigurationSource).CreateDefault();

            Assert.AreEqual(GenericConnectionString, database.ConnectionString);
            Assert.IsInstanceOfType(database, typeof(SqlDatabase));
        }

        [TestMethod]
        public void MappedDatabaseIsResolvedWhenSetAsDefault()
        {
            configurationStart.WithProviderNamed(DbProviderMapping.DefaultSqlProviderName)
                                .MappedToDatabase<SqlDatabase>()
                                .ForDatabaseNamed(DatabaseName)
                                .AsDefault()
                                .ThatIs
                                    .AnotherDatabaseType(DbProviderMapping.DefaultSqlProviderName)
                                    .WithConnectionString(GenericConnectionString);
            DictionaryConfigurationSource source = new DictionaryConfigurationSource();
            builder.UpdateConfigurationWithReplace(source);
            base.ConfigurationSource = source;
            //         ConfigureContainer();

            var database = new DatabaseProviderFactory(base.ConfigurationSource).CreateDefault();

            Assert.IsNotNull(database);
            Assert.IsInstanceOfType(database, typeof(SqlDatabase));
        }
    }
}

