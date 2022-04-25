namespace CryptoChecker.ViewModels
{
    using CryptoChecker.Models.BaseModels;

    /// <summary>
    /// Class which represents the Base model for each View page.
    /// </summary>
    public class BaseViewModel : BaseModel
    {
        /// <summary>
        /// The title of the page.
        /// </summary>
        private string title = string.Empty;

        /// <summary>
        /// Weather there is an error on the page.
        /// </summary>
        private bool isThereAnError;

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether is there an error on the page.
        /// </summary>
        public bool IsThereAnError
        {
            get => isThereAnError;
            set => SetProperty(ref isThereAnError, value);
        }
    }
}
