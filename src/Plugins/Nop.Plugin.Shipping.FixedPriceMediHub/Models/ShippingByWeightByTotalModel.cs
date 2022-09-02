using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Shipping.FixedPriceMediHub.Models
{
    public class ShippingByWeightByTotalModel : BaseNopEntityModel
    {
        public ShippingByWeightByTotalModel()
        {
            AvailableCountries = new List<SelectListItem>();
            AvailableStates = new List<SelectListItem>();
            AvailableShippingMethods = new List<SelectListItem>();
            AvailableStores = new List<SelectListItem>();
            AvailableWarehouses = new List<SelectListItem>();
        }

        [NopResourceDisplayName("Plugins.Shipping.FixedPriceMediHub.Fields.Store")]
        public int StoreId { get; set; }
        [NopResourceDisplayName("Plugins.Shipping.FixedPriceMediHub.Fields.Store")]
        public string StoreName { get; set; }

        [NopResourceDisplayName("Plugins.Shipping.FixedPriceMediHub.Fields.Warehouse")]
        public int WarehouseId { get; set; }
        [NopResourceDisplayName("Plugins.Shipping.FixedPriceMediHub.Fields.Warehouse")]
        public string WarehouseName { get; set; }

        [NopResourceDisplayName("Plugins.Shipping.FixedPriceMediHub.Fields.Country")]
        public int CountryId { get; set; }
        [NopResourceDisplayName("Plugins.Shipping.FixedPriceMediHub.Fields.Country")]
        public string CountryName { get; set; }

        [NopResourceDisplayName("Plugins.Shipping.FixedPriceMediHub.Fields.StateProvince")]
        public int StateProvinceId { get; set; }
        [NopResourceDisplayName("Plugins.Shipping.FixedPriceMediHub.Fields.StateProvince")]
        public string StateProvinceName { get; set; }

        [NopResourceDisplayName("Plugins.Shipping.FixedPriceMediHub.Fields.Zip")]
        public string Zip { get; set; }

        [NopResourceDisplayName("Plugins.Shipping.FixedPriceMediHub.Fields.ShippingMethod")]
        public int ShippingMethodId { get; set; }
        [NopResourceDisplayName("Plugins.Shipping.FixedPriceMediHub.Fields.ShippingMethod")]
        public string ShippingMethodName { get; set; }

        [NopResourceDisplayName("Plugins.Shipping.FixedPriceMediHub.Fields.WeightFrom")]
        public decimal WeightFrom { get; set; }

        [NopResourceDisplayName("Plugins.Shipping.FixedPriceMediHub.Fields.WeightTo")]
        public decimal WeightTo { get; set; }

        [NopResourceDisplayName("Plugins.Shipping.FixedPriceMediHub.Fields.OrderSubtotalFrom")]
        public decimal OrderSubtotalFrom { get; set; }

        [NopResourceDisplayName("Plugins.Shipping.FixedPriceMediHub.Fields.OrderSubtotalTo")]
        public decimal OrderSubtotalTo { get; set; }

        [NopResourceDisplayName("Plugins.Shipping.FixedPriceMediHub.Fields.AdditionalFixedCost")]
        public decimal AdditionalFixedCost { get; set; }

        [NopResourceDisplayName("Plugins.Shipping.FixedPriceMediHub.Fields.PercentageRateOfSubtotal")]
        public decimal PercentageRateOfSubtotal { get; set; }

        [NopResourceDisplayName("Plugins.Shipping.FixedPriceMediHub.Fields.RatePerWeightUnit")]
        public decimal RatePerWeightUnit { get; set; }

        [NopResourceDisplayName("Plugins.Shipping.FixedPriceMediHub.Fields.LowerWeightLimit")]
        public decimal LowerWeightLimit { get; set; }

        [NopResourceDisplayName("Plugins.Shipping.FixedPriceMediHub.Fields.DataHtml")]
        public string DataHtml { get; set; }

        public string PrimaryStoreCurrencyCode { get; set; }
        public string BaseWeightIn { get; set; }

        public IList<SelectListItem> AvailableCountries { get; set; }
        public IList<SelectListItem> AvailableStates { get; set; }
        public IList<SelectListItem> AvailableShippingMethods { get; set; }
        public IList<SelectListItem> AvailableStores { get; set; }
        public IList<SelectListItem> AvailableWarehouses { get; set; }
    }
}