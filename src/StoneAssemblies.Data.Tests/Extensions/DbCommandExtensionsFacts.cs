// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DbCommandExtensionsFacts.cs" company="Stone Assemblies">
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
    ///     The database command extensions facts.
    /// </summary>
    public class DbCommandExtensionsFacts
    {
        /// <summary>
        ///     The  add parameter with value  method.
        /// </summary>
        [TestFixture]
        public class The_AddParameterWithValue_Method
        {
            /// <summary>
            ///     Adds the specified parameter.
            /// </summary>
            [Test]
            public void Adds_The_Specified_Parameter()
            {
                var commandMock = new Mock<IDbCommand>();
                var dataParameterMock = new Mock<IDbDataParameter>();
                commandMock.Setup(command => command.CreateParameter()).Returns(dataParameterMock.Object);
                var dataParameterCollection = new Mock<IDataParameterCollection>();
                commandMock.Setup(command => command.Parameters).Returns(dataParameterCollection.Object);

                commandMock.Object.AddParameterWithValue("@param0", "paraValue");
                dataParameterMock.VerifySet(parameter => parameter.ParameterName = "@param0");
                dataParameterMock.VerifySet(parameter => parameter.Value = "paraValue");
                dataParameterCollection.Verify(
                    collection => collection.Add(It.Is<IDbDataParameter>(parameter => parameter == dataParameterMock.Object)),
                    Times.Once);
                dataParameterCollection.Verify(collection => collection.Add(It.IsAny<IDbDataParameter>()), Times.Once);
            }
        }

        /// <summary>
        /// The execute non query async method.
        /// </summary>
        [TestFixture]
        public class The_ExecuteNonQueryAsync_Method
        {
            /// <summary>
            ///     Throws invalid operation exception because the underlying connection is closed.
            /// </summary>
            /// <returns>
            ///     The <see cref="Task" />.
            /// </returns>
            [Test]
            public async Task Throws_InvalidOperationException_Because_The_UnderlyingConnection_Is_Closed()
            {
                var command = new SqlCommand() as IDbCommand;

                Assert.ThrowsAsync<InvalidOperationException>(() => command.ExecuteNonQueryAsync());
            }

            /// <summary>
            ///     Calls the execute non query method and returns the expected value.
            /// </summary>
            /// <returns>
            ///     The <see cref="Task" />.
            /// </returns>
            [Test]
            public async Task Calls_The_ExecuteNonQuery_Method_And_Returns_The_Expected_Value()
            {
                var mock = new Mock<IDbCommand>();
                var expected = 1;
                mock.Setup(dbCommand => dbCommand.ExecuteNonQuery()).Returns(expected);
                var command = mock.Object;
                var result = await command.ExecuteNonQueryAsync();
                Assert.AreEqual(expected, result);
            }
        }

        /// <summary>
        ///     The  execute scalar async method.
        /// </summary>
        [TestFixture]
        public class The_ExecuteScalarAsync_Method
        {
            /// <summary>
            ///     Throws invalid operation exception because the underlying connection is closed.
            /// </summary>
            /// <returns>
            ///     The <see cref="Task" />.
            /// </returns>
            [Test]
            public async Task Throws_InvalidOperationException_Because_The_UnderlyingConnection_Is_Closed()
            {
                var command = new SqlCommand() as IDbCommand;

                Assert.ThrowsAsync<InvalidOperationException>(() => command.ExecuteScalarAsync());
            }

            /// <summary>
            ///     Calls execute scalar and returns the expected value.
            /// </summary>
            /// <returns>
            ///     The <see cref="Task" />.
            /// </returns>
            [Test]
            public async Task Calls_ExecuteScalar_And_Returns_The_Expected_Value()
            {
                var commandMock = new Mock<IDbCommand>();
                var expected = 1;
                commandMock.Setup(command => command.ExecuteScalar()).Returns(expected);
                var result = await commandMock.Object.ExecuteScalarAsync();
                Assert.AreEqual(expected, result);
            }

        }

        /// <summary>
        ///     The execute reader async method.
        /// </summary>
        [TestFixture]
        public class The_ExecuteReaderAsync_Method
        {
            /// <summary>
            ///     Throws invalid operation exception because the underlying connection is closed.
            /// </summary>
            /// <returns>
            ///     The <see cref="Task" />.
            /// </returns>
            [Test]
            public async Task Throws_InvalidOperationException_Because_The_UnderlyingConnection_Is_Closed()
            {
                var command = new SqlCommand() as IDbCommand;

                Assert.ThrowsAsync<InvalidOperationException>(() => command.ExecuteReaderAsync());
            }

            /// <summary>
            ///     Calls execute scalar and returns the expected value.
            /// </summary>
            /// <returns>
            ///     The <see cref="Task" />.
            /// </returns>
            [Test]
            public async Task Calls_ExecuteScalar_And_Returns_The_Expected_Value()
            {
                var commandMock = new Mock<IDbCommand>();
                var dataReaderMock = new Mock<IDataReader>();
                var expected = dataReaderMock.Object;
                commandMock.Setup(command => command.ExecuteReader()).Returns(expected);
                var result = await commandMock.Object.ExecuteReaderAsync();
                Assert.AreEqual(expected, result);
            }
        }

        /// <summary>
        ///     The  execute reader async generic method.
        /// </summary>
        [TestFixture]
        public class The_ExecuteScalarAsync_Generic_Method
        {
            /// <summary>
            ///     Throws invalid operation exception because the underlying connection is closed.
            /// </summary>
            /// <returns>
            ///     The <see cref="Task" />.
            /// </returns>
            [Test]
            public async Task Throws_InvalidOperationException_Because_The_UnderlyingConnection_Is_Closed()
            {
                var command = new SqlCommand() as IDbCommand;

                Assert.ThrowsAsync<InvalidOperationException>(() => command.ExecuteScalarAsync<int>());
            }

            /// <summary>
            ///     Calls execute scalar and returns the expected value.
            /// </summary>
            /// <returns>
            ///     The <see cref="Task" />.
            /// </returns>
            [Test]
            public async Task Calls_ExecuteScalar_And_Returns_The_Expected_Value()
            {
                var commandMock = new Mock<IDbCommand>();
                var expected = 1;
                commandMock.Setup(command => command.ExecuteScalar()).Returns(expected);
                var result = await commandMock.Object.ExecuteScalarAsync<int>();
                Assert.AreEqual(expected, result);
            }
        }
    }
}