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
        }
    }
}