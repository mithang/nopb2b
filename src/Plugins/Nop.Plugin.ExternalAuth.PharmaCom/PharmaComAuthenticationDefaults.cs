namespace Nop.Plugin.ExternalAuth.PharmaCom
{
    /// <summary>
    /// Represents plugin constants
    /// </summary>
    public class PharmaComAuthenticationDefaults
    {
        /// <summary>
        /// Gets a name of the view component to display login button
        /// </summary>
        public const string VIEW_COMPONENT_NAME = "PharmaComAuthentication";

        /// <summary>
        /// Gets a plugin system name
        /// </summary>
        public static string SystemName = "ExternalAuth.PharmaCom";

        /// <summary>
        /// Gets a name of error callback method
        /// </summary>
        public static string ErrorCallback = "ErrorCallback";
    }
}