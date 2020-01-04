using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Nop.Core;
using Nop.Core.Domain.Orders;
using Nop.Plugin.Payment.VnPay;
using Nop.Plugin.Payment.VnPay.Components;
using Nop.Services.Cms;
using Nop.Services.Common;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Orders;
using Nop.Services.Payments;
using Nop.Services.Plugins;
using Nop.Web.Framework.Infrastructure;
using Nop.Web.Framework.Menu;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nop.Plugin.Payment.VnPay
{
    public class MyVnPayPlugin : BasePlugin, IPaymentMethod, IAdminMenuPlugin
    {
        //private readonly CheckMoneyOrderPaymentSettings _checkMoneyOrderPaymentSettings;
        private readonly ILocalizationService _localizationService;
        private readonly IPaymentService _paymentService;
        private readonly ISettingService _settingService;
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IWebHelper _webHelper;

        public MyVnPayPlugin(
            ILocalizationService localizationService,
            IPaymentService paymentService,
            ISettingService settingService,
            IShoppingCartService shoppingCartService,
            IWebHelper webHelper)
        {
            _localizationService = localizationService;
            _paymentService = paymentService;
            _settingService = settingService;
            _shoppingCartService = shoppingCartService;
            _webHelper = webHelper;
        }
        public bool HideInWidgetList => false;

        public bool SupportCapture => false;

        public bool SupportPartiallyRefund => false;

        public bool SupportRefund => false;

        public bool SupportVoid => false;

        public RecurringPaymentType RecurringPaymentType => RecurringPaymentType.NotSupported;

        public PaymentMethodType PaymentMethodType => PaymentMethodType.Standard;

        public bool SkipPaymentInfo => false;

        public string PaymentMethodDescription => "DemoPaymentMethodDescription";
        public override string GetConfigurationPageUrl()
        {
            return $"{_webHelper.GetStoreLocation()}Admin/MyVnPayPlugin/Configure";
        }
        public override void Install()
        {
            //var settings = new MyCustomPluginSettings()
            //{
            //    UseSandbox = true,
            //    Message = "Hello World"
            //};
            //_settingService.SaveSetting(settings);

            //_localizationService.AddOrUpdatePluginLocaleResource("Plugin.Widgets.MyCustomPlugin.UseSandbox", "UseSandbox");
            //_localizationService.AddOrUpdatePluginLocaleResource("Plugin.Widgets.MyCustomPlugin.Message", "Message");
            base.Install();
        }

        public override void Uninstall()
        {
            //_localizationService.DeletePluginLocaleResource("Plugin.Widgets.MyCustomPlugin.UseSandbox");
            //_localizationService.DeletePluginLocaleResource("Plugin.Widgets.MyCustomPlugin.Message");
            base.Uninstall();
        }

        public void ManageSiteMap(SiteMapNode rootNode)
        {
            var menuItem = new SiteMapNode()
            {
                SystemName = "Nop.Plugin.Payment.VnPay",
                Title = "Payment VnPay",
                ControllerName = "MyVnPayPlugin",
                ActionName = "Configure",
                Visible = true,
                RouteValues = new RouteValueDictionary() { { "area", "Admin" } },
            };
            var pluginNode = rootNode.ChildNodes.FirstOrDefault(x => x.SystemName == "Third party plugins");
            if (pluginNode != null)
                pluginNode.ChildNodes.Add(menuItem);
            else
                rootNode.ChildNodes.Add(menuItem);
        }

        public ProcessPaymentResult ProcessPayment(ProcessPaymentRequest processPaymentRequest)
        {
            return new ProcessPaymentResult();
        }

        public void PostProcessPayment(PostProcessPaymentRequest postProcessPaymentRequest)
        {
            //throw new System.NotImplementedException();
        }

        public bool HidePaymentMethod(IList<ShoppingCartItem> cart)
        {
            //if (_checkMoneyOrderPaymentSettings.ShippableProductRequired && !_shoppingCartService.ShoppingCartRequiresShipping(cart))
            //    return true;

            return false;
        }

        public decimal GetAdditionalHandlingFee(IList<ShoppingCartItem> cart)
        {
            //return _paymentService.CalculateAdditionalFee(cart,
            //   _checkMoneyOrderPaymentSettings.AdditionalFee, _checkMoneyOrderPaymentSettings.AdditionalFeePercentage);
            return 1;
        }

        public CapturePaymentResult Capture(CapturePaymentRequest capturePaymentRequest)
        {
            return new CapturePaymentResult { Errors = new[] { "Capture method not supported" } };
        }

        public RefundPaymentResult Refund(RefundPaymentRequest refundPaymentRequest)
        {
            return new RefundPaymentResult { Errors = new[] { "Refund method not supported" } };
        }

        public VoidPaymentResult Void(VoidPaymentRequest voidPaymentRequest)
        {
            return new VoidPaymentResult { Errors = new[] { "Void method not supported" } };
        }

        public ProcessPaymentResult ProcessRecurringPayment(ProcessPaymentRequest processPaymentRequest)
        {
            return new ProcessPaymentResult { Errors = new[] { "Recurring payment not supported" } };
        }

        public CancelRecurringPaymentResult CancelRecurringPayment(CancelRecurringPaymentRequest cancelPaymentRequest)
        {
            return new CancelRecurringPaymentResult { Errors = new[] { "Recurring payment not supported" } };
        }

        public bool CanRePostProcessPayment(Order order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            //it's not a redirection payment method. So we always return false
            return false;
        }

        public IList<string> ValidatePaymentForm(IFormCollection form)
        {
            return new List<string>();
        }

        public ProcessPaymentRequest GetPaymentInfo(IFormCollection form)
        {
            return new ProcessPaymentRequest();
        }

        public string GetPublicViewComponentName()
        {
            return "PaymentVnPayPlugin";
        }
    }
}