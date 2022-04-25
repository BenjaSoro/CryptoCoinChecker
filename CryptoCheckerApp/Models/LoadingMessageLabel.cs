namespace CryptoChecker.Models
{
    using CryptoChecker.Models.BaseModels;

    /// <summary>
    /// Class to represent the label used during loading the available coins.
    /// </summary>
    public class LoadingMessageLabel : BaseLabel
    {
        /// <summary>
        /// Second boolean to hide the label when the loading items completes.
        /// </summary>
        private bool isDone;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoadingMessageLabel"/> class.
        /// </summary>
        public LoadingMessageLabel()
        {
            this.IsShowing = true;
            this.IsDone = false;
            this.Text = "Loading please wait...";
        }

        /// <summary>
        /// Gets or sets a value indicating whether the loading the items is done.
        /// </summary>
        public bool IsDone
        {
            get => this.isDone;
            set => this.SetProperty(ref this.isDone, value);
        }
    }
}
