using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace eJay.Converters
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   An amount to color converter. </summary>
    ///
    /// <remarks>   Andre Beging, 10.09.2017. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    public class AmountToColorConverter : IValueConverter
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Converts. </summary>
        ///
        /// <remarks>   Andre Beging, 10.09.2017. </remarks>
        ///
        /// <param name="value">        The value. </param>
        /// <param name="targetType">   Type of the target. </param>
        /// <param name="parameter">    The parameter. </param>
        /// <param name="culture">      The culture. </param>
        ///
        /// <returns>   An object. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var defaultReturnValue = new SolidColorBrush(Colors.Black);
            if (value == null) return defaultReturnValue;
            

            double doubleValue = 0.0;
            if (value is double)
            {
                doubleValue = (double)value;
            }
            else if(value is string)
            {
                if (string.IsNullOrWhiteSpace(value.ToString())) return defaultReturnValue;
                doubleValue = Double.Parse(value.ToString());
            }

            if (doubleValue < 0.001 && doubleValue > -0.001)
                return new SolidColorBrush(Colors.Black);

            if (doubleValue > 0)
                return new SolidColorBrush(Colors.Green);

            if (doubleValue < 0)
                return new SolidColorBrush(Colors.Red);

            return defaultReturnValue;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Convert back. </summary>
        ///
        /// <remarks>   Andre Beging, 10.09.2017. </remarks>
        ///
        /// <param name="value">        The value. </param>
        /// <param name="targetType">   Type of the target. </param>
        /// <param name="parameter">    The parameter. </param>
        /// <param name="culture">      The culture. </param>
        ///
        /// <returns>   The back converted. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
