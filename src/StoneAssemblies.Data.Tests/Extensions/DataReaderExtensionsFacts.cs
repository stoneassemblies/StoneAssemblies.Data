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
    using System.Linq;
    using System.Threading.Tasks;

    using Dasync.Collections;

    using Moq;

    using Newtonsoft.Json;

    using NUnit.Framework;

    using StoneAssemblies.Data.Extensions;
    using StoneAssemblies.Data.Extensions.Interfaces;
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
                AnnualSalary = random.Next(1, 100),
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
                                                 AnnualSalary = random.Next(1, 100),
                                             },
                                             new Person
                                             {
                                                 Id = Guid.NewGuid(),
                                                 FirstName = Guid.NewGuid().ToString(),
                                                 LastName = Guid.NewGuid().ToString(),
                                                 Age = random.Next(100),
                                                 BirthDay = DateTime.Now,
                                                 AnnualSalary = random.Next(1, 100),
                                             },
                                             new Person
                                             {
                                                 Id = Guid.NewGuid(),
                                                 FirstName = Guid.NewGuid().ToString(),
                                                 LastName = Guid.NewGuid().ToString(),
                                                 Age = random.Next(100),
                                                 BirthDay = DateTime.Now,
                                                 AnnualSalary = random.Next(1, 100),
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
                AnnualSalary = random.Next(1, 100),
            };
            yield return new Person
            {
                FirstName = Guid.NewGuid().ToString(),
                LastName = Guid.NewGuid().ToString(),
                Age = random.Next(100),
                BirthDay = DateTime.Now,
                AnnualSalary = random.Next(1, 100),
                Relatives = new List<Person>
                                         {
                                             new Person
                                             {
                                                 FirstName = Guid.NewGuid().ToString(),
                                                 LastName = Guid.NewGuid().ToString(),
                                                 Age = random.Next(100),
                                                 BirthDay = DateTime.Now,
                                                 AnnualSalary = random.Next(1, 100),
                                             },
                                             new Person
                                             {
                                                 FirstName = Guid.NewGuid().ToString(),
                                                 LastName = Guid.NewGuid().ToString(),
                                                 Age = random.Next(100),
                                                 BirthDay = DateTime.Now,
                                                 AnnualSalary = random.Next(1, 100),
                                             },
                                             new Person
                                             {
                                                 FirstName = Guid.NewGuid().ToString(),
                                                 LastName = Guid.NewGuid().ToString(),
                                                 Age = random.Next(100),
                                                 BirthDay = DateTime.Now,
                                                 AnnualSalary = random.Next(1, 100),
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
                        dataReaderMock.Setup(reader => reader.GetValue(idx)).Returns(JsonConvert.SerializeObject(value));
                    }
                }
            }
        }

        /// <summary>
        ///     Setup mock from expected result but wrong types.
        /// </summary>
        /// <param name="entity">
        ///     The entity.
        /// </param>
        /// <param name="dataReaderMock">
        ///     The data reader mock.
        /// </param>
        private static void SetupMockFromExpectedResultButWrongTypes(object entity, Mock<IDataReader> dataReaderMock)
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
                        if (value is string)
                        {
                            dataReaderMock.Setup(reader => reader.GetValue(idx)).Returns(1);
                        }
                        else
                        {
                            dataReaderMock.Setup(reader => reader.GetValue(idx)).Returns(value);
                        }
                    }
                    else
                    {
                        dataReaderMock.Setup(reader => reader.GetValue(idx)).Returns(JsonConvert.SerializeObject(value));
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
            /// Gets or sets the annual salary.
            /// </summary>
            public decimal AnnualSalary { get; set; }

            /// <summary>
            ///     Gets or sets the relatives.
            /// </summary>
            public List<Person> Relatives { get; set; }
        }

        /// <summary>
        /// The all async method.
        /// </summary>
        [TestFixture]
        public class The_GetAllAsync_Method
        {
            /// <summary>
            /// The returns_ the_ entity_ from_ the_ projection.
            /// </summary>
            /// <returns>
            /// The <see cref="Task"/>.
            /// </returns>
            [Test]
            public async Task Returns_All_Entities_From_The_Projection()
            {
                var dataReaderMock = new Mock<IDataReader>();
                var setupSequentialResult = dataReaderMock.SetupSequence(reader => reader.Read());
                var count = new Random().Next(10);
                for (var i = 0; i < count; i++)
                {
                    setupSequentialResult = setupSequentialResult.Returns(true);
                }

                setupSequentialResult.Returns(false);

                var expectedPerson = new Person
                {
                    Id = Guid.NewGuid(),
                    FirstName = Guid.NewGuid().ToString(),
                    LastName = Guid.NewGuid().ToString(),
                };

                dataReaderMock.Setup(reader => reader.GetGuid(0)).Returns(expectedPerson.Id);
                dataReaderMock.Setup(reader => reader.GetString(1)).Returns(expectedPerson.FirstName);
                dataReaderMock.Setup(reader => reader.GetString(2)).Returns(expectedPerson.LastName);

                var persons = await dataReaderMock.Object.GetAllAsync(
                                 reader => new Person
                                 {
                                     Id = reader.GetGuid(0),
                                     FirstName = reader.GetString(1),
                                     LastName = reader.GetString(2),
                                 }).ToListAsync();

                Assert.AreEqual(count, persons.Count);

                foreach (var person in persons)
                {
                    Assert.That(person, Is.DeepEqualTo(expectedPerson));
                }
            }

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

                var makeGenericMethod = typeof(DataReaderExtensions).GetMethods().FirstOrDefault(
                        info => info.Name == nameof(DataReaderExtensions.GetAllAsync)
                                && info.GetParameters()[1].ParameterType == typeof(IDataReaderOptions))
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
        public class The_GetSingleAsync_Method
        {
            /// <summary>
            /// The returns_ the_ entity_ from_ the_ projection.
            /// </summary>
            /// <returns>
            /// The <see cref="Task"/>.
            /// </returns>
            [Test]
            public async Task Returns_The_Entity_From_The_Projection()
            {
                var dataReaderMock = new Mock<IDataReader>();
                dataReaderMock.Setup(reader => reader.Read()).Returns(true);
                var expectedPerson = new Person
                {
                    Id = Guid.NewGuid(),
                    FirstName = Guid.NewGuid().ToString(),
                    LastName = Guid.NewGuid().ToString(),
                };

                dataReaderMock.Setup(reader => reader.GetGuid(0)).Returns(expectedPerson.Id);
                dataReaderMock.Setup(reader => reader.GetString(1)).Returns(expectedPerson.FirstName);
                dataReaderMock.Setup(reader => reader.GetString(2)).Returns(expectedPerson.LastName);

                var person = await dataReaderMock.Object.GetSingleAsync(
                                 reader => new Person
                                 {
                                     Id = reader.GetGuid(0),
                                     FirstName = reader.GetString(1),
                                     LastName = reader.GetString(2),
                                 });

                Assert.That(person, Is.DeepEqualTo(expectedPerson));
            }

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

                var makeGenericMethod = typeof(DataReaderExtensions).GetMethods().FirstOrDefault(
                        info => info.Name == nameof(DataReaderExtensions.GetSingleAsync)
                                && info.GetParameters()[1].ParameterType == typeof(IDataReaderOptions))
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
            public async Task Succeeds_Even_When_Deserialization_Fails_Or_Propery_Types_Does_Not_Match(object entity)
            {
                var dataReaderMock = new Mock<IDataReader>();

                dataReaderMock.Setup(reader => reader.Read()).Returns(true);

                SetupMockFromExpectedResultButWrongTypes(entity, dataReaderMock);

                var makeGenericMethod = typeof(DataReaderExtensions).GetMethods().FirstOrDefault(
                        info => info.Name == nameof(DataReaderExtensions.GetSingleAsync)
                                && info.GetParameters()[1].ParameterType == typeof(IDataReaderOptions))
                    ?.MakeGenericMethod(entity.GetType());

                var parameters = new object[]
                                 {
                                     dataReaderMock.Object, new DataReaderOptions
                                                            {
                                                                DefaultHandler = (s, type) => throw new Exception(),
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

        /// <summary>
        ///     The get all method.
        /// </summary>
        [TestFixture]
        public class The_GetAll_Method
        {
            /// <summary>
            /// Returns a nom empty enumeration when read does not throw an exception.
            /// </summary>
            [Test]
            public void Returns_A_Nom_Empty_Enumeration_When_Read_Does_Not_Throw_An_Exception()
            {
                var dataReaderMock = new Mock<IDataReader>();

                var count = 0;
                dataReaderMock.Setup(reader => reader.Read()).Callback(() => count++).Returns(() => count < 2);
                dataReaderMock.Setup(reader => reader.GetString(It.IsAny<int>())).Returns("string data");

                var collection = dataReaderMock.Object.GetAll(reader => reader.GetString(0), true).ToList();

                Assert.IsNotEmpty(collection);
            }

            /// <summary>
            ///     Returns empty enumeration when projection throws an exception.
            /// </summary>
            [Test]
            public void Returns_Empty_Enumeration_When_Projection_Throws_An_Exception()
            {
                var dataReaderMock = new Mock<IDataReader>();
                dataReaderMock.Setup(reader => reader.Read()).Returns(true);
                dataReaderMock.Setup(reader => reader.GetString(0)).Throws<Exception>();
                var persons = dataReaderMock.Object.GetAll(
                    reader => new Person
                    {
                        FirstName = reader.GetString(0),
                    },
                    true).ToList();

                Assert.IsEmpty(persons);
            }

            /// <summary>
            ///     Throws an exception when projection throws an exception and safety is false.
            /// </summary>
            [Test]
            public void Throws_An_Exception_When_Projection_Throws_An_Exception_And_Safety_Is_False()
            {
                var dataReaderMock = new Mock<IDataReader>();
                dataReaderMock.Setup(reader => reader.Read()).Returns(true);
                dataReaderMock.Setup(reader => reader.GetString(0)).Throws<Exception>();
                Assert.Throws<Exception>(
                    () => dataReaderMock.Object.GetAll(
                        reader => new Person
                        {
                            FirstName = reader.GetString(0),
                        }).ToList());
            }

            /// <summary>
            ///     Returns empty enumeration when data reader read throws an exception.
            /// </summary>
            [Test]
            public void Returns_Empty_Enumeration_When_DataReader_Read_Throws_An_Exception()
            {
                var dataReaderMock = new Mock<IDataReader>();

                dataReaderMock.Setup(reader => reader.Read()).Throws<Exception>();

                var persons = dataReaderMock.Object.GetAll(
                    reader => new Person
                    {
                        FirstName = reader.GetString(0),
                    },
                    true).ToList();

                Assert.IsEmpty(persons);
            }

            /// <summary>
            ///     Returns an empty enumeration when GetString throws an exception.
            /// </summary>
            [Test]
            public void Returns_An_Empty_Enumeration_When_GetString_Throws_An_Exception()
            {
                var dataReaderMock = new Mock<IDataReader>();

                var count = 0;
                dataReaderMock.Setup(reader => reader.Read()).Callback(() => count++).Returns(() => count < 2);
                dataReaderMock.Setup(reader => reader.GetString(It.IsAny<int>())).Throws<Exception>();

                var collection = dataReaderMock.Object.GetAll(reader => reader.GetString(0), true).ToList();

                Assert.IsEmpty(collection);
            }

            /// <summary>
            ///     Throws exception when reader read throws an exception.
            /// </summary>
            [Test]
            public void Throws_Exception_When_Reader_Read_Throws_An_Exception()
            {
                var dataReaderMock = new Mock<IDataReader>();

                dataReaderMock.Setup(reader => reader.Read()).Throws<Exception>();

                Assert.Throws<Exception>(
                    () => dataReaderMock.Object.GetAll(
                        reader => new Person
                        {
                            FirstName = reader.GetString(0),
                        }).ToList());
            }
        }
    }
}