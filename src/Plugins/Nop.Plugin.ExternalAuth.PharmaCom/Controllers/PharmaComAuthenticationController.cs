using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
//using Microsoft.AspNetCore.Authentication.PharmaCom;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Nop.Core;
using Nop.Plugin.ExternalAuth.PharmaCom.Models;
using Nop.Services.Authentication.External;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Plugin.ExternalAuth.PharmaCom.Controllers
{
    public class PharmaComAuthenticationController : BasePluginController
    {
        #region Fields

        private readonly PharmaComExternalAuthSettings _PharmaComExternalAuthSettings;
        private readonly IAuthenticationPluginManager _authenticationPluginManager;
        private readonly IExternalAuthenticationService _externalAuthenticationService;
        private readonly ILocalizationService _localizationService;
        private readonly INotificationService _notificationService;
        //private readonly IOptionsMonitorCache<PharmaComOptions> _optionsCache;
        private readonly IPermissionService _permissionService;
        private readonly ISettingService _settingService;
        private readonly IStoreContext _storeContext;
        private readonly IWorkContext _workContext;

        #endregion

        #region Ctor

        public PharmaComAuthenticationController(PharmaComExternalAuthSettings PharmaComExternalAuthSettings,
            IAuthenticationPluginManager authenticationPluginManager,
            IExternalAuthenticationService externalAuthenticationService,
            ILocalizationService localizationService,
            INotificationService notificationService,
            //IOptionsMonitorCache<PharmaComOptions> optionsCache,
            IPermissionService permissionService,
            ISettingService settingService,
            IStoreContext storeContext,
            IWorkContext workContext)
        {
            _PharmaComExternalAuthSettings = PharmaComExternalAuthSettings;
            _authenticationPluginManager = authenticationPluginManager;
            _externalAuthenticationService = externalAuthenticationService;
            _localizationService = localizationService;
            _notificationService = notificationService;
            //_optionsCache = optionsCache;
            _permissionService = permissionService;
            _settingService = settingService;
            _storeContext = storeContext;
            _workContext = workContext;
        }

        #endregion

        #region Methods

        [AuthorizeAdmin]
        [Area(AreaNames.Admin)]
        public IActionResult Configure()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageExternalAuthenticationMethods))
                return AccessDeniedView();

            var model = new ConfigurationModel
            {
                ClientId = _PharmaComExternalAuthSettings.ClientKeyIdentifier,
                ClientSecret = _PharmaComExternalAuthSettings.ClientSecret
            };

            return View("~/Plugins/ExternalAuth.PharmaCom/Views/Configure.cshtml", model);
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

            //save settings
            _PharmaComExternalAuthSettings.ClientKeyIdentifier = model.ClientId;
            _PharmaComExternalAuthSettings.ClientSecret = model.ClientSecret;
            _settingService.SaveSetting(_PharmaComExternalAuthSettings);

            //clear PharmaCom authentication options cache
            _optionsCache.TryRemove(PharmaComDefaults.AuthenticationScheme);

            _notificationService.SuccessNotification(_localizationService.GetResource("Admin.Plugins.Saved"));

            return Configure();
        }

        public IActionResult Login(string returnUrl)
        {
            var methodIsAvailable = _authenticationPluginManager
                .IsPluginActive(PharmaComAuthenticationDefaults.SystemName, _workContext.CurrentCustomer, _storeContext.CurrentStore.Id);
            if (!methodIsAvailable)
                throw new NopException("PharmaCom authentication module cannot be loaded");

            if (string.IsNullOrEmpty(_PharmaComExternalAuthSettings.ClientKeyIdentifier) ||
                string.IsNullOrEmpty(_PharmaComExternalAuthSettings.ClientSecret))
            {
                throw new NopException("PharmaCom authentication module not configured");
            }

            //configure login callback action
            var authenticationProperties = new AuthenticationProperties
            {
                RedirectUri = Url.Action("LoginCallback", "PharmaComAuthentication", new { returnUrl = returnUrl })
            };
            authenticationProperties.SetString(PharmaComAuthenticationDefaults.ErrorCallback, Url.RouteUrl("Login", new { returnUrl }));

            return Challenge(authenticationProperties, PharmaComDefaults.AuthenticationScheme);
        }

        public async Task<IActionResult> LoginCallback(string returnUrl)
        {
            //authenticate PharmaCom user
            var authenticateResult = await HttpContext.AuthenticateAsync(PharmaComDefaults.AuthenticationScheme);
            if (!authenticateResult.Succeeded || !authenticateResult.Principal.Claims.Any())
                return RedirectToRoute("Login");

            //create external authentication parameters
            var authenticationParameters = new ExternalAuthenticationParameters
            {
                ProviderSystemName = PharmaComAuthenticationDefaults.SystemName,
                AccessToken = await HttpContext.GetTokenAsync(PharmaComDefaults.AuthenticationScheme, "access_token"),
                Email = authenticateResult.Principal.FindFirst(claim => claim.Type == ClaimTypes.Email)?.Value,
                ExternalIdentifier = authenticateResult.Principal.FindFirst(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value,
                ExternalDisplayIdentifier = authenticateResult.Principal.FindFirst(claim => claim.Type == ClaimTypes.Name)?.Value,
                Claims = authenticateResult.Principal.Claims.Select(claim => new ExternalAuthenticationClaim(claim.Type, claim.Value)).ToList()
            };

            //authenticate Nop user
            return _externalAuthenticationService.Authenticate(authenticationParameters, returnUrl);
        }

        #endregion
    }
}