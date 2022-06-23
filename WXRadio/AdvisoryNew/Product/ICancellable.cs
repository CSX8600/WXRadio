namespace WXRadio.WeatherManager.Product
{
    /// <summary>
    /// Indicates that a product can be cancelled.
    /// </summary>
    public interface ICancellable
    {
        bool IsCancelled { get; }
        /// <summary>
        /// The summary that would normally be spoken by a synthesizer.
        /// </summary>
        string GetCancelSummary();
    }
}
