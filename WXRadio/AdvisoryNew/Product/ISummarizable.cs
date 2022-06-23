namespace WXRadio.WeatherManager.Product
{
    /// <summary>
    /// Indicates that a Product can be summarized.
    /// </summary>
    public interface ISummarizable
    {
        /// <summary>
        /// The summary that would normally be spoken by the synthesizer.
        /// </summary>
        string GetSummary();
    }
}
