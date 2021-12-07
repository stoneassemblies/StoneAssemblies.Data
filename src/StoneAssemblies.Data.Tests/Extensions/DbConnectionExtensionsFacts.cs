// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DbConnectionExtensionsFacts.cs" company="Stone Assemblies">
// Copyright © 2021 - 2021 Stone Assemblies. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace StoneAssemblies.Data.Tests.Extensions
{
    using System;
    using System.Data;
    using System.Threading.Tasks;

    using Microsoft.Data.SqlClient;

    using Moq;

    using NUnit.Framework;

    using StoneAssemblies.Data.Extensions;

    /// <summary>
    /// The database connection extensions facts.
    /// </summary>
    public class DbConnectionExtensionsFacts
    {
        /// <summary>
        /// The dispose async method.
        /// </summary>
        [TestFixture]
        public class The_DisposeAsync_Method
        {
            /// <summary>
            /// Calls dispose async.
            /// </summary>
            /// <returns>
            /// The <see cref="Task"/>.
            /// </returns>
            [Test]
            public async Task Calls_Dispose_Once_Async()
            {
                var connectionMock = new Mock<IDbConnection>();
                await connectionMock.Object.DisposeAsync();

                connectionMock.Verify(connection => connection.Dispose(), Times.Once);
            }

            /// <summary>
            ///     Calls dispose async once async.
            /// </summary>
            /// <returns>
            ///     The <see cref="Task" />.
            /// </returns>
            [Test]
            public async Task Calls_DisposeAsync_Once_Async()
            {
                var connectionMock = new Mock<IDbConnection>();
                var disposableMock = connectionMock.As<IAsyncDisposable>();
                await connectionMock.Object.DisposeAsync();

                disposableMock.Verify(disposable => disposable.DisposeAsync(), Times.Once);
            }
        }

        /// <summary>
        /// The open async method.
        /// </summary>
        [TestFixture]
        public class The_OpenAsync_Method
        {
            /// <summary>
            ///     Calls open async.
            /// </summary>
            /// <returns>
            ///     The <see cref="Task" />.
            /// </returns>
            [Test]
            public async Task Calls_Open_Async()
            {
                var connectionMock = new Mock<IDbConnection>();
                await connectionMock.Object.OpenAsync();

                connectionMock.Verify(connection => connection.Open(), Times.Once);
            }

            /// <summary>
            ///     Throws invalid operation exception because the underlying connection is closed.
            /// </summary>
            /// <returns>
            ///     The <see cref="Task" />.
            /// </returns>
            [Test]
            public async Task Throws_InvalidOperationException_Because_The_UnderlyingConnection_Is_Closed()
            {
                var connection = new SqlConnection() as IDbConnection;

                Assert.ThrowsAsync<InvalidOperationException>(() => connection.OpenAsync());
            }
        }
    }
}