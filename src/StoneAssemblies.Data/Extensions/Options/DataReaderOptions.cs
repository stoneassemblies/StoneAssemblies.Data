// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataReaderOptions.cs" company="Stone Assemblies">
// Copyright © 2021 - 2021 Stone Assemblies. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace StoneAssemblies.Data.Extensions.Options
{
    using System;

    using StoneAssemblies.Data.Extensions.Interfaces;

    /// <summary>
    ///     The data reader options.
    /// </summary>
    public class DataReaderOptions : IDataReaderOptions
    {
        /// <summary>
        ///     Gets or sets the default handler.
        /// </summary>
        public Func<string, Type, object> DefaultHandler { get; set; }
    }
}