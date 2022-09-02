using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Shipping.FixedPriceMediHub.Models
{
    public class ConfigurationModel : BaseSearchModel
    {
        [NopResourceDisplayName("Plugins.Shipping.FixedPriceMediHub.Fields.LimitMethodsToCreated")]
        public bool LimitMethodsToCreated { get; set; }

        public bool ShippingByWeightByTotalEnabled { get; set; }

        public ConfigurationModel()
        {
            AvailableCountries = new List<SelectListItem>();
            AvailableStates = new List<SelectListItem>();
            AvailableShippingMethods = new List<SelectListItem>();
            AvailableStores = new List<SelectListItem>();
            AvailableWarehouses = new List<SelectListItem>();
        }

        [NopResourceDisplayName("Plugins.Shipping.FixedPriceMediHub.Fields.Store")]
        public int SearchStoreId { get; set; }

        [NopResourceDisplayName("Plugins.Shipping.FixedPriceMediHub.Fields.Warehouse")]
        public int SearchWarehouseId { get; set; }

        [NopResourceDisplayName("Plugins.Shipping.FixedPriceMediHub.Fields.Country")]
        public int SearchCountryId { get; set; }

        [NopResourceDisplayName("Plugins.Shipping.FixedPriceMediHub.Fields.StateProvince")]
        public int SearchStateProvinceId { get; set; }

        [NopResourceDisplayName("Plugins.Shipping.FixedPriceMediHub.Fields.Zip")]
        public string SearchZip { get; set; }

        [NopResourceDisplayName("Plugins.Shipping.FixedPriceMediHub.Fields.ShippingMethod")]
        public int SearchShippingMethodId { get; set; }
        
        public IList<SelectListItem> AvailableCountries { get; set; }
        public IList<SelectListItem> AvailableStates { get; set; }
        public IList<SelectListItem> AvailableShippingMethods { get; set; }
        public IList<SelectListItem> AvailableStores { get; set; }
        public IList<SelectListItem> AvailableWarehouses { get; set; }
    }
}