

using Microsoft.AspNetCore.Routing;
using Nop.Core;
using Nop.Services.Authentication.External;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Plugins;

namespace Nop.Plugin.ExternalAuth.Database
{
    public class DatabaseExternalAuthenticationMethod : BasePlugin, IExternalAuthenticationMethod
    {
        #region Fields

        private readonly ISettingService _settingService;
        private readonly ILocalizationService _localizationService;
        private readonly IWebHelper _webHelper;

        #endregion

        #region Ctor

        public DatabaseExternalAuthenticationMethod(ILocalizationService localizationService, ISettingService settingService, IWebHelper webHelper)
        {
            this._settingService = settingService;
            _localizationService = localizationService;
            _webHelper = webHelper;
        }

        #endregion

        //public void GetConfigurationRoute(out string actionName, out string controllerName, out RouteValueDictionary routeValues)
        //{
        //    // set like this if configuration is not required
        //    //actionName = null;
        //    //controllerName = null;
        //    //routeValues = null;

        //    actionName = "Configure";
        //    controllerName = "ExternalAuthDatabase";
        //    routeValues = new RouteValueDictionary() { { "Namespaces", "Nop.Plugin.ExternalAuth.Database.Controllers" }, { "area", null } };
        //}

        //public void GetPublicInfoRoute(out string actionName, out string controllerName, out RouteValueDictionary routeValues)
        //{
        //    actionName = "PublicInfo";
        //    controllerName = "ExternalAuthDatabase";
        //    routeValues = new RouteValueDictionary() { { "Namespaces", "Nop.Plugin.ExternalAuth.Database.Controllers" }, { "area", null } };
        //}

        /// <summary>
        /// Gets a configuration page URL
        /// </summary>
        public override string GetConfigurationPageUrl()
        {
            return $"{_webHelper.GetStoreLocation()}Admin/DatabaseAuthentication/Configure";
        }

        /// <summary>
        /// Gets a name of a view component for displaying plugin in public store
        /// </summary>
        /// <returns>View component name</returns>
        public string GetPublicViewComponentName()
        {
            return DatabaseAuthenticationDefaults.VIEW_COMPONENT_NAME;
        }

        /// <summary>
        /// Install plugin
        /// </summary>
        public override void Install()
        {
            var settings = new DatabaseExternalAuthSettings()
            {
                ConnectionString = "",
                SQLQuery = "",
            };
            _settingService.SaveSetting(settings);

            //locales
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.ExternalAuth.Database.LoginError", "User not found");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.ExternalAuth.Database.Login", "Login using Database account");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.ExternalAuth.Database.ConnectionString", "DB connection string");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.ExternalAuth.Database.ConnectionString.Hint", "Entre your full sql server connection string.");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.ExternalAuth.Database.SQLQuery", "Sql Query");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.ExternalAuth.Database.SQLQuery.Hint", "The SQL query to get a user, this must return [Email], [FirstName] and [LastName] columns. Inputs parameters are @login and @password");
            
            base.Install();
        }

        public override void Uninstall()
        {
            //settings
            _settingService.DeleteSetting<DatabaseExternalAuthSettings>();

            //locales
            _localizationService.DeletePluginLocaleResource("Plugins.ExternalAuth.Database.LoginError");
            _localizationService.DeletePluginLocaleResource("Plugins.ExternalAuth.Database.Login");
            _localizationService.DeletePluginLocaleResource("Plugins.ExternalAuth.Database.ConnectionString");
            _localizationService.DeletePluginLocaleResource("Plugins.ExternalAuth.Database.ConnectionString.Hint");
            _localizationService.DeletePluginLocaleResource("Plugins.ExternalAuth.Database.SQLQuery");
            _localizationService.DeletePluginLocaleResource("Plugins.ExternalAuth.Database.SQLQuery.Hint");
            
            base.Uninstall();
        }
    }
}
