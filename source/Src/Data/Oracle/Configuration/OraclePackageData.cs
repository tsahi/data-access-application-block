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

using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Oracle;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Oracle.Configuration
{
    /// <summary>
    /// <para>Represents the package information to use when calling a stored procedure for Oracle.</para>
    /// </summary>
    /// <remarks>
    /// <para>
    /// A package name can be appended to the stored procedure name of a command if the prefix of the stored procedure
    /// matches the prefix defined. This allows the caller of the stored procedure to use stored procedures
    /// in a more database independent fashion.
    /// </para>
    /// </remarks>
    [ResourceDescription(typeof(DesignResources), "OraclePackageDataDescription")]
    [ResourceDisplayName(typeof(DesignResources), "OraclePackageDataDisplayName")]
    public class OraclePackageData : NamedConfigurationElement, IOraclePackage
    {
        private const string prefixProperty = "prefix";

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="OraclePackageData"/> class.</para>
        /// </summary>
        public OraclePackageData() : base()
        {
            this.Prefix = string.Empty;
        }

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="OraclePackageData"/> class, given the prefix to search for and the name of the package.</para>
        /// </summary>
        /// <param name="name">
        /// <para>The name of the package to append to any found procedure that has the <paramref name="prefix"/>.</para>
        /// </param>
        /// <param name="prefix">
        /// <para>The prefix of the stored procedures used in this package.</para>
        /// </param>
        public OraclePackageData(string name, string prefix) : base(name)
        {
            this.Prefix = prefix;
        }

        /// <summary>
        /// <para>Gets or sets the prefix of the stored procedures that are in the package in Oracle.</para>
        /// </summary>
        /// <value>
        /// <para>The prefix of the stored procedures that are in the package in Oracle.</para>
        /// </value>
        [ConfigurationProperty(prefixProperty, IsRequired= true)]
        [ResourceDescription(typeof(DesignResources), "OraclePackageDataPrefixDescription")]
        [ResourceDisplayName(typeof(DesignResources), "OraclePackageDataPrefixDisplayName")]
        [ViewModel(CommonDesignTime.ViewModelTypeNames.CollectionEditorContainedElementProperty)]
        public string Prefix
        {
            get
            {
                return (string)this[prefixProperty];
            }
            set
            {
                this[prefixProperty] = value;
            }
        }
        /// <summary>
        /// Gets or sets the name of the element.
        /// </summary>
        /// <value>
        /// The name of the element.
        /// </value>
        /// <remarks>
        /// Overridden in order to annotate with design-time attribute.
        /// </remarks>
        [ViewModel(CommonDesignTime.ViewModelTypeNames.CollectionEditorContainedElementProperty)]
        public override string Name
        {
            get { return base.Name; }
            set { base.Name = value; }
        }
    }
}
