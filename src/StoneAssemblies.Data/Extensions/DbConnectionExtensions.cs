// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DbConnectionExtensions.cs" company="Stone Assemblies">
// Copyright © 2021 - 2021 Stone Assemblies. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace StoneAssemblies.Data.Extensions
{
    using System;
    using System.Data;
    using System.Data.Common;
    using System.Threading.Tasks;

    /// <summary>
    ///     The database connection extensions.
    /// </summary>
    public static class DbConnectionExtensions
    {
        /// <summary>
        ///     Close the connection async.
        /// </summary>
        /// <param name="connection">
        ///     The connection.
        /// </param>
        /// <returns>
        ///     The <see cref="Task" />.
        /// </returns>
        public static async Task CloseAsync(this IDbConnection connection)
        {
            if (connection is DbConnection dbConnection)
            {
                await dbConnection.CloseAsync();
            }
            else
            {
                connection.Close();
            }
        }

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources
        ///     asynchronously.
        /// </summary>
        /// <param name="connection">
        ///     The connection.
        /// </param>
        /// <returns>
        ///     The <see cref="Task" />.
        /// </returns>
        public static async Task DisposeAsync(this IDbConnection connection)
        {
            if (connection is IAsyncDisposable dbConnection)
            {
                await dbConnection.DisposeAsync();
            }
            else
            {
                connection.Dispose();
            }
        }

        /// <summary>
        ///     Open connection async.
        /// </summary>
        /// <param name="connection">
        ///     The connection.
        /// </param>
        /// <returns>
        ///     The <see cref="Task" />.
        /// </returns>
        public static async Task OpenAsync(this IDbConnection connection)
        {
            if (connection is DbConnection dbConnection)
            {
                await dbConnection.OpenAsync();
            }
            else
            {
                connection.Open();
            }
        }
    }
}