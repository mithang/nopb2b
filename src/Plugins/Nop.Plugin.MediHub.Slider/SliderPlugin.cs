using System.Collections.Generic;
using Microsoft.AspNetCore.Routing;
using Nop.Core;
using Nop.Core.Infrastructure;
using Nop.Plugin.MediHub.Slider.Data;
using Nop.Services.Cms;
using Nop.Services.Common;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Media;
using Nop.Services.Plugins;
using Nop.Web.Framework.Infrastructure;

namespace Nop.Plugin.MediHub.Slider
{
    /// <summary>
    /// PLugin
    /// </summary>
    public class SliderPlugin : BasePlugin, IMiscPlugin//IWidgetPlugin
    {
        private readonly ILocalizationService _localizationService;
        private readonly IPictureService _pictureService;
        private readonly ISettingService _settingService;
        private readonly IWebHelper _webHelper;
        private readonly INopFileProvider _fileProvider;
        private readonly CustomObjectContext _context;

        public SliderPlugin(ILocalizationService localizationService, CustomObjectContext context,
            IPictureService pictureService,
            ISettingService settingService,
            IWebHelper webHelper,
            INopFileProvider fileProvider)
        {
            _localizationService = localizationService;
            _pictureService = pictureService;
            _settingService = settingService;
            _webHelper = webHelper;
            _fileProvider = fileProvider;
            _context = context;
        }

        ///// <summary>
        ///// Gets a route for provider configuration
        ///// </summary>
        ///// <param name="actionName">Action name</param>
        ///// <param name="controllerName">Controller name</param>
        ///// <param name="routeValues">Route values</param>
        //public void GetConfigurationRoute(out string actionName, out string controllerName, out RouteValueDictionary routeValues)
        //{
        //    actionName = "Configure";
        //    controllerName = "SliderPlugins";
        //    routeValues = new RouteValueDictionary { { "Namespaces", "Nop.Plugin.Medihub.SliderPlugin.Controllers" }, { "area", null } };
        //}

        public override string GetConfigurationPageUrl()
        {
            return _webHelper.GetStoreLocation() + "Admin/SliderPlugin/Configure";
        }

        public override void Install()
        {
            _context.Install();
            base.Install();
        }
        public override void Uninstall()
        {
            _context.Uninstall();
            base.Uninstall();
        }
    }
}