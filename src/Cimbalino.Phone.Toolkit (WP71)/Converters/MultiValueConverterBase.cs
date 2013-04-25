﻿// ****************************************************************************
// <copyright file="MultiValueConverterBase.cs" company="Pedro Lamas">
// Copyright © Pedro Lamas 2013
// </copyright>
// ****************************************************************************
// <author>Pedro Lamas</author>
// <email>pedrolamas@gmail.com</email>
// <date>25-04-2013</date>
// <project>Cimbalino.Phone.Toolkit</project>
// <web>http://www.pedrolamas.com</web>
// <license>
// See license.txt in this solution or http://www.pedrolamas.com/license_MIT.txt
// </license>
// ****************************************************************************

using System;
using System.Globalization;
using System.Windows.Data;

namespace Cimbalino.Phone.Toolkit.Converters
{
    /// <summary>
    /// An <see cref="IValueConverter"/> abstract implementation to be used with the <see cref="Cimbalino.Phone.Toolkit.Behaviors.MultiBindingBehavior"/>.
    /// </summary>
    /// <typeparam name="T">The return type.</typeparam>
    public abstract class MultiValueConverterBase<T> : IValueConverter
    {
        /// <summary>
        /// Modifies the source data before passing it to the target for display in the UI.
        /// </summary>
        /// <returns>The value to be passed to the target dependency property.</returns>
        /// <param name="values">The source data being passed to the target.</param>
        /// <param name="targetType">The <see cref="T:System.Type"/> of data expected by the target dependency property.</param>
        /// <param name="parameter">An optional parameter to be used in the converter logic.</param>
        /// <param name="culture">The culture of the conversion.</param>
        public abstract T Convert(object[] values, Type targetType, object parameter, CultureInfo culture);

        /// <summary>
        /// Modifies the target data before passing it to the source object. This method is called only in <see cref="F:System.Windows.Data.BindingMode.TwoWay"/> bindings.
        /// </summary>
        /// <returns>The value to be passed to the source object.</returns>
        /// <param name="value">The target data being passed to the source.</param>
        /// <param name="targetType">The <see cref="T:System.Type"/> of data expected by the source object.</param>
        /// <param name="parameter">An optional parameter to be used in the converter logic.</param>
        /// <param name="culture">The culture of the conversion.</param>
        public abstract object[] ConvertBack(T value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture);

        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Convert((object[])value, targetType, parameter, culture);
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ConvertBack((T)value, targetType, parameter, culture);
        }
    }
}