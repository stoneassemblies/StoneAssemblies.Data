// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataReaderExtensionsFacts.cs" company="Stone Assemblies">
// Copyright © 2021 - 2021 Stone Assemblies. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace StoneAssemblies.Data.Tests.Extensions
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Threading.Tasks;

    using Dasync.Collections;

    using Moq;

    using Newtonsoft.Json;

    using NUnit.Framework;

    using StoneAssemblies.Data.Extensions;
    using StoneAssemblies.Data.Extensions.Options;

    using DataReaderExtensions = StoneAssemblies.Data.Extensions.DataReaderExtensions;
    using Is = NUnit.DeepObjectCompare.Is;

    /// <summary>
    ///     The data reader extensions facts.
    /// </summary>
    public class DataReaderExtensionsFacts
    {
        public static IEnumerable Entities()
        {
            var random = new Random();
            yield return new Person
                         {
                             Id = Guid.NewGuid(),
                             FirstName = Guid.NewGuid().ToString(),
                             LastName = Guid.NewGuid().ToString(),
                             Age = random.Next(100),
                             BirthDay = DateTime.Now,
                             Relatives = new List<Person>
                                         {
                                             new Person
                                             {
                                                 Id = Guid.NewGuid(),
                                                 FirstName = Guid.NewGuid().ToString(),
                                                 LastName = Guid.NewGuid().ToString(),
                                                 Age = random.Next(100),
                                                 BirthDay = DateTime.Now,
                                             },
                                             new Person
                                             {
                                                 Id = Guid.NewGuid(),
                                                 FirstName = Guid.NewGuid().ToString(),
                                                 LastName = Guid.NewGuid().ToString(),
                                                 Age = random.Next(100),
                                                 BirthDay = DateTime.Now,
                                             },
                                             new Person
                                             {
                                                 Id = Guid.NewGuid(),
                                                 FirstName = Guid.NewGuid().ToString(),
                                                 LastName = Guid.NewGuid().ToString(),
                                                 Age = random.Next(100),
                                                 BirthDay = DateTime.Now,
                                             },
                                         },
                         };
            yield return new Person
                         {
                             Id = Guid.NewGuid(),
                             FirstName = Guid.NewGuid().ToString(),
                             LastName = null,
                             Age = random.Next(100),
                             BirthDay = DateTime.Now,
                             Relatives = null,
                         };
            yield return new Person
                         {
                             FirstName = Guid.NewGuid().ToString(),
                             LastName = Guid.NewGuid().ToString(),
                             Age = random.Next(100),
                             BirthDay = DateTime.Now,
                             Relatives = new List<Person>
                                         {
                                             new Person
                                             {
                                                 FirstName = Guid.NewGuid().ToString(),
                                                 LastName = Guid.NewGuid().ToString(),
                                                 Age = random.Next(100),
                                                 BirthDay = DateTime.Now,
                                             },
                                             new Person
                                             {
                                                 FirstName = Guid.NewGuid().ToString(),
                                                 LastName = Guid.NewGuid().ToString(),
                                                 Age = random.Next(100),
                                                 BirthDay = DateTime.Now,
                                             },
                                             new Person
                                             {
                                                 FirstName = Guid.NewGuid().ToString(),
                                                 LastName = Guid.NewGuid().ToString(),
                                                 Age = random.Next(100),
                                                 BirthDay = DateTime.Now,
                                             },
                                         },
                         };
        }

        /// <summary>
        ///     Setups a mock from expected result.
        /// </summary>
        /// <param name="entity">
        ///     The entity.
        /// </param>
        /// <param name="dataReaderMock">
        ///     The data reader mock.
        /// </param>
        private static void SetupMockFromExpectedResult(object entity, Mock<IDataReader> dataReaderMock)
        {
            var propertyInfos = entity.GetType().GetProperties();
            dataReaderMock.Setup(reader => reader.FieldCount).Returns(propertyInfos.Length);
            for (var index = 0; index < propertyInfos.Length; index++)
            {
                var idx = index;
                var propertyInfo = propertyInfos[idx];
                dataReaderMock.Setup(reader => reader.GetName(idx)).Returns(propertyInfo.Name);
                var value = propertyInfo.GetValue(entity);

                if (value == null)
                {
                    dataReaderMock.Setup(reader => reader.IsDBNull(idx)).Returns(true);
                }
                else
                {
                    dataReaderMock.Setup(reader => reader.IsDBNull(idx)).Returns(false);

                    if (propertyInfo.IsReadableFromDatabase())
                    {
                        dataReaderMock.Setup(reader => reader.GetValue(idx)).Returns(value);
                    }
                    else
                    {
                        dataReaderMock.Setup(reader => reader.GetString(idx)).Returns(JsonConvert.SerializeObject(value));
                    }
                }
            }
        }

        /// <summary>
        ///     Setups mock from expected result but throws on get value.
        /// </summary>
        /// <param name="entity">
        ///     The entity.
        /// </param>
        /// <param name="dataReaderMock">
        ///     The data reader mock.
        /// </param>
        private static void SetupMockFromExpectedResultButThrowsOnGetValue(object entity, Mock<IDataReader> dataReaderMock)
        {
            var propertyInfos = entity.GetType().GetProperties();
            dataReaderMock.Setup(reader => reader.FieldCount).Returns(propertyInfos.Length);
            for (var index = 0; index < propertyInfos.Length; index++)
            {
                var idx = index;
                var propertyInfo = propertyInfos[idx];
                dataReaderMock.Setup(reader => reader.GetName(idx)).Returns(propertyInfo.Name);
                var value = propertyInfo.GetValue(entity);

                if (value == null)
                {
                    dataReaderMock.Setup(reader => reader.IsDBNull(idx)).Returns(true);
                }
                else
                {
                    dataReaderMock.Setup(reader => reader.IsDBNull(idx)).Returns(false);

                    if (propertyInfo.IsReadableFromDatabase())
                    {
                        dataReaderMock.Setup(reader => reader.GetValue(idx)).Throws(new Exception());
                    }
                    else
                    {
                        dataReaderMock.Setup(reader => reader.GetString(idx)).Throws(new Exception());
                    }
                }
            }
        }

        /// <summary>
        ///     The person.
        /// </summary>
        public class Person
        {
            /// <summary>
            ///     Gets or sets the age.
            /// </summary>
            public int Age { get; set; }

            /// <summary>
            ///     Gets or sets the birth day.
            /// </summary>
            public DateTime BirthDay { get; set; }

            /// <summary>
            ///     Gets or sets the first name.
            /// </summary>
            public string FirstName { get; set; }

            /// <summary>
            ///     Gets or sets the id.
            /// </summary>
            public Guid Id { get; set; }

            /// <summary>
            ///     Gets or sets the last name.
            /// </summary>
            public string LastName { get; set; }

            /// <summary>
            ///     Gets or sets the relatives.
            /// </summary>
            public List<Person> Relatives { get; set; }
        }

        /// <summary>
        /// The all async method.
        /// </summary>
        [TestFixture]
        public class The_AllAsync_Method
        {
            /// <summary>
            /// Returns all elements as expected.
            /// </summary>
            /// <param name="entity">
            /// The entity.
            /// </param>
            /// <returns>
            /// The <see cref="Task"/>.
            /// </returns>
            [Test]
            [TestCaseSource(typeof(DataReaderExtensionsFacts), nameof(Entities))]
            public async Task Returns_All_Elements_As_Expected(object entity)
            {
                var dataReaderMock = new Mock<IDataReader>();

                var setupSequentialResult = dataReaderMock.SetupSequence(reader => reader.Read());
                var count = new Random().Next(10);
                for (var i = 0; i < count; i++)
                {
                    setupSequentialResult = setupSequentialResult.Returns(true);
                }

                setupSequentialResult.Returns(false);

                SetupMockFromExpectedResult(entity, dataReaderMock);

                var makeGenericMethod = typeof(DataReaderExtensions).GetMethod(nameof(DataReaderExtensions.AllAsync))
                    ?.MakeGenericMethod(entity.GetType());

                var parameters = new object[]
                                 {
                                     dataReaderMock.Object, new DataReaderOptions
                                                            {
                                                                DefaultHandler = JsonConvert.DeserializeObject,
                                                            },
                                 };

                Assert.NotNull(makeGenericMethod);

                var invoke = (IAsyncEnumerable<object>)makeGenericMethod.Invoke(typeof(DataReaderExtensions), parameters);

                var entities = await invoke.ToListAsync();

                Assert.AreEqual(count, entities.Count);

                foreach (var resultEntity in entities)
                {
                    Assert.That(resultEntity, Is.DeepEqualTo(entity));
                }
            }
        }

        /// <summary>
        /// The the_ single async_ method.
        /// </summary>
        [TestFixture]
        public class The_SingleAsync_Method
        {
            /// <summary>
            /// Returns the expected result.
            /// </summary>
            /// <param name="entity">
            /// The entity.
            /// </param>
            /// <returns>
            /// The <see cref="Task"/>.
            /// </returns>
            [Test]
            [TestCaseSource(typeof(DataReaderExtensionsFacts), nameof(Entities))]
            public async Task Returns_The_Expected_Result(object entity)
            {
                var dataReaderMock = new Mock<IDataReader>();
                dataReaderMock.Setup(reader => reader.Read()).Returns(true);

                SetupMockFromExpectedResult(entity, dataReaderMock);

                var makeGenericMethod = typeof(DataReaderExtensions).GetMethod(nameof(DataReaderExtensions.SingleAsync))
                    ?.MakeGenericMethod(entity.GetType());

                var parameters = new object[]
                                 {
                                     dataReaderMock.Object, new DataReaderOptions
                                                            {
                                                                DefaultHandler = JsonConvert.DeserializeObject,
                                                            },
                                 };

                Assert.NotNull(makeGenericMethod);

                var invokeTask = (Task)makeGenericMethod.Invoke(typeof(DataReaderExtensions), parameters);
                if (invokeTask != null)
                {
                    await invokeTask;
                }

                var resultEntity = invokeTask?.GetType()?.GetProperty("Result")?.GetValue(invokeTask);
                Assert.That(resultEntity, Is.DeepEqualTo(entity));
            }

            /// <summary>
            /// Succeeds even when read data from the database fails.
            /// </summary>
            /// <param name="entity">
            /// The entity.
            /// </param>
            /// <returns>
            /// The <see cref="Task"/>.
            /// </returns>
            [Test]
            [TestCaseSource(typeof(DataReaderExtensionsFacts), nameof(Entities))]
            public async Task Succeeds_Even_When_Read_Data_From_DB_Fails(object entity)
            {
                var dataReaderMock = new Mock<IDataReader>();

                dataReaderMock.Setup(reader => reader.Read()).Returns(true);

                SetupMockFromExpectedResultButThrowsOnGetValue(entity, dataReaderMock);

                var makeGenericMethod = typeof(DataReaderExtensions).GetMethod(nameof(DataReaderExtensions.SingleAsync))
                    ?.MakeGenericMethod(entity.GetType());

                var parameters = new object[]
                                 {
                                     dataReaderMock.Object, new DataReaderOptions
                                                            {
                                                                DefaultHandler = JsonConvert.DeserializeObject,
                                                            },
                                 };

                Assert.NotNull(makeGenericMethod);

                var invokeTask = (Task)makeGenericMethod.Invoke(typeof(DataReaderExtensions), parameters);
                if (invokeTask != null)
                {
                    await invokeTask;
                }

                var resultEntity = invokeTask?.GetType()?.GetProperty("Result")?.GetValue(invokeTask);
                Assert.NotNull(resultEntity);
            }
        }
    }
}