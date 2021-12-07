// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SqlConnectionFactoryFacts.cs" company="Stone Assemblies">
// Copyright © 2021 - 2021 Stone Assemblies. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace StoneAssemblies.Data.Tests.SqlClient.Services
{
    using System;

    using Microsoft.Data.SqlClient;

    using NUnit.Framework;

    using StoneAssemblies.Data.SqlClient.Services;

    /// <summary>
    ///     The sql connection factory facts.
    /// </summary>
    public class SqlConnectionFactoryFacts
    {
        /// <summary>
        ///     The the_ constructor_ method.
        /// </summary>
        [TestFixture]
        public class The_Constructor
        {
            /// <summary>
            ///     Creates an instance of sql connection factory.
            /// </summary>
            [Test]
            public void Creates_An_Instance_Of_SqlConnectionFactory()
            {
                Assert.IsNotNull(new SqlConnectionFactory());
            }
        }

        /// <summary>
        ///     The create method.
        /// </summary>
        [TestFixture]
        public class The_Create_Method
        {
            /// <summary>
            ///     Creates an SQL connection.
            /// </summary>
            [Test]
            public void Creates_An_SqlConnection()
            {
                var connection = new SqlConnectionFactory().Create("Data Source=localhost");
                Assert.IsInstanceOf<SqlConnection>(connection);
            }

            /// <summary>
            ///     throws  argument exception when connection string is empty.
            /// </summary>
            [Test]
            public void Throws_ArgumentException_When_ConnectionString_Is_Empty()
            {
                Assert.Throws<ArgumentException>(() => new SqlConnectionFactory().Create(string.Empty));
            }

            /// <summary>
            ///     Throws argument exception when connection string is null.
            /// </summary>
            [Test]
            public void Throws_ArgumentException_When_ConnectionString_Is_Null()
            {
                Assert.Throws<ArgumentException>(() => new SqlConnectionFactory().Create(null));
            }
        }
    }
}