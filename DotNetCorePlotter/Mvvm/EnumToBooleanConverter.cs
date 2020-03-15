using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace DotNetCorePlotter.Mvvm
{
    /// <summary>
    /// Converter that converts Enum values to Boolean values and vice versa, by comparing with the passed parameter.
    /// </summary>
    public class EnumToBooleanConverter : IValueConverter
    {
        /// <summary>
        /// Converts the passed Enum value to a Boolean value, if it is the same as the passed parameter.
        /// </summary>
        /// <param name="value">Enum value to be converted.</param>
        /// <param name="targetType">Target Enum type.</param>
        /// <param name="parameter">Enum parameter to compare with.</param>
        /// <param name="culture">Current runtime's culture.</param>
        /// <returns><c>true</c> if the passed value is the same as the passed parameter; <c>false</c> otherwise.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string parameterString = parameter as string;
            if (parameterString == null)
            {
                return DependencyProperty.UnsetValue;
            }

            if (Enum.IsDefined(value.GetType(), value) == false)
            {
                return DependencyProperty.UnsetValue;
            }

            object parameterValue = Enum.Parse(value.GetType(), parameterString);

            return parameterValue.Equals(value);
        }

        /// <summary>
        /// Inverse conversion of the Enum to Boolean converter.
        /// </summary>
        /// <param name="value">Enum value to be converted.</param>
        /// <param name="targetType">Target Enum type.</param>
        /// <param name="parameter">Enum parameter to compare with.</param>
        /// <param name="culture">Current runtime's culture.</param>
        /// <returns>The passed Enum value, if the parameter is <c>true</c>.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string parameterString = parameter as string;
            if (parameterString == null)
                return DependencyProperty.UnsetValue;

            return Enum.Parse(targetType, parameterString);
        }
    }
}