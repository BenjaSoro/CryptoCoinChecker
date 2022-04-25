namespace CryptoChecker.Models
{
    using CryptoChecker.Models.BaseModels;

    /// <summary>
    /// Class which represents the label used to inform about the search results.
    /// </summary>
    public class SearchResultLabel : BaseLabel
    {
        /// <summary>
        /// Method to change the text of the label based on the amount of results.
        /// </summary>
        /// <param name="resultsCount">
        /// The results count.
        /// </param>
        /// <param name="query">
        /// The search box query.
        /// </param>
        public void SetMessageBaseOnResults(int resultsCount, string query)
        {
            this.Text = resultsCount == 0 ? $"No results found for '{query}'" : $"{resultsCount} result(s) for the name '{query}'";
        }
    }
}
