// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDataReaderOptions.cs" company="Stone Assemblies">
// Copyright © 2021 - 2021 Stone Assemblies. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace StoneAssemblies.Data.Extensions.Interfaces
{
    using System;

    /// <summary>
    ///     The DataReaderOptions interface.
    /// </summary>
    public interface IDataReaderOptions
    {
        /// <summary>
        ///     Gets the default handler.
        /// </summary>
        Func<string, Type, object> DefaultHandler { get; }
    }
}