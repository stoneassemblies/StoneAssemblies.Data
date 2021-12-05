// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DbCommandExtensions.cs" company="Stone Assemblies">
// Copyright © 2021 - 2021 Stone Assemblies. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace StoneAssemblies.Data.Extensions
{
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Threading.Tasks;

    using StoneAssemblies.Data.Extensions.Interfaces;

    /// <summary>
    ///     The database command extensions.
    /// </summary>
    public static class DbCommandExtensions
    {
        /// <summary>
        ///     Adds parameter with value.
        /// </summary>
        /// <param name="command">
        ///     The command.
        /// </param>
        /// <param name="parameterName">
        ///     The parameter name.
        /// </param>
        /// <param name="value">
        ///     The value.
        /// </param>
        public static void AddParameterWithValue(this IDbCommand command, string parameterName, object value)
        {
            var uniqueReferenceParameter = command.CreateParameter();
            uniqueReferenceParameter.ParameterName = parameterName;
            uniqueReferenceParameter.Value = value;
            command.Parameters.Add(uniqueReferenceParameter);
        }

        /// <summary>
        ///     Gets all async.
        /// </summary>
        /// <param name="command">
        ///     The command.
        /// </param>
        /// <param name="dataReaderOptions">
        ///     The data reader options.
        /// </param>
        /// <typeparam name="TResponse">
        ///     The response type.
        /// </typeparam>
        /// <returns>
        ///     The <see cref="Task" />.
        /// </returns>
        public static async IAsyncEnumerable<TResponse> AllAsync<TResponse>(this IDbCommand command, IDataReaderOptions dataReaderOptions = null)
            where TResponse : new()
        {
            IDataReader dataReader;
            if (command is DbCommand dbCommand)
            {
                dataReader = await dbCommand.ExecuteReaderAsync();
            }
            else
            {
                dataReader = command.ExecuteReader();
            }

            await foreach (var response in dataReader.AllAsync<TResponse>(dataReaderOptions))
            {
                yield return response;
            }
        }

        /// <summary>
        ///     Execute non query async.
        /// </summary>
        /// <param name="command">
        ///     The command.
        /// </param>
        /// <returns>
        ///     The <see cref="Task" />.
        /// </returns>
        public static async Task<int> ExecuteNonQueryAsync(this IDbCommand command)
        {
            if (command is DbCommand dbCommand)
            {
                return await dbCommand.ExecuteNonQueryAsync();
            }

            return command.ExecuteNonQuery();
        }

        /// <summary>
        ///     Executes reader async.
        /// </summary>
        /// <param name="command">
        ///     The command.
        /// </param>
        /// <returns>
        ///     The <see cref="Task" />.
        /// </returns>
        public static async Task<IDataReader> ExecuteReaderAsync(this IDbCommand command)
        {
            if (command is DbCommand dbCommand)
            {
                return await dbCommand.ExecuteReaderAsync();
            }

            return command.ExecuteReader();
        }

        /// <summary>
        ///     The single async.
        /// </summary>
        /// <param name="command">
        ///     The command.
        /// </param>
        /// <param name="dataReaderOptions">
        ///     The options.
        /// </param>
        /// <typeparam name="TResponse">
        ///     The response type.
        /// </typeparam>
        /// <returns>
        ///     The <see cref="Task" />.
        /// </returns>
        public static async Task<TResponse> SingleAsync<TResponse>(
            this IDbCommand command, IDataReaderOptions dataReaderOptions = null)
            where TResponse : new()
        {
            IDataReader dataReader;
            if (command is DbCommand dbCommand)
            {
                dataReader = await dbCommand.ExecuteReaderAsync();
            }
            else
            {
                dataReader = command.ExecuteReader();
            }

            return await dataReader.SingleAsync<TResponse>(dataReaderOptions);
        }
    }
}