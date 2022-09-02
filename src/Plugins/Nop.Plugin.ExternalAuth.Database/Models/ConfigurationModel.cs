using Nop.Web.Framework;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.ExternalAuth.Database.Models
{
    public class ConfigurationModel : BaseNopModel
    {
        [NopResourceDisplayName("Plugins.ExternalAuth.Database.ConnectionString")]
        public string ConnectionString { get; set; }
        [NopResourceDisplayName("Plugins.ExternalAuth.Database.SQLQuery")]
        public string SQLQuery { get; set; }
    }
}