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
        ///     All async.
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
        public static async IAsyncEnumerable<TEntity> AllAsync<TEntity>(
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
                await dataReader.FillEntityAsync(entity, dataReaderOptions, properties);
                yield return entity;
            }
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
        ///     Query single async.
        /// </summary>
        /// <param name="dataReader">
        ///     The data reader.
        /// </param>
        /// <param name="dataReaderOptions">
        ///     The options.
        /// </param>
        /// <typeparam name="TResponse">
        ///     The response type.
        /// </typeparam>
        /// <returns>
        ///     The <see cref="Task" />.
        ///     The task with the response.
        /// </returns>
        public static async Task<TResponse> SingleAsync<TResponse>(
            this IDataReader dataReader, IDataReaderOptions dataReaderOptions = null)
            where TResponse : new()
        {
            return await dataReader.AllAsync<TResponse>(dataReaderOptions).FirstAsync();
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
        /// <param name="dataReaderOptions">
        ///     The data reader options.
        /// </param>
        /// <param name="properties">
        ///     The properties.
        /// </param>
        /// <returns>
        ///     The <see cref="Task" />.
        /// </returns>
        private static async Task FillEntityAsync(this IDataReader dataReader, object entity, IDataReaderOptions dataReaderOptions, Dictionary<string, PropertyInfo> properties)
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
                try
                {
                    var value = dataReader.GetValue(fieldIdx);
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
                try
                {
                    var fieldValue = dataReader.GetString(fieldIdx);
                    var deserializedFieldValue = dataReaderOptions?.DefaultHandler(fieldValue, property.PropertyType);
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