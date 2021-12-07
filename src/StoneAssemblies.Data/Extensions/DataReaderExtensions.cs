// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataReaderExtensions.cs" company="Stone Assemblies">
//  Copyright © 2021 - 2021 Stone Assemblies. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace StoneAssemblies.Data.Extensions
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;

    using Dasync.Collections;

    using Serilog;

    using StoneAssemblies.Data.Extensions.Interfaces;

    /// <summary>
    ///     The data reader extensions.
    /// </summary>
    public static class DataReaderExtensions
    {
        /// <summary>
        ///     The properties cache per type.
        /// </summary>
        private static readonly ConcurrentDictionary<Type, Dictionary<string, PropertyInfo>> PropertiesCache =
            new ConcurrentDictionary<Type, Dictionary<string, PropertyInfo>>();

        /// <summary>
        ///     Gets all async.
        /// </summary>
        /// <param name="dataReader">
        ///     The data reader.
        /// </param>
        /// <param name="dataReaderOptions">
        ///     The data reader options.
        /// </param>
        /// <typeparam name="TEntity">
        ///     The response type.
        /// </typeparam>
        /// <returns>
        ///     The <see cref="IAsyncEnumerable{TResponse}" />.
        /// </returns>
        public static async IAsyncEnumerable<TEntity> GetAllAsync<TEntity>(
            this IDataReader dataReader, IDataReaderOptions dataReaderOptions = null)
            where TEntity : new()
        {
            var properties = PropertiesCache.GetOrAdd(
                typeof(TEntity),
                type => type.GetProperties(BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.Instance).ToDictionary(
                    info => info.Name,
                    StringComparer.InvariantCultureIgnoreCase));

            while (await dataReader.ReadAsync())
            {
                var entity = new TEntity();
                await dataReader.FillEntityAsync(entity, properties, dataReaderOptions);
                yield return entity;
            }
        }

        /// <summary>
        ///     Gets all async.
        /// </summary>
        /// <param name="dataReader">
        ///     The data reader.
        /// </param>
        /// <param name="projection">
        ///     The projection.
        /// </param>
        /// <typeparam name="TEntity">
        ///     The entity type.
        /// </typeparam>
        /// <returns>
        ///     The <see cref="IAsyncEnumerable" />.
        /// </returns>
        public static async IAsyncEnumerable<TEntity> GetAllAsync<TEntity>(
            this IDataReader dataReader, Func<IDataReader, TEntity> projection)
        {
            while (await dataReader.ReadAsync())
            {
                var value = projection(dataReader);
                yield return value;
            }
        }

        /// <summary>
        ///     Reads the next record.
        /// </summary>
        /// <param name="dataReader">
        ///     The data reader.
        /// </param>
        /// <param name="safety">
        ///     Indicates whether will be a safety read.
        /// </param>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public static bool Read(this IDataReader dataReader, bool safety)
        {
            try
            {
                return dataReader.Read();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error reading the next record from data reader");

                if (!safety)
                {
                    throw;
                }

                return false;
            }
        }

        /// <summary>
        ///     Selects records from data reader.
        /// </summary>
        /// <param name="dataReader">
        ///     The data reader.
        /// </param>
        /// <param name="projection">
        ///     The projection.
        /// </param>
        /// <param name="safety">
        ///     Indicates whether the operation will be safe.
        /// </param>
        /// <typeparam name="TEntity">
        ///     The result type.
        /// </typeparam>
        /// <returns>
        ///     The <see cref="IEnumerable{TEntity}" />.
        /// </returns>
        public static IEnumerable<TEntity> GetAll<TEntity>(
            this IDataReader dataReader, Func<IDataReader, TEntity> projection, bool safety = false)
        {
            while (dataReader.Read(safety))
            {
                TEntity value;

                try
                {
                    value = projection(dataReader);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Error projecting from data reader");

                    if (!safety)
                    {
                        throw;
                    }

                    yield break;
                }

                yield return value;
            }
        }

        /// <summary>
        ///     Gets a single instance async.
        /// </summary>
        /// <param name="dataReader">
        ///     The data reader.
        /// </param>
        /// <param name="projection">
        ///     The projection.
        /// </param>
        /// <typeparam name="TEntity">
        ///     The entity type.
        /// </typeparam>
        /// <returns>
        ///     The <see cref="Task" />.
        /// </returns>
        public static async Task<TEntity> GetSingleAsync<TEntity>(this IDataReader dataReader, Func<IDataReader, TEntity> projection)
            where TEntity : new()
        {
            return await dataReader.GetAllAsync(projection).FirstAsync();
        }

        /// <summary>
        ///     Gets a single instance async.
        /// </summary>
        /// <param name="dataReader">
        ///     The data reader.
        /// </param>
        /// <param name="dataReaderOptions">
        ///     The options.
        /// </param>
        /// <typeparam name="TEntity">
        ///     The entity type.
        /// </typeparam>
        /// <returns>
        ///     The <see cref="Task" />.
        ///     The task with the response.
        /// </returns>
        public static async Task<TEntity> GetSingleAsync<TEntity>(
            this IDataReader dataReader, IDataReaderOptions dataReaderOptions = null)
            where TEntity : new()
        {
            return await dataReader.GetAllAsync<TEntity>(dataReaderOptions).FirstAsync();
        }

        /// <summary>
        ///     Checks whether is database value is null async.
        /// </summary>
        /// <param name="dataReader">
        ///     The data reader.
        /// </param>
        /// <param name="ordinal">
        ///     The ordinal.
        /// </param>
        /// <returns>
        ///     The <see cref="Task" />.
        /// </returns>
        public static async Task<bool> IsDBNullAsync(this IDataReader dataReader, int ordinal)
        {
            if (dataReader is DbDataReader dbDataReader)
            {
                return await dbDataReader.IsDBNullAsync(ordinal);
            }

            return dataReader.IsDBNull(ordinal);
        }

        /// <summary>
        ///     Reads async.
        /// </summary>
        /// <param name="dataReader">
        ///     The data reader.
        /// </param>
        /// <returns>
        ///     The <see cref="Task" />.
        /// </returns>
        public static async Task<bool> ReadAsync(this IDataReader dataReader)
        {
            if (dataReader is DbDataReader dbDataReader)
            {
                return await dbDataReader.ReadAsync();
            }

            return dataReader.Read();
        }

        /// <summary>
        ///     Fills the object async.
        /// </summary>
        /// <param name="dataReader">
        ///     The data reader.
        /// </param>
        /// <param name="entity">
        ///     The object to be filled.
        /// </param>
        /// <param name="properties">
        ///     The properties.
        /// </param>
        /// <param name="dataReaderOptions">
        ///     The data reader options.
        /// </param>
        /// <returns>
        ///     The <see cref="Task" />.
        /// </returns>
        private static async Task FillEntityAsync(
            this IDataReader dataReader,
            object entity,
            Dictionary<string, PropertyInfo> properties,
            IDataReaderOptions dataReaderOptions)
        {
            var fieldIdx = 0;
            var propertyIdx = 0;
            while (fieldIdx < dataReader.FieldCount && propertyIdx < properties.Count)
            {
                var fieldName = dataReader.GetName(fieldIdx);
                if (!await dataReader.IsDBNullAsync(fieldIdx) && properties.TryGetValue(fieldName, out var property))
                {
                    dataReader.FillEntityPropertyFromField(entity, property, fieldIdx, dataReaderOptions);
                    propertyIdx++;
                }

                fieldIdx++;
            }
        }

        /// <summary>
        ///     Fills entity property from data set field.
        /// </summary>
        /// <param name="dataReader">
        ///     The data reader.
        /// </param>
        /// <param name="entity">
        ///     The entity.
        /// </param>
        /// <param name="property">
        ///     The property.
        /// </param>
        /// <param name="fieldIdx">
        ///     The field index.
        /// </param>
        /// <param name="dataReaderOptions">
        ///     The data reader options.
        /// </param>
        private static void FillEntityPropertyFromField(
            this IDataReader dataReader, object entity, PropertyInfo property, int fieldIdx, IDataReaderOptions dataReaderOptions)
        {
            if (property.IsReadableFromDatabase())
            {
                var value = dataReader.GetValue(fieldIdx);
                try
                {
                    property.SetValue(entity, value);
                }
                catch (Exception ex)
                {
                    Log.Warning(
                        ex,
                        "Error setting native value for {PropertyName} from data reader field {FieldIdx}",
                        property.Name,
                        fieldIdx);
                }
            }
            else if (dataReaderOptions?.DefaultHandler != null)
            {
                var value = dataReader.GetValue(fieldIdx);
                try
                {
                    var serializedValue = (string)value;
                    var deserializedFieldValue = dataReaderOptions?.DefaultHandler(serializedValue, property.PropertyType);
                    property.SetValue(entity, deserializedFieldValue);
                }
                catch (Exception ex)
                {
                    Log.Warning(
                        ex,
                        "Error deserializing for {PropertyName} from data reader field {FieldIdx} using default handler",
                        property.Name,
                        fieldIdx);
                }
            }
        }
    }
}