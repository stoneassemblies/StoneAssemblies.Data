// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyInfoExtension.cs" company="Stone Assemblies">
// Copyright © 2021 - 2021 Stone Assemblies. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace StoneAssemblies.Data.Extensions
{
    using System;
    using System.Reflection;

    /// <summary>
    ///     The property info extension.
    /// </summary>
    public static class PropertyInfoExtension
    {
        /// <summary>
        ///     The is readable from database.
        /// </summary>
        /// <param name="propertyInfo">
        ///     The property info.
        /// </param>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public static bool IsReadableFromDatabase(this PropertyInfo propertyInfo)
        {
            return propertyInfo.PropertyType.IsPrimitive || propertyInfo.PropertyType.IsEnum
                                                         || propertyInfo.PropertyType == typeof(string)
                                                         || propertyInfo.PropertyType == typeof(Guid)
                                                         || propertyInfo.PropertyType == typeof(TimeSpan)
                                                         || propertyInfo.PropertyType == typeof(DateTime);
        }
    }
}