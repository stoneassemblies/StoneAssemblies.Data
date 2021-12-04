// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDbConnectionFactory.cs" company="Stone Assemblies">
// Copyright © 2021 - 2021 Stone Assemblies. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace StoneAssemblies.Data.Services
{
    using System.Data;

    /// <summary>
    ///     The database connection factory interface.
    /// </summary>
    public interface IDbConnectionFactory
    {
        /// <summary>
        ///     Creates a database connection.
        /// </summary>
        /// <param name="connectionString">
        ///     The connection string.
        /// </param>
        /// <returns>
        ///     The <see cref="IDbConnection" />.
        /// </returns>
        IDbConnection Create(string connectionString);
    }
}