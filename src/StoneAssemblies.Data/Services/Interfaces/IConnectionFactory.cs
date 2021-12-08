// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IConnectionFactory.cs" company="Stone Assemblies">
// Copyright © 2021 - 2021 Stone Assemblies. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace StoneAssemblies.Data.Services.Interfaces
{
    using System.Data;

    /// <summary>
    ///     The database connection factory interface.
    /// </summary>
    public interface IConnectionFactory
    {
        /// <summary>
        ///     Creates a database connection.
        /// </summary>
        /// <typeparam name="TConnection">
        ///     The connection type.
        /// </typeparam>
        /// <param name="connectionString">
        ///     The connection string.
        /// </param>
        /// <returns>
        ///     The <see cref="IDbConnection" />.
        /// </returns>
        IDbConnection Create<TConnection>(string connectionString)
            where TConnection : IDbConnection;
    }
}