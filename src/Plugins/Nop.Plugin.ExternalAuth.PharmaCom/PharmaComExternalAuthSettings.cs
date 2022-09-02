using Nop.Core.Configuration;

namespace Nop.Plugin.ExternalAuth.PharmaCom
{
    /// <summary>
    /// Represents settings of the PharmaCom authentication method
    /// </summary>
    public class PharmaComExternalAuthSettings : ISettings
    {
        /// <summary>
        /// Gets or sets OAuth2 client identifier
        /// </summary>
        public string ClientKeyIdentifier { get; set; }

        /// <summary>
        /// Gets or sets OAuth2 client secret
        /// </summary>
        public string ClientSecret { get; set; }
    }
}