namespace CryptoChecker.Models
{
    using CryptoChecker.Models.BaseModels;

    /// <summary>
    /// Class which represents the label used when the signalR is subscribing.
    /// </summary>
    public class SignalRConnectionLabel : BaseLabel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SignalRConnectionLabel"/> class.
        /// </summary>
        public SignalRConnectionLabel()
        {
            this.IsShowing = false;
            this.Text = "Subscribing for updated prices...";
        }
    }
}
