namespace CryptoChecker.Converters
{
    using System;
    using System.Globalization;
    using System.Reflection;

    using Xamarin.Forms;

    /// <summary>
    /// Converter to embed images into image sources.
    /// </summary>
    public class EmbeddedToImageSourceConverter : IValueConverter
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
            if (value is string fileName && parameter is string assemblyName)
            {
                try
                {
                    var imageSource = ImageSource.FromResource(assemblyName + "." + fileName, typeof(EmbeddedToImageSourceConverter).GetTypeInfo().Assembly);
                    return imageSource;
                }
                catch
                {
                    return value;
                }
            }

            return value;
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