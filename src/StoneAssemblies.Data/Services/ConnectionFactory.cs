// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConnectionFactory.cs" company="Stone Assemblies">
// Copyright © 2021 - 2021 Stone Assemblies. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace StoneAssemblies.Data.Services
{
    using System;
    using System.Data;

    using StoneAssemblies.Data.Services.Interfaces;

    /// <summary>
    ///     The connection factory.
    /// </summary>
    public class ConnectionFactory : IConnectionFactory
    {
        /// <summary>
        ///     Creates a connection.
        /// </summary>
        /// <param name="connectionString">
        ///     The connection string.
        /// </param>
        /// <typeparam name="TConnection">
        ///     The connection type.
        /// </typeparam>
        /// <returns>
        ///     The <see cref="IDbConnection" />.
        /// </returns>
        public IDbConnection Create<TConnection>(string connectionString)
            where TConnection : IDbConnection
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(connectionString));
            }

            return (IDbConnection)Activator.CreateInstance(typeof(TConnection), connectionString);
        }
    }
}