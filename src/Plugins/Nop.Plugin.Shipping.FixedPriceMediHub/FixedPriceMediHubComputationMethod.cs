using System;
using System.Collections.Generic;
using System.Linq;
using Nop.Core;
using Nop.Core.Domain.Shipping;
using Nop.Plugin.Shipping.FixedPriceMediHub.Data;
using Nop.Plugin.Shipping.FixedPriceMediHub.Services;
using Nop.Services.Catalog;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Plugins;
using Nop.Services.Shipping;
using Nop.Services.Shipping.Tracking;

namespace Nop.Plugin.Shipping.FixedPriceMediHub
{
    /// <summary>
    /// Fixed rate or by weight shipping computation method 
    /// </summary>
    public class FixedPriceMediHubComputationMethod : BasePlugin, IShippingRateComputationMethod
    {
        #region Fields

        private readonly FixedPriceMediHubSettings _fixedByWeightByTotalSettings;
        private readonly ILocalizationService _localizationService;
        private readonly IPriceCalculationService _priceCalculationService;
        private readonly ISettingService _settingService;
        private readonly IShippingByWeightByTotalService _shippingByWeightByTotalService;
        private readonly IShippingService _shippingService;
        private readonly IStoreContext _storeContext;
        private readonly IWebHelper _webHelper;
        private readonly ShippingByWeightByTotalObjectContext _objectContext;

        #endregion

        #region Ctor

        public FixedPriceMediHubComputationMethod(FixedPriceMediHubSettings fixedByWeightByTotalSettings,
            ILocalizationService localizationService,
            IPriceCalculationService priceCalculationService,
            ISettingService settingService,
            IShippingByWeightByTotalService shippingByWeightByTotalService,
            IShippingService shippingService,
            IStoreContext storeContext,
            IWebHelper webHelper,
            ShippingByWeightByTotalObjectContext objectContext)
        {
            _fixedByWeightByTotalSettings = fixedByWeightByTotalSettings;
            _localizationService = localizationService;
            _priceCalculationService = priceCalculationService;
            _settingService = settingService;
            _shippingByWeightByTotalService = shippingByWeightByTotalService;
            _shippingService = shippingService;
            _storeContext = storeContext;
            _webHelper = webHelper;
            _objectContext = objectContext;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Get fixed rate
        /// </summary>
        /// <param name="shippingMethodId">Shipping method ID</param>
        /// <returns>Rate</returns>
        private decimal GetRate(int shippingMethodId)
        {
            return _settingService.GetSettingByKey<decimal>(string.Format(FixedPriceMediHubDefaults.FixedRateSettingsKey, shippingMethodId));
        }

        /// <summary>
        /// Get rate by weight and by total
        /// </summary>
        /// <param name="subTotal">Subtotal</param>
        /// <param name="weight">Weight</param>
        /// <param name="shippingMethodId">Shipping method ID</param>
        /// <param name="storeId">Store ID</param>
        /// <param name="warehouseId">Warehouse ID</param>
        /// <param name="countryId">Country ID</param>
        /// <param name="stateProvinceId">State/Province ID</param>
        /// <param name="zip">Zip code</param>
        /// <returns>Rate</returns>
        private decimal? GetRate(decimal subTotal, decimal weight, int shippingMethodId,
            int storeId, int warehouseId, int countryId, int stateProvinceId, string zip)
        {
            var shippingByWeightByTotalRecord = _shippingByWeightByTotalService.FindRecords(shippingMethodId, storeId, warehouseId, countryId, stateProvinceId, zip, weight, subTotal);
            if (shippingByWeightByTotalRecord == null)
            {
                if (_fixedByWeightByTotalSettings.LimitMethodsToCreated)
                    return null;

                return decimal.Zero;
            }

            //additional fixed cost
            var shippingTotal = shippingByWeightByTotalRecord.AdditionalFixedCost;

            //charge amount per weight unit
            if (shippingByWeightByTotalRecord.RatePerWeightUnit > decimal.Zero)
            {
                var weightRate = Math.Max(weight - shippingByWeightByTotalRecord.LowerWeightLimit, decimal.Zero);
                shippingTotal += shippingByWeightByTotalRecord.RatePerWeightUnit * weightRate;
            }

            //percentage rate of subtotal
            if (shippingByWeightByTotalRecord.PercentageRateOfSubtotal > decimal.Zero)
            {
                shippingTotal += Math.Round((decimal)((((float)subTotal) * ((float)shippingByWeightByTotalRecord.PercentageRateOfSubtotal)) / 100f), 2);
            }

            return Math.Max(shippingTotal, decimal.Zero);
        }

        #endregion

        #region Methods

        /// <summary>
        ///  Gets available shipping options
        /// </summary>
        /// <param name="getShippingOptionRequest">A request for getting shipping options</param>
        /// <returns>Represents a response of getting shipping rate options</returns>
        public GetShippingOptionResponse GetShippingOptions(GetShippingOptionRequest getShippingOptionRequest)
        {
            return new GetShippingOptionResponse()
            {
                ShippingOptions = new List<ShippingOption> {
                    new ShippingOption
                    {
                        Name = "Giao Hàng Tiết Kiệm",
                        Description = "Đường Bộ",
                        Rate = 10
                    },
                    new ShippingOption
                    {
                        Name = "Giao Hàng Tiết Kiệm",
                        Description = "Đường Bay",
                        Rate = 20
                    }
                }
            };
        }

        /// <summary>
        /// Gets fixed shipping rate (if shipping rate computation method allows it and the rate can be calculated before checkout).
        /// </summary>
        /// <param name="getShippingOptionRequest">A request for getting shipping options</param>
        /// <returns>Fixed shipping rate; or null in case there's no fixed shipping rate</returns>
        public decimal? GetFixedRate(GetShippingOptionRequest getShippingOptionRequest)
        {
            return 50;
        }

        /// <summary>
        /// Gets a configuration page URL
        /// </summary>
        public override string GetConfigurationPageUrl()
        {
            return $"{_webHelper.GetStoreLocation()}Admin/FixedByWeightByTotal/Configure";
        }

        /// <summary>
        /// Install plugin
        /// </summary>
        public override void Install()
        {
            //settings
            _settingService.SaveSetting(new FixedPriceMediHubSettings());

            //database objects
            _objectContext.Install();

            //locales
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Shipping.FixedPriceMediHub.AddRecord", "Add record");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Shipping.FixedPriceMediHub.Fields.AdditionalFixedCost", "Additional fixed cost");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Shipping.FixedPriceMediHub.Fields.AdditionalFixedCost.Hint", "Specify an additional fixed cost per shopping cart for this option. Set to 0 if you don't want an additional fixed cost to be applied.");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Shipping.FixedPriceMediHub.Fields.Country", "Country");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Shipping.FixedPriceMediHub.Fields.Country.Hint", "If an asterisk is selected, then this shipping rate will apply to all customers, regardless of the country.");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Shipping.FixedPriceMediHub.Fields.DataHtml", "Data");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Shipping.FixedPriceMediHub.Fields.LimitMethodsToCreated", "Limit shipping methods to configured ones");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Shipping.FixedPriceMediHub.Fields.LimitMethodsToCreated.Hint", "If you check this option, then your customers will be limited to shipping options configured here. Otherwise, they'll be able to choose any existing shipping options even they are not configured here (zero shipping fee in this case).");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Shipping.FixedPriceMediHub.Fields.LowerWeightLimit", "Lower weight limit");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Shipping.FixedPriceMediHub.Fields.LowerWeightLimit.Hint", "Lower weight limit. This field can be used for \"per extra weight unit\" scenarios.");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Shipping.FixedPriceMediHub.Fields.OrderSubtotalFrom", "Order subtotal from");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Shipping.FixedPriceMediHub.Fields.OrderSubtotalFrom.Hint", "Order subtotal from.");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Shipping.FixedPriceMediHub.Fields.OrderSubtotalTo", "Order subtotal to");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Shipping.FixedPriceMediHub.Fields.OrderSubtotalTo.Hint", "Order subtotal to.");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Shipping.FixedPriceMediHub.Fields.PercentageRateOfSubtotal", "Charge percentage (of subtotal)");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Shipping.FixedPriceMediHub.Fields.PercentageRateOfSubtotal.Hint", "Charge percentage (of subtotal).");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Shipping.FixedPriceMediHub.Fields.Rate", "Rate");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Shipping.FixedPriceMediHub.Fields.RatePerWeightUnit", "Rate per weight unit");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Shipping.FixedPriceMediHub.Fields.RatePerWeightUnit.Hint", "Rate per weight unit.");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Shipping.FixedPriceMediHub.Fields.ShippingMethod", "Shipping method");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Shipping.FixedPriceMediHub.Fields.ShippingMethod.Hint", "Choose shipping method.");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Shipping.FixedPriceMediHub.Fields.StateProvince", "State / province");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Shipping.FixedPriceMediHub.Fields.StateProvince.Hint", "If an asterisk is selected, then this shipping rate will apply to all customers from the given country, regardless of the state.");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Shipping.FixedPriceMediHub.Fields.Store", "Store");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Shipping.FixedPriceMediHub.Fields.Store.Hint", "If an asterisk is selected, then this shipping rate will apply to all stores.");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Shipping.FixedPriceMediHub.Fields.Warehouse", "Warehouse");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Shipping.FixedPriceMediHub.Fields.Warehouse.Hint", "If an asterisk is selected, then this shipping rate will apply to all warehouses.");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Shipping.FixedPriceMediHub.Fields.WeightFrom", "Order weight from");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Shipping.FixedPriceMediHub.Fields.WeightFrom.Hint", "Order weight from.");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Shipping.FixedPriceMediHub.Fields.WeightTo", "Order weight to");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Shipping.FixedPriceMediHub.Fields.WeightTo.Hint", "Order weight to.");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Shipping.FixedPriceMediHub.Fields.Zip", "Zip");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Shipping.FixedPriceMediHub.Fields.Zip.Hint", "Zip / postal code. If zip is empty, then this shipping rate will apply to all customers from the given country or state, regardless of the zip code.");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Shipping.FixedPriceMediHub.Fixed", "Fixed Rate");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Shipping.FixedPriceMediHub.Formula", "Formula to calculate rates");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Shipping.FixedPriceMediHub.Formula.Value", "[additional fixed cost] + ([order total weight] - [lower weight limit]) * [rate per weight unit] + [order subtotal] * [charge percentage]");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Shipping.FixedPriceMediHub.ShippingByWeight", "By Weight");

            base.Install();
        }

        /// <summary>
        /// Uninstall plugin
        /// </summary>
        public override void Uninstall()
        {
            //settings
            _settingService.DeleteSetting<FixedPriceMediHubSettings>();

            //fixed rates
            var fixedRates = _shippingService.GetAllShippingMethods()
                .Select(shippingMethod => _settingService.GetSetting(
                    string.Format(FixedPriceMediHubDefaults.FixedRateSettingsKey, shippingMethod.Id)))
                .Where(setting => setting != null).ToList();
            _settingService.DeleteSettings(fixedRates);

            //database objects
            _objectContext.Uninstall();

            //locales
            _localizationService.DeletePluginLocaleResource("Plugins.Shipping.FixedPriceMediHub.AddRecord");
            _localizationService.DeletePluginLocaleResource("Plugins.Shipping.FixedPriceMediHub.Fields.AdditionalFixedCost");
            _localizationService.DeletePluginLocaleResource("Plugins.Shipping.FixedPriceMediHub.Fields.AdditionalFixedCost.Hint");
            _localizationService.DeletePluginLocaleResource("Plugins.Shipping.FixedPriceMediHub.Fields.Country");
            _localizationService.DeletePluginLocaleResource("Plugins.Shipping.FixedPriceMediHub.Fields.Country.Hint");
            _localizationService.DeletePluginLocaleResource("Plugins.Shipping.FixedPriceMediHub.Fields.DataHtml");
            _localizationService.DeletePluginLocaleResource("Plugins.Shipping.FixedPriceMediHub.Fields.LimitMethodsToCreated");
            _localizationService.DeletePluginLocaleResource("Plugins.Shipping.FixedPriceMediHub.Fields.LimitMethodsToCreated.Hint");
            _localizationService.DeletePluginLocaleResource("Plugins.Shipping.FixedPriceMediHub.Fields.LowerWeightLimit");
            _localizationService.DeletePluginLocaleResource("Plugins.Shipping.FixedPriceMediHub.Fields.LowerWeightLimit.Hint");
            _localizationService.DeletePluginLocaleResource("Plugins.Shipping.FixedPriceMediHub.Fields.OrderSubtotalFrom");
            _localizationService.DeletePluginLocaleResource("Plugins.Shipping.FixedPriceMediHub.Fields.OrderSubtotalFrom.Hint");
            _localizationService.DeletePluginLocaleResource("Plugins.Shipping.FixedPriceMediHub.Fields.OrderSubtotalTo");
            _localizationService.DeletePluginLocaleResource("Plugins.Shipping.FixedPriceMediHub.Fields.OrderSubtotalTo.Hint");
            _localizationService.DeletePluginLocaleResource("Plugins.Shipping.FixedPriceMediHub.Fields.PercentageRateOfSubtotal");
            _localizationService.DeletePluginLocaleResource("Plugins.Shipping.FixedPriceMediHub.Fields.PercentageRateOfSubtotal.Hint");
            _localizationService.DeletePluginLocaleResource("Plugins.Shipping.FixedPriceMediHub.Fields.Rate");
            _localizationService.DeletePluginLocaleResource("Plugins.Shipping.FixedPriceMediHub.Fields.RatePerWeightUnit");
            _localizationService.DeletePluginLocaleResource("Plugins.Shipping.FixedPriceMediHub.Fields.RatePerWeightUnit.Hint");
            _localizationService.DeletePluginLocaleResource("Plugins.Shipping.FixedPriceMediHub.Fields.ShippingMethod");
            _localizationService.DeletePluginLocaleResource("Plugins.Shipping.FixedPriceMediHub.Fields.ShippingMethod.Hint");
            _localizationService.DeletePluginLocaleResource("Plugins.Shipping.FixedPriceMediHub.Fields.StateProvince");
            _localizationService.DeletePluginLocaleResource("Plugins.Shipping.FixedPriceMediHub.Fields.StateProvince.Hint");
            _localizationService.DeletePluginLocaleResource("Plugins.Shipping.FixedPriceMediHub.Fields.Store");
            _localizationService.DeletePluginLocaleResource("Plugins.Shipping.FixedPriceMediHub.Fields.Store.Hint");
            _localizationService.DeletePluginLocaleResource("Plugins.Shipping.FixedPriceMediHub.Fields.Warehouse");
            _localizationService.DeletePluginLocaleResource("Plugins.Shipping.FixedPriceMediHub.Fields.Warehouse.Hint");
            _localizationService.DeletePluginLocaleResource("Plugins.Shipping.FixedPriceMediHub.Fields.WeightFrom");
            _localizationService.DeletePluginLocaleResource("Plugins.Shipping.FixedPriceMediHub.Fields.WeightFrom.Hint");
            _localizationService.DeletePluginLocaleResource("Plugins.Shipping.FixedPriceMediHub.Fields.WeightTo");
            _localizationService.DeletePluginLocaleResource("Plugins.Shipping.FixedPriceMediHub.Fields.WeightTo.Hint");
            _localizationService.DeletePluginLocaleResource("Plugins.Shipping.FixedPriceMediHub.Fields.Zip");
            _localizationService.DeletePluginLocaleResource("Plugins.Shipping.FixedPriceMediHub.Fields.Zip.Hint");
            _localizationService.DeletePluginLocaleResource("Plugins.Shipping.FixedPriceMediHub.Fixed");
            _localizationService.DeletePluginLocaleResource("Plugins.Shipping.FixedPriceMediHub.Formula");
            _localizationService.DeletePluginLocaleResource("Plugins.Shipping.FixedPriceMediHub.Formula.Value");
            _localizationService.DeletePluginLocaleResource("Plugins.Shipping.FixedPriceMediHub.ShippingByWeight");

            base.Uninstall();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a shipping rate computation method type
        /// </summary>
        public ShippingRateComputationMethodType ShippingRateComputationMethodType
        {
            get { return ShippingRateComputationMethodType.Offline; }
        }

        /// <summary>
        /// Gets a shipment tracker
        /// </summary>
        public IShipmentTracker ShipmentTracker
        {
            get
            {
                //uncomment a line below to return a general shipment tracker (finds an appropriate tracker by tracking number)
                //return new GeneralShipmentTracker(EngineContext.Current.Resolve<ITypeFinder>());
                return null;
            }
        }

        #endregion
    }
}