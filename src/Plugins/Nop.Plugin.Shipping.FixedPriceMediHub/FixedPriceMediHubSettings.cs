using Nop.Core.Configuration;

namespace Nop.Plugin.Shipping.FixedPriceMediHub
{
    /// <summary>
    /// Represents settings of the "Fixed or by weight" shipping plugin
    /// </summary>
    public class FixedPriceMediHubSettings : ISettings
    {
        /// <summary>
        /// Gets or sets a value indicating whether to limit shipping methods to configured ones
        /// </summary>
        public bool LimitMethodsToCreated { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the "shipping calculation by weight and by total" method is selected
        /// </summary>
        public bool ShippingByWeightByTotalEnabled { get; set; }
    }
}