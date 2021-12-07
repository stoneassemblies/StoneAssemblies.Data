// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SqlConnectionFactory.cs" company="Stone Assemblies">
// Copyright © 2021 - 2021 Stone Assemblies. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace StoneAssemblies.Data.SqlClient.Services
{
    using System;
    using System.Data;

    using Microsoft.Data.SqlClient;

    using StoneAssemblies.Data.Services.Interfaces;

    /// <summary>
    ///     The SQL client connection factory.
    /// </summary>
    public class SqlConnectionFactory : IConnectionFactory
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
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(connectionString));
            }

            return new SqlConnection(connectionString);
        }
    }
}