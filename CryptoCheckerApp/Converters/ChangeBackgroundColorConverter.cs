namespace CryptoChecker.Converters
{
    using System;
    using System.Globalization;

    using CryptoChecker.Models;

    using Xamarin.Forms;

    /// <summary>
    /// Converted to change the background of the box-view based on the percentage change value.
    /// NOT BEING USED: It has been discovered the box-views got glitch during scrolling.
    /// </summary>
    public class ChangeBackgroundColorConverter : IValueConverter
    {
        /// <summary>
        /// Overwritten method to convert a value into the target type.
        /// </summary>
        /// <param name="value">
        /// The source value.
        /// </param>
        /// <param name="targetType">
        /// The target type.
        /// </param>
        /// <param name="parameter">
        /// The parameter.
        /// </param>
        /// <param name="culture">
        /// The culture.
        /// </param>
        /// <returns>
        /// The <see cref="object"/> new target type object.
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is CoinItem currency
                       ? currency.ChangedIn24Hours == 0 ? Color.Gray :
                         currency.ChangedIn24Hours > 0 ? Color.GreenYellow : Color.Red
                       : value;
        }

        /// <summary>
        /// Overwritten method to convert back the target type into source type.
        /// </summary>
        /// <param name="value">
        /// The source value.
        /// </param>
        /// <param name="targetType">
        /// The target type.
        /// </param>
        /// <param name="parameter">
        /// The parameter.
        /// </param>
        /// <param name="culture">
        /// The culture.
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}