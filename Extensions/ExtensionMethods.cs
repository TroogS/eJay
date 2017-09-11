using System.Globalization;
using System.Windows;

namespace DebtMgr.Extensions
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   An extension methods. </summary>
    ///
    /// <remarks>   Andre Beging, 10.09.2017. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    public static class ExtensionMethods
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Centers the current window on parent </summary>
        ///
        /// <remarks>   Andre Beging, 08.09.2017. </remarks>
        ///
        /// <param name="window">   The window to act on. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public static void CenterOnParent(this Window window)
        {
            var curApp = Application.Current;
            var mainWindow = curApp.MainWindow;
            window.Left = mainWindow.Left + (mainWindow.Width - window.ActualWidth) / 2 - (window.Width / 2);
            window.Top = mainWindow.Top + (mainWindow.Height - window.ActualHeight) / 2 - (window.Height / 2);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   A string extension method that gets a double. </summary>
        ///
        /// <remarks>   Andre Beging, 09.09.2017. </remarks>
        ///
        /// <param name="value">        The value to act on. </param>
        /// <param name="defaultValue"> The default value. </param>
        ///
        /// <returns>   The double. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public static double GetDouble(this string value, double defaultValue)
        {
            double result;

            // Try parsing in the current culture
            if (!double.TryParse(value, NumberStyles.Any, CultureInfo.CurrentCulture, out result) &&
                // Then try in US english
                !double.TryParse(value, NumberStyles.Any, CultureInfo.GetCultureInfo("en-US"), out result) &&
                // Then in neutral language
                !double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out result))
            {
                result = defaultValue;
            }
            return result;
        }
    }
}
