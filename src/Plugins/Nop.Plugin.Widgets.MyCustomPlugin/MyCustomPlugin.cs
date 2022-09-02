using Microsoft.AspNetCore.Routing;
using Nop.Core;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Tasks;
using Nop.Plugin.Widgets.MyCustomPlugin;
using Nop.Plugin.Widgets.MyCustomPlugin.Components;
using Nop.Plugin.Widgets.MyCustomPlugin.Models;
using Nop.Plugin.Widgets.MyCustomPlugin.Tasks;
using Nop.Services.Cms;
using Nop.Services.Common;
using Nop.Services.Configuration;
using Nop.Services.Events;
using Nop.Services.Localization;
using Nop.Services.Plugins;
using Nop.Services.Tasks;
using Nop.Web.Framework.Infrastructure;
using Nop.Web.Framework.Menu;
using System.Collections.Generic;
using System.Linq;

namespace Nop.Plugin.Widgets.MyCustomPlugin
{
    public class MyCustomPlugin : BasePlugin, IWidgetPlugin, IAdminMenuPlugin, IConsumer<ConfigurationModel>, IConsumer<CustomerRegisteredEvent>
    {
        private readonly ISettingService _settingService;
        private readonly IWebHelper _webHelper;
        private readonly ILocalizationService _localizationService;
        private readonly IScheduleTaskService _scheduleTaskService;

        public bool HideInWidgetList => false;

        public MyCustomPlugin(ISettingService settingService, IWebHelper webHelper, 
            ILocalizationService localizationService, IScheduleTaskService scheduleTaskService)
        {
            this._webHelper = webHelper;
            this._settingService = settingService;
            this._localizationService = localizationService;
            this._scheduleTaskService = scheduleTaskService;
        }
        public override string GetConfigurationPageUrl()
        {
            return $"{_webHelper.GetStoreLocation()}Admin/MyCustomPlugin/Configure";
        }
        public IList<string> GetWidgetZones()
        {
            return new List<string> { PublicWidgetZones.HomeMyCustomPlugin };
        }
         
        public string GetWidgetViewComponentName(string widgetZone)
        {
            return WidgetsMyCustomPluginViewComponent.ViewComponentName;
        }

        public static string GetTaskName<T>()
        {
            return $"{typeof(T).FullName}, {typeof(T).Assembly.GetName().Name}";
        }

        public override void Install()
        {
            var settings = new MyCustomPluginSettings()
            {
                UseSandbox = true,
                Message = "Hello World"
            };
            _settingService.SaveSetting(settings);

            _localizationService.AddOrUpdatePluginLocaleResource("Plugin.Widgets.MyCustomPlugin.UseSandbox", "UseSandbox");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugin.Widgets.MyCustomPlugin.Message", "Message");

            string taskName = GetTaskName<InsertLogScheduleTask>();
            ScheduleTask task = _scheduleTaskService.GetTaskByType(taskName);
            if (task == null)
            {
                task = new ScheduleTask()
                {
                    Enabled = true,
                    Name = "retailCRM: Export catalog",
                    //Seconds = 60 * 60 * 4,
                    Seconds=30,
                    StopOnError = false,
                    Type = taskName,
                };
                _scheduleTaskService.InsertTask(task);
            }

            base.Install();
        }

        public override void Uninstall()
        {
            _localizationService.DeletePluginLocaleResource("Plugin.Widgets.MyCustomPlugin.UseSandbox");
            _localizationService.DeletePluginLocaleResource("Plugin.Widgets.MyCustomPlugin.Message");
            foreach (string typeName in new string[] {
                            GetTaskName<InsertLogScheduleTask>(),
                           //GetTaskName<DownloadHistoryTask>(),
                           //GetTaskName<ExportCatalogTask>(),
                           //GetTaskName<UploadCustomerTask>(),
                           //GetTaskName<UploadOrderTask>()
            })
            {
                ScheduleTask task = _scheduleTaskService.GetTaskByType(typeName);
                if (task != null)
                    _scheduleTaskService.DeleteTask(task);
            }
            base.Uninstall();
        }

        public void ManageSiteMap(SiteMapNode rootNode)
        {
            var menuItem = new SiteMapNode()
            {
                SystemName = "MyCustomPlugin",
                Title = "MyCustomPlugin Title",
                ControllerName = "MyCustomPlugin",
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

        //Sự kiện khi cấu hình plugin MyCustomPlugin, cùng plugin
        public void HandleEvent(ConfigurationModel eventMessage)
        {
            
        }
        //Sự kiện khi đăng ký một user mới, khác plugin
        public void HandleEvent(CustomerRegisteredEvent eventMessage)
        {
           
        }
    }
}