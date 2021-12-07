// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DbCommandExtensions.cs" company="Stone Assemblies">
// Copyright © 2021 - 2021 Stone Assemblies. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace StoneAssemblies.Data.Extensions
{
    using System;
    using System.Data;
    using System.Data.Common;
    using System.Threading.Tasks;

    using Serilog;

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
        ///     Execute reader safety.
        /// </summary>
        /// <param name="command">
        ///     The command.
        /// </param>
        /// <param name="safety">
        ///     Indicates if the execution will be safety.
        /// </param>
        /// <returns>
        ///     The <see cref="IDataAdapter" />.
        /// </returns>
        public static IDataReader ExecuteReader(this IDbCommand command, bool safety)
        {
            IDataReader dataReader = null;
            try
            {
                dataReader = command.ExecuteReader();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error executing reader");

                if (!safety)
                {
                    throw;
                }
            }

            return dataReader;
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
        ///     This is the asynchronous version of ExecuteScalar().
        /// </summary>
        /// <param name="command">
        ///     The command.
        /// </param>
        /// <returns>
        ///     The <see cref="Task" />.
        /// </returns>
        public static async Task<object> ExecuteScalarAsync(this IDbCommand command)
        {
            if (command is DbCommand dbConnection)
            {
                return await dbConnection.ExecuteScalarAsync();
            }

            return command.ExecuteScalar();
        }

        /// <summary>
        ///     This is the asynchronous and generic version of ExecuteScalar().
        /// </summary>
        /// <param name="command">
        ///     The command.
        /// </param>
        /// <typeparam name="TResult">
        ///     The result type.
        /// </typeparam>
        /// <returns>
        ///     The <see cref="Task" />.
        /// </returns>
        public static async Task<TResult> ExecuteScalarAsync<TResult>(this IDbCommand command)
        {
            if (command is DbCommand dbConnection)
            {
                return (TResult)await dbConnection.ExecuteScalarAsync();
            }

            return (TResult)command.ExecuteScalar();
        }
    }
}