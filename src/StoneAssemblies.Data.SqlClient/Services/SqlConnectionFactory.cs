// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SqlConnectionFactory.cs" company="Stone Assemblies">
// Copyright © 2021 - 2021 Stone Assemblies. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace StoneAssemblies.Data.SqlClient.Services
{
    using System.Data;

    using Microsoft.Data.SqlClient;

    using StoneAssemblies.Data.Services.Interfaces;

    /// <summary>
    ///     The SQL client connection factory.
    /// </summary>
    public class SqlConnectionFactory : IDbConnectionFactory
    {
        /// <summary>
        ///     Creates an <see cref="SqlConnection" />.
        /// </summary>
        /// <param name="connectionString">
        ///     The connection string.
        /// </param>
        /// <returns>
        ///     The <see cref="IDbConnection" />.
        /// </returns>
        public IDbConnection Create(string connectionString)
        {
            return new SqlConnection(connectionString);
        }
    }
}