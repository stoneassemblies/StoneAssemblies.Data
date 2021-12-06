// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DbCommandExtensionsFacts.cs" company="Stone Assemblies">
// Copyright © 2021 - 2021 Stone Assemblies. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace StoneAssemblies.Data.Tests.Extensions
{
    using System.Data;

    using Moq;

    using NUnit.Framework;

    using StoneAssemblies.Data.Extensions;

    public class DbCommandExtensionsFacts
    {
        [TestFixture]
        public class The_AddParameterWithValue_Method
        {
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
    }
}