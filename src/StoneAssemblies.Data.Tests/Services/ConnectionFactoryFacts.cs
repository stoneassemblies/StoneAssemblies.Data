// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConnectionFactoryFacts.cs" company="Stone Assemblies">
// Copyright © 2021 - 2021 Stone Assemblies. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace StoneAssemblies.Data.Tests.Services
{
    using System;

    using Microsoft.Data.SqlClient;

    using MySqlConnector;

    using Npgsql;

    using NUnit.Framework;

    using Oracle.ManagedDataAccess.Client;

    using StoneAssemblies.Data.Services;

    /// <summary>
    ///     The connection factory facts.
    /// </summary>
    public class ConnectionFactoryFacts
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
                Assert.IsNotNull(new ConnectionFactory());
            }
        }

        /// <summary>
        ///     The create method.
        /// </summary>
        [TestFixture]
        public class The_Create_Method
        {
            /// <summary>
            ///     Creates a mysql connection.
            /// </summary>
            [Test]
            public void Creates_A_MySqlConnection()
            {
                var connection = new ConnectionFactory().Create<MySqlConnection>(
                    "Server=127.0.0.1;Port=5432;Database=myDataBase;User Id=myUsername;Password=myPassword;");
                Assert.IsInstanceOf<MySqlConnection>(connection);
            }

            /// <summary>
            ///     Creates a npgsql connection.
            /// </summary>
            [Test]
            public void Creates_A_NpgsqlConnection()
            {
                var connection = new ConnectionFactory().Create<NpgsqlConnection>(
                    "Server=127.0.0.1;Port=5432;Database=myDataBase;User Id=myUsername;Password=myPassword;");
                Assert.IsInstanceOf<NpgsqlConnection>(connection);
            }

            /// <summary>
            ///     Creates a SQL connection.
            /// </summary>
            [Test]
            public void Creates_A_SqlConnection()
            {
                var connection = new ConnectionFactory().Create<SqlConnection>("Data Source=localhost");
                Assert.IsInstanceOf<SqlConnection>(connection);
            }

            /// <summary>
            ///     Creates a Oracle connection.
            /// </summary>
            [Test]
            public void Creates_An_OracleConnection()
            {
                var connection = new ConnectionFactory().Create<OracleConnection>(
                    "User Id=myUser;Password=myPassword;Data Source=MyOracleConnection");
                Assert.IsInstanceOf<OracleConnection>(connection);
            }

            /// <summary>
            ///     throws  argument exception when connection string is empty.
            /// </summary>
            [Test]
            public void Throws_ArgumentException_When_ConnectionString_Is_Empty()
            {
                Assert.Throws<ArgumentException>(() => new ConnectionFactory().Create<SqlConnection>(string.Empty));
            }

            /// <summary>
            ///     Throws argument exception when connection string is null.
            /// </summary>
            [Test]
            public void Throws_ArgumentException_When_ConnectionString_Is_Null()
            {
                Assert.Throws<ArgumentException>(() => new ConnectionFactory().Create<SqlConnection>(null));
            }
        }
    }
}