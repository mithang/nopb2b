using Nop.Core.Configuration;

namespace Nop.Plugin.ExternalAuth.Database
{
    public class DatabaseExternalAuthSettings : ISettings
    {
        public string ConnectionString { get; set; }
        public string SQLQuery { get; set; }
    }
}
