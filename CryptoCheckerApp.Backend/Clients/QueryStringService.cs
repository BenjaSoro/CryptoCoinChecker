namespace CryptoCheckerApp.Backend.Clients
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    /// <summary>
    /// Class containing methods with Uri logic.
    /// </summary>
    public static class QueryStringService
    {
        /// <summary>
        /// Method to append parameters to an Uri.
        /// </summary>
        /// <param name="path">
        /// The path aka Uri.
        /// </param>
        /// <param name="parameter">
        /// The parameters to be added to Uri.
        /// </param>
        /// <returns>
        /// The <see cref="Uri"/> Uri which is created as a result.
        /// </returns>
        public static Uri AppendQueryString(string path, Dictionary<string, object> parameter)
        {
            return CreateUrl(path, parameter);
        }

        /// <summary>
        /// Private method which adds the list of parameters to the Path provided to create an Uri.
        /// </summary>
        /// <param name="path">
        /// The path to be used as base.
        /// </param>
        /// <param name="parameter">
        /// The list of parameters to be added to the Path.
        /// </param>
        /// <returns>
        /// The <see cref="Uri"/> Uri created with parameters added to Path.
        /// </returns>
        private static Uri CreateUrl(string path, Dictionary<string, object> parameter)
        {
            var urlParameters = new List<string>();
            foreach (var (key, value) in parameter)
            {
                urlParameters.Add(string.IsNullOrWhiteSpace(value?.ToString())
                                      ? null
                                      : $"{key}={value.ToString()?.ToLowerInvariant()}");
            }

            var encodedParams = urlParameters
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(WebUtility.HtmlEncode)
                .Select((x, i) => i > 0 ? $"&{x}" : $"?{x}")
                .ToArray();
            var url = encodedParams.Length > 0 ? $"{path}{string.Join(string.Empty, encodedParams)}" : path;
            return new Uri(url);
        }
    }
}
