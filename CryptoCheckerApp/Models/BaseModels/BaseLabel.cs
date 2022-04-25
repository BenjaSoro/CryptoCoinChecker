namespace CryptoChecker.Models.BaseModels
{
    /// <summary>
    /// Class model for the Base Label definition.
    /// </summary>
    public class BaseLabel : BaseModel
    {
        /// <summary>
        /// Private field for Showing label.
        /// </summary>
        private bool isShowing;

        /// <summary>
        /// Private field for Text label.
        /// </summary>
        private string text;

        /// <summary>
        /// Gets or sets a value indicating whether label is showing.
        /// </summary>
        public bool IsShowing
        {
            get => this.isShowing;
            set => this.SetProperty(ref this.isShowing, value);
        }

        /// <summary>
        /// Gets or sets the label text.
        /// </summary>
        public string Text
        {
            get => this.text;
            set => this.SetProperty(ref this.text, value);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseLabel"/> class.
        /// </summary>
        public BaseLabel()
        {
            this.IsShowing = false;
        }
    }
}
