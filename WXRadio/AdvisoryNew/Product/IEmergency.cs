namespace WXRadio.WeatherManager.Product
{
    /// <summary>
    /// Indicates that this product can be treated as an emergency
    /// </summary>
    public interface IEmergency
    {
        EmergencyNotificationTypes EmergencyNotificationType { get; }
        bool QueueForImmediateBroadcast();
        void AcknowledgeImmediateBroadcastQueue();
        string ImmediateBroadcastInformation();
    }

    public enum EmergencyNotificationTypes
    {
        /// <summary>
        /// Would not prompt siren activation
        /// </summary>
        General,
        /// <summary>
        /// Would prompt siren activation for limited amount of time
        /// </summary>
        WarnPublicOnce,
        /// <summary>
        /// Would prompt consistent siren notification until cancellation
        /// </summary>
        WarnPublicConsistently
    }
}
