// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DbCommandExtensions.cs" company="Stone Assemblies">
// Copyright © 2021 - 2021 Stone Assemblies. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace StoneAssemblies.Data.Extensions
{
    using System.Data;
    using System.Data.Common;
    using System.Threading;
    using System.Threading.Tasks;

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
        /// <param name="cancellationToken">
        ///     The cancellation token.
        /// </param>
        /// <returns>
        ///     The <see cref="Task" />.
        /// </returns>
        public static async Task<object> ExecuteScalarAsync(this IDbCommand command, CancellationToken cancellationToken = default)
        {
            if (command is DbCommand dbConnection)
            {
                return await dbConnection.ExecuteScalarAsync(cancellationToken);
            }

            return command.ExecuteScalar();
        }
    }
}