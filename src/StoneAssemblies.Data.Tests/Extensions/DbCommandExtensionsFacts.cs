// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DbCommandExtensionsFacts.cs" company="Stone Assemblies">
// Copyright © 2021 - 2021 Stone Assemblies. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace StoneAssemblies.Data.Tests.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Threading.Tasks;

    using Dasync.Collections;

    using Moq;

    using Newtonsoft.Json;

    using NUnit.Framework;

    using StoneAssemblies.Data.Extensions;
    using StoneAssemblies.Data.Extensions.Options;

    using Is = NUnit.DeepObjectCompare.Is;

    /// <summary>
    /// The database command extensions facts.
    /// </summary>
    public class DbCommandExtensionsFacts
    {
        /// <summary>
        /// The the_ all async_ method.
        /// </summary>
        [TestFixture]
        public class The_AllAsync_Method
        {
            [Test]
            [TestCaseSource(typeof(DataReaderExtensionsFacts), nameof(DataReaderExtensionsFacts.Entities))]
            public async Task Returns_All_Elements_As_Expected(object entity)
            {
                var commandMock = new Mock<IDbCommand>();

                var dataReaderMock = new Mock<IDataReader>();

                commandMock.Setup(command => command.ExecuteReader()).Returns(dataReaderMock.Object);

                var setupSequentialResult = dataReaderMock.SetupSequence(reader => reader.Read());
                var count = new Random().Next(10);
                for (var i = 0; i < count; i++)
                {
                    setupSequentialResult = setupSequentialResult.Returns(true);
                }

                setupSequentialResult.Returns(false);

                DataReaderExtensionsFacts.SetupMockFromExpectedResult(entity, dataReaderMock);

                var makeGenericMethod = typeof(DbCommandExtensions).GetMethod(nameof(DbCommandExtensions.AllAsync))
                    ?.MakeGenericMethod(entity.GetType());

                var parameters = new object[]
                                 {
                                     commandMock.Object, new DataReaderOptions
                                                         {
                                                             DefaultHandler = JsonConvert.DeserializeObject,
                                                         },
                                 };

                Assert.NotNull(makeGenericMethod);

                var invoke = (IAsyncEnumerable<object>)makeGenericMethod.Invoke(typeof(DbCommandExtensions), parameters);
                var entities = await invoke.ToListAsync();
                Assert.AreEqual(count, entities.Count);

                foreach (var resultEntity in entities)
                {
                    Assert.That(resultEntity, Is.DeepEqualTo(entity));
                }
            }
        }

        [TestFixture]
        public class The_SingleAsync_Method
        {
            [Test]
            [TestCaseSource(typeof(DataReaderExtensionsFacts), nameof(DataReaderExtensionsFacts.Entities))]
            public async Task Returns_The_Expected_Result(object entity)
            {
                var commandMock = new Mock<IDbCommand>();

                var dataReaderMock = new Mock<IDataReader>();
                commandMock.Setup(command => command.ExecuteReader()).Returns(dataReaderMock.Object);

                dataReaderMock.Setup(reader => reader.Read()).Returns(true);

                DataReaderExtensionsFacts.SetupMockFromExpectedResult(entity, dataReaderMock);

                var makeGenericMethod = typeof(DbCommandExtensions).GetMethod(nameof(DbCommandExtensions.SingleAsync))
                    ?.MakeGenericMethod(entity.GetType());

                var parameters = new object[]
                                 {
                                     commandMock.Object, new DataReaderOptions
                                                         {
                                                             DefaultHandler = JsonConvert.DeserializeObject,
                                                         },
                                 };

                Assert.NotNull(makeGenericMethod);

                var invokeTask = (Task)makeGenericMethod.Invoke(typeof(DbCommandExtensions), parameters);
                if (invokeTask != null)
                {
                    await invokeTask;
                }

                var resultEntity = invokeTask?.GetType()?.GetProperty("Result")?.GetValue(invokeTask);
                Assert.That(resultEntity, Is.DeepEqualTo(entity));
            }
        }
    }
}