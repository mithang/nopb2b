

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.ExternalAuth.Database.Models;
using Nop.Services.Authentication.External;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Security;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Plugin.ExternalAuth.Database.Controllers
{
    public class DatabaseAuthenticationController : BasePluginController
    {
        private readonly IExternalAuthenticationService _externalAuthenticationService;
        private readonly ISettingService _settingService; 
        private readonly DatabaseExternalAuthSettings _databaseExternalAuthSettings;
        private readonly IPermissionService _permissionService;
        private readonly ILocalizationService _localizationService;
        

        public DatabaseAuthenticationController(ISettingService settingService,
            DatabaseExternalAuthSettings databaseExternalAuthSettings,
            IPermissionService permissionService,
            ILocalizationService localizationService,
            IExternalAuthenticationService externalAuthenticationService)
        {
            this._settingService = settingService;
            this._databaseExternalAuthSettings = databaseExternalAuthSettings;
            _externalAuthenticationService = externalAuthenticationService;
            _permissionService = permissionService;
            _localizationService = localizationService;
        }

        [AuthorizeAdmin]
        [Area(AreaNames.Admin)]
        public IActionResult Configure()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageExternalAuthenticationMethods))
                return AccessDeniedView();

            //var model = new ConfigurationModel
            //{
            //    ClientId = _facebookExternalAuthSettings.ClientKeyIdentifier,
            //    ClientSecret = _facebookExternalAuthSettings.ClientSecret
            //};

            var model = new ConfigurationModel
            {
                ConnectionString = "",
                SQLQuery = ""
            };
            return View("~/Plugins/Nop.Plugin.ExternalAuth.Database/Views/Configure.cshtml", model);
        }

        [HttpPost]
        [AdminAntiForgery]
        [AuthorizeAdmin]
        [Area(AreaNames.Admin)]
        public IActionResult Configure(ConfigurationModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageExternalAuthenticationMethods))
                return AccessDeniedView();

            if (!ModelState.IsValid)
                return Configure();

            ////save settings
            //_facebookExternalAuthSettings.ClientKeyIdentifier = model.ClientId;
            //_facebookExternalAuthSettings.ClientSecret = model.ClientSecret;
            //_settingService.SaveSetting(_facebookExternalAuthSettings);

            ////clear Facebook authentication options cache
            //_optionsCache.TryRemove(FacebookDefaults.AuthenticationScheme);

            //_notificationService.SuccessNotification(_localizationService.GetResource("Admin.Plugins.Saved"));

            return Configure();
        }


        
        //public async Task<IActionResult> Login(Models.LoginModel model, string returnUrl)
        public async Task<IActionResult> Login(string returnUrl)
        {


                //create external authentication parameters
                var authenticationParameters = new ExternalAuthenticationParameters
                {
                    ProviderSystemName = DatabaseAuthenticationDefaults.SystemName,
                    AccessToken = await HttpContext.GetTokenAsync(DatabaseAuthenticationDefaults.SystemName, "access_token"),
                    Email = "minhit@yahoo.com",
                    ExternalIdentifier = "minhit",
                    ExternalDisplayIdentifier = "minhitdisplay",
                    Claims = new List<ExternalAuthenticationClaim>()
                };

                //authenticate Nop user
                return _externalAuthenticationService.Authenticate(authenticationParameters, returnUrl);

                      
           

        }

                  
    }
}
