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

        [TestFixture]
        public class The_SingleAsync_Method
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

            [Test]
            [TestCaseSource(nameof(Entities))]
            public async Task Returns_The_Expected_Result(object entity)
            {
                var dataReaderMock = new Mock<IDataReader>();

                dataReaderMock.Setup(reader => reader.Read()).Returns(true);

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
        }
    }
}